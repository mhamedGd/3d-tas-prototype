using UnityEngine;

public interface IBuilding
{
    Transform Transform();
    void Init();
    void WrongPlacement();
    void RightPlacement();

    void UnlockedPlacement();

    void ConnectLine(Vector3 toPoint);

    void HideLine();

    void ShowLine();

    void SetOwner(Transform _t);
    Transform GetOwner();
}
