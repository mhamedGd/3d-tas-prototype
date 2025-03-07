using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] float stopDistance;
    public float StopDistance => stopDistance;

    public UnityEvent onInteract = new();
    public UnityEvent onStopInteract = new();

    public virtual void Interact(Player p)
    {
        onInteract.Invoke();
        print("Start");
    }

    public virtual void StopInteract(Player p)
    {
        onStopInteract.Invoke();
        print("Stop");
    }
}
