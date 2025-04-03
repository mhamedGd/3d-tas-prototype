using UnityEngine;

public class CB_BeaconTower : CB_Base
{
    BuildingSystem bs;
    public override void Start()
    {
        base.Start();
        bs = FindAnyObjectByType<BuildingSystem>();

    }
    public override void Act()
    {
        base.Act();
        bs.BuildBeacon();
    }
}
