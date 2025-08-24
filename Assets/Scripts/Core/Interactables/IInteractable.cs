using UnityEngine;

public interface IInteractable
{
    public CustomAddedPlayerAction GetAction();
    public void Interact();
    public void IsInteractable();
    public void OnReachable();
    public void OnUnreachable();
}
