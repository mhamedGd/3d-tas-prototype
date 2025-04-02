using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector2 padding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(!_isPanning) {
            float xAxis = 0;
            float yAxis = 0;
            xAxis = Input.GetAxis("Horizontal");
            yAxis = Input.GetAxis("Vertical");
            float xMinBorder = -WorldInfo.Instance.WorldBounds.x + padding.x;
            float xMaxBorder = WorldInfo.Instance.WorldBounds.x - padding.x;

            float yMinBorder = -WorldInfo.Instance.WorldBounds.y + padding.y;
            float yMaxBorder = WorldInfo.Instance.WorldBounds.y - padding.y;

            transform.position += (Vector3.right * xAxis + Vector3.forward * yAxis) * speed * Time.deltaTime;
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, xMinBorder, xMaxBorder),
                0,
                Mathf.Clamp(transform.position.z, yMinBorder, yMaxBorder)
            );
        }else {
            transform.position = Vector3.MoveTowards(transform.position, _panningTarget, Time.deltaTime * speed * 3);
            _panningTimer += Time.deltaTime;
            if(_panningTimer >= _panningDuration) {
                _isPanning = false;
            }
        }

    }

    bool _isPanning = false;
    Vector3 _panningTarget;
    float _panningTimer;
    float _panningDuration;
    public void Pan(Vector3 _target, float _duration)
    {
        _panningTimer = 0;
        _isPanning = true;
        _panningTarget = _target;
        _panningDuration = _duration;
    }
}
