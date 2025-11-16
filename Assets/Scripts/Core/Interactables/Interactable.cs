using System;
using UnityEngine;

public enum InteractableStatus
{
    None = -1,
    Success,
    InProgress,
    Failed,
}

public class Interactable : MonoBehaviour
{
    public bool IsInteractable { get; protected set; }
    public string InteractText { get; protected set; }
    public  PlayerInteraction Interaction { get; protected set; }

    public virtual int Interact(InteractionHandler handler)
    {
        return (int)InteractableStatus.Failed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            InteractionHandler handler = collision.GetComponent<InteractionHandler>();
            if (handler != null)
            {
                handler.RegisterInteraction(this);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            InteractionHandler handler = collision.GetComponent<InteractionHandler>();
            if (handler != null)
            {
                handler.DeregisterInteraction(this);
            }
        }
    }
}
