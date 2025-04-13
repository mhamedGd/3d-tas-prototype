using Pathfinding;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    FollowerEntity followerEntity;
    [SerializeField] float detectionRadius;
    void Start()
    {
        followerEntity = GetComponent<FollowerEntity>();
    }

    bool _isFound = false;
    // Update is called once per frame
    void Update()
    {
        if(_isFound) return;
        var cols = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach(var c in cols) {
            if(c.tag == "Player" && followerEntity.reachedEndOfPath){
                Destroy(gameObject, 3);
                _isFound = true;
                break;
            }
        }
    }
}
