using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string label;
    [SerializeField] float stopDistance;
    public float StopDistance => stopDistance;

    public UnityEvent onInteract = new();
    public UnityEvent onStopInteract = new();

    public virtual void Interact(Player p)
    {
        onInteract.Invoke();
    }

    public virtual void StopInteract(Player p)
    {
        onStopInteract.Invoke();
    }
}
