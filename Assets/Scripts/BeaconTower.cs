using FischlWorks_FogWar;
using UnityEngine;

public class BeaconTower : BaseBuilding
{
    Transform parent;
    
    csFogWar.FogRevealer thisRevealer;
    void AddThisToRevealers() {
        var fogs = FindObjectsByType<csFogWar>(FindObjectsSortMode.None);
        thisRevealer = new csFogWar.FogRevealer(
                transform,
                3,
                true
        );
        foreach(var f in fogs) {
            f.AddFogRevealer(thisRevealer);
        }
    }

    void RemoveThisFromRevealers() {
        var fogs = FindObjectsByType<csFogWar>(FindObjectsSortMode.None);

        foreach(var f in fogs) {
            for(int i = 0; i < f._FogRevealers.Count; i++) {
                if(f._FogRevealers[i] == thisRevealer) {
                    f.RemoveFogRevealer(i);
                }
            }
        }
    }
    

    void OnDestroy()
    {
        RemoveThisFromRevealers();       
    }

    public override void Init()
    {
        base.Init();
        AddThisToRevealers();
        var cols = Physics.OverlapSphere(transform.position, fogSight);
        foreach(var c in cols) {
            if(c.tag == "Town") {
                c.GetComponent<Town>().Unlock(transform);
            }
        }
    }
}
