using Unity.VisualScripting;
using UnityEngine;

public class WorldInfo : MonoBehaviour
{
    [SerializeField] Vector2 worldBounds;
    public Vector2 WorldBounds => worldBounds;

    public static WorldInfo Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null) Instance = this;
        if (this != Instance) Destroy(this);
    }
}
