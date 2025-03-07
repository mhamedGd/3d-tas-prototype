using System;
using AndoomiUtils;
using Pathfinding;
using UnityEngine;

public class Player : MonoBehaviour
{
    int coins;
    public int Coins => coins;
    public void IncreaseCoinCount(int _increment) => coins += _increment;

    Plane _raycastPlane;
    [SerializeField] float speed;
    public void SetSpeed(float _s) => speed = _s;
    public float Speed => speed;
    [SerializeField] LayerMask interactionMask;
    FollowerEntity _followerEntity;
    [SerializeField] Transform marker;

    Action<Player> _currentInteractMethod = null;
    Action<Player> _currentStopInteractMethod = null;
    Interactable _currentInteractable = null;
    bool _isInteracting;

    // Enemy Related
    public CountdownTimer _attackTimer;
    [SerializeField] float attackTime;
    [SerializeField] ParticleSystem attackEffect;
    [SerializeField] float damage;
    public float Damage => damage;
    public void SetDamage(float newDamage) => damage = newDamage;

    CountdownTimer _pathTimer;
    Vector3 _targetPosition;
    readonly Vector3 _forbiddenPosition = new Vector3{x=1000,y=1000,z=1000};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _followerEntity = GetComponent<FollowerEntity>();
        _raycastPlane = new Plane(Vector3.up, Vector3.zero);

        _attackTimer = new(attackTime, true);
        _attackTimer.OnTimerStart += () =>
        {
            attackEffect.Play();
        };

        _targetPosition = _forbiddenPosition;
        _pathTimer = new(0.1f, true);
        _pathTimer.OnTimerStart += () =>
        {
            //UpdatePath();
        };
        _pathTimer.Start();
    }

    void UpdatePath()
    {
        if (_currentInteractable != null)
        {
            _followerEntity.destination = _currentInteractable.transform.position;
            _targetPosition = _currentInteractable.transform.position;
        }
        else if (_targetPosition != _forbiddenPosition) _followerEntity.destination = _targetPosition;

        marker.position = _targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            MoveToMouse(speed);
        }
        UpdatePath();

        if (!_isInteracting && _currentInteractMethod != null && _followerEntity.reachedEndOfPath)
        {
            _currentInteractMethod.Invoke(this);
            _currentStopInteractMethod = _currentInteractable.StopInteract;
            _isInteracting = true;
        }

        _attackTimer.Tick();
        _pathTimer.Tick();
    }

    void MoveToMouse(float speed)
    {
        void StopInteractingWith(Interactable inter)
        {
            if (_currentStopInteractMethod != null && _currentInteractable != inter)
            {
                _isInteracting = false;
                _currentStopInteractMethod.Invoke(this);
                _currentStopInteractMethod = null;
            }
        }
        void ResetTargetInteractable()
        {
            if (_currentStopInteractMethod != null) _currentStopInteractMethod.Invoke(this);
            _currentStopInteractMethod = null;
            _isInteracting = false;
            _currentInteractable = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_raycastPlane.Raycast(ray, out float en))
        {
            _followerEntity.maxSpeed = speed;
            _followerEntity.stopDistance = 0;
            //_followerEntity.destination = ray.GetPoint(en);
            _targetPosition = ray.GetPoint(en);

            _currentInteractMethod = null;
            //_currentStopInteractMethod = null;
            //_currentInteractable = null;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, interactionMask))
            {
                var inter = hit.collider.GetComponent<Interactable>();
                if (inter != null)
                {
                    _followerEntity.stopDistance = inter.StopDistance;
                    _targetPosition = inter.transform.position;
                    //_followerEntity.destination = inter.transform.position;

                    // Hitting another interactable
                    StopInteractingWith(inter);
                    
                    _currentInteractMethod = inter.Interact;
                    _currentInteractable = inter;
                } else
                {
                    // Hitting the floor
                    ResetTargetInteractable();
                }
            }


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Coin")
        {
            Destroy(other.gameObject);
            coins++;
        }
    }
}
