using UnityEngine;

public enum InteractableError
{
    Success,
    Failed,
}

public interface IInteractable
{
    public bool IsInteractable { get; }
    public string InteractText { get; }
    public int Interact(PlayerUnitController controller);
    public void OnReachable(PlayerUnitController controller);
    public void OnUnreachable(PlayerUnitController controller);
    public void OnSelected();
    public void OnUnselected();
}
