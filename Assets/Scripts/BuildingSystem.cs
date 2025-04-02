using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] LayerMask buildingMask;
    [SerializeField, Range(3, 10)] float buildingRange;
    [SerializeField] Player player;
    [SerializeField] GameObject beaconPrefab;
    [SerializeField] Button beaconButton;
    IBuilding _currentBuild = null;
    Transform _currBuildOwner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    RaycastHit _currentHit;
    bool _canBuild = false;
    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _currentHit, player.InteractionMask)) {
            if(_currentBuild != null) {
                _currentBuild.Transform().position = _currentHit.point;
                var allBs = Physics.OverlapSphere(_currentBuild.Transform().position, buildingRange, buildingMask, QueryTriggerInteraction.Ignore);
                _canBuild = allBs.Length > 0;
                
                if(_canBuild){
                    _currBuildOwner = allBs[0].transform;
                    _currentBuild.RightPlacement();
                    _currentBuild.ConnectLine(allBs[0].transform.position);
                    _currentBuild.ShowLine();
                }
                else{
                    _currentBuild.WrongPlacement();
                    _currentBuild.HideLine();
                }
            }
        }

        if(Input.GetMouseButtonDown(0)) {
            if(_currentBuild == null || !_canBuild) return;
            if(_currBuildOwner.parent == null) _currentBuild.SetOwner(_currBuildOwner);
            else _currentBuild.SetOwner(_currBuildOwner.parent);
            _currentBuild.Init();
        }
    }

    void LateUpdate()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(_currentBuild == null || !_canBuild) {
                return;
            }
            beaconButton.interactable = true;
            _currentBuild = null;
            player.FreezeDelay(false, .25f);
        }
    }

    public void BuildBeacon() {
        _currentBuild = Instantiate(beaconPrefab, _currentHit.point, beaconPrefab.transform.rotation).GetComponent<IBuilding>();
        beaconButton.interactable = false;
        player.Freeze(true);
    }
}
