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
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

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
    }
}
