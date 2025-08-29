using UnityEngine;

public enum InteractableError
{
    Success,
    Failed,
}

public interface IInteractable
{
    public string InteractText();
    public int Interact(PlayerUnitController controller);
    public bool IsInteractable();
    public void OnReachable(PlayerUnitController controller);
    public void OnUnreachable(PlayerUnitController controller);
    public void OnSelected();
    public void OnUnselected();
}
