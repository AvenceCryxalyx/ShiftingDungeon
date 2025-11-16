using UnityEngine;

public abstract class PlayerInteraction
{
    public string InteractText { get; }
    public abstract int Interact(InteractionHandler handler);
}
