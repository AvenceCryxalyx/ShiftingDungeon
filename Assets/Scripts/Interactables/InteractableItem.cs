using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    private bool canPickup = false;
    private Item item;
    public string InteractText()
    {
        return "Pick up";
    }

    public int Interact(PlayerUnitController controller)
    {
        if (item == null)
        {
            return 1;
        }
        int res = Player.instance.Inventory.AddItem(item);
        if (res == 0)
        {
            Destroy(this);
        }
        return 0;
    }

    public bool IsInteractable()
    {
        return canPickup;
    }

    public void OnReachable(PlayerUnitController controller)
    {
        canPickup = true;
    }

    public void OnUnreachable(PlayerUnitController controller)
    {
        canPickup = false;
    }
}
