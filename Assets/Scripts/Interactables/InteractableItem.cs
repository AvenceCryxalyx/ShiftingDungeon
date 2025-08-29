using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class InteractableItem : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Item.ItemRarity RarityDrop;
    [SerializeField]
    private bool useWeightedGacha;
    [SerializeField]
    private Item item;
    [SerializeField]
    private GameObject selectedVisual;

    private bool canPickup = false;

    public void SetItem(Item item)
    {
        if(this.item == null)
        {
            this.item = item;
            this.item.transform.SetParent(this.transform);
        }
        else
        {
            Debug.LogWarning("Trying to overwrite the item assigned to drop object");
        }
    }

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
            gameObject.PoolOrDestroy();
        }
        return 0;
    }

    public bool IsInteractable()
    {
        return canPickup;
    }

    public void OnSelected()
    {
        if (selectedVisual != null)
        {
            selectedVisual.SetActive(true);
        }
    }

    public void OnUnselected()
    {
        if (selectedVisual != null)
        {
            selectedVisual.SetActive(false);
        }
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
