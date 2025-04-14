using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering;

public class CB_Wanderer : CB_Base
{
    Town[] towns;
    [SerializeField] GameObject wandererPrefab;
    Player player;

    public override void Start()
    {
        base.Start();

        player = FindAnyObjectByType<Player>();
        towns = FindObjectsByType<Town>(FindObjectsSortMode.None);
    }


    public override void Act()
    {
        base.Act();
        var nearestTown = towns[0];
        float nearestDis = Vector3.Distance(player.transform.position, nearestTown.transform.position);
        for(int i = 1; i < towns.Length; i++) {
            float dis = Vector3.Distance(player.transform.position, towns[i].transform.position);
            if((dis < nearestDis || nearestTown.IsRevealed) && !towns[i].IsRevealed) {
                nearestTown = towns[i];
                nearestDis = dis;
            }
        }
        if(nearestTown.IsRevealed) return;

        var wandererAI = Instantiate(wandererPrefab, 
        player.transform.position + (nearestTown.transform.position - player.transform.position).normalized * 2
        ,player.transform.rotation).GetComponent<FollowerEntity>();

        wandererAI.destination = nearestTown.transform.position;
    }
}
