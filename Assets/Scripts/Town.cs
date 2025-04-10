using UnityEngine;
using FischlWorks_FogWar;
using AndoomiUtils;
using TMPro;

public class Town : Interactable
{
    [SerializeField] Collider bodyCollider;
    Transform _owner;
    Transform _ownerDestTemp;
    bool _revealed;
    public bool IsRevealed => _revealed;

    [SerializeField] Transform unlockTarget;
    bool _mustMove;
    
    CountdownTimer beaconDestructionTimer;
    [SerializeField] float beaconDestructionDelay;

    CameraHandler cameraHandler;
    
    void Start()
    {
        cameraHandler = FindAnyObjectByType<CameraHandler>();
        beaconDestructionTimer = new(beaconDestructionDelay);

        beaconDestructionTimer.OnTimerStop += () => {
            var ttBuilding = _ownerDestTemp.GetComponent<IBuilding>();
            if(ttBuilding == null){
                _mustMove = true;
                return;
            }
            var ttOwner = ttBuilding.GetOwner();
            Destroy(_ownerDestTemp.gameObject);
            _ownerDestTemp = ttOwner;
            if(_ownerDestTemp != null){
                beaconDestructionTimer.Start();   
            }
        };
    }

    void Update()
    {
        beaconDestructionTimer.Tick();
        if(_mustMove) {
            if(Vector3.Distance(transform.position, unlockTarget.position) > 0.05f) {
                transform.position = Vector3.MoveTowards(transform.position, unlockTarget.position, Time.deltaTime * 2);
            }else {
                _mustMove = false;
            }
        }
    }

    public void Unlock(Transform _owner) {
        if(_revealed) return;

        var fogs = FindObjectsByType<csFogWar>(FindObjectsSortMode.None);
        var thisRevealer = new csFogWar.FogRevealer(
                transform,
                6,
                true
        );
        foreach(var f in fogs) {
            f.AddFogRevealer(thisRevealer);
        }

        bodyCollider.isTrigger = false;
        RecurseColor(_owner);
        this._owner = _owner;
        _revealed = true;

        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    public override void Interact(Player p)
    {
        if(!_revealed) return;
        base.Interact(p);
        var tempOwner = _owner;
        _ownerDestTemp = _owner;
        // while(tempOwner != null) {
        //     var ttOwnner = tempOwner.GetComponent<BeaconTower>().GetOwner();
        //     Destroy(tempOwner.gameObject);
        //     tempOwner = ttOwnner.transform;
        // }
        beaconDestructionTimer.Start();
        cameraHandler.Pan(transform.position, 3);
        
    }

    

    void RecurseColor(Transform ownerBeacon) {
        var ownerBuilding = ownerBeacon.GetComponent<IBuilding>();
        if(ownerBuilding == null) {
            if(ownerBeacon.parent != null) ownerBuilding = ownerBeacon.parent.GetComponent<IBuilding>();
            if(ownerBuilding == null) return;
        }

        ownerBuilding.UnlockedPlacement();
        RecurseColor(ownerBuilding.GetOwner());
    }
}
