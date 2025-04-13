using AndoomiUtils;
using UnityEngine;

public class CB_MissingHome : CB_Base
{
    UpgradeCenter hub;
    Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        hub = FindAnyObjectByType<UpgradeCenter>();
        player = FindAnyObjectByType<Player>();
    }
    public override void Act()
    {
        base.Act();
        player.transform.position = hub.transform.position + Random.insideUnitCircle.Vec2ToVec3Z() - Vector3.up * hub.transform.localScale.y/2;
        player.StopInPlace();
    }
}
