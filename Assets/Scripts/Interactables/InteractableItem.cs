using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

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

    #region Properties
    public bool IsInteractable { get; protected set; }
    public string InteractText { get { return "Pick up"; } }
    #endregion
    public Action<InteractableItem> EvtOnTaken;

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

    private void OnDisable()
    {
        item = null;
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
            if (EvtOnTaken != null)
            {
                EvtOnTaken.Invoke(this);
            }
        }
        return 0;
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
        IsInteractable = true;
    }

    public void OnUnreachable(PlayerUnitController controller)
    {
        IsInteractable = false;
    }
}
