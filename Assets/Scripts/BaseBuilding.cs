using UnityEngine;

public class BaseBuilding : MonoBehaviour, IBuilding
{
    [SerializeField] protected int fogSight;

    [SerializeField] MeshRenderer bodyMesh;

    [SerializeField] Color wrongPlacement;
    [SerializeField] Color rightPlacement;
    [SerializeField] Color unlockedPlacement;

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Collider buildingCollider;

    protected Transform owner;
    public void ConnectLine(Vector3 toPoint) {
        lineRenderer.SetPosition(0, bodyMesh.transform.position);
        lineRenderer.SetPosition(1, toPoint);
    }

    public void HideLine() {
        lineRenderer.enabled =false;
    }

    public void ShowLine() {
        lineRenderer.enabled = true;
    }

    public virtual void Init()
    {
        buildingCollider.enabled = true;
    }

    public void RightPlacement()
    {
        bodyMesh.materials[0].SetColor("_BaseColor", rightPlacement);
    }

    public Transform Transform() => transform;

    public void WrongPlacement()
    {
        bodyMesh.materials[0].SetColor("_BaseColor", wrongPlacement);
    }

    public void SetOwner(Transform _t) => owner = _t;
    public Transform GetOwner() => owner;

    public void UnlockedPlacement()
    {
        bodyMesh.materials[0].SetColor("_BaseColor", unlockedPlacement);
        lineRenderer.enabled = false;
    }
}
