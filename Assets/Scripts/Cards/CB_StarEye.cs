using UnityEngine;

public class CB_StarEye : CB_Base
{
    CameraHandler cameraHandler;

    [SerializeField] float panDistance;
    [SerializeField] float panDuration;
    public override void Start()
    {
        base.Start();
        cameraHandler = FindAnyObjectByType<CameraHandler>();
    }

    public override void Act()
    {
        base.Act();

        cameraHandler.Pan(cameraHandler.transform.position-Camera.main.transform.forward*panDistance, panDuration);
    }
}
