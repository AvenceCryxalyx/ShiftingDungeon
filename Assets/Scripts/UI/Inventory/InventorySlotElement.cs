using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotElement : MonoBehaviour, IDropHandler
{
    public int Index { get; protected set; }
    public Transform ParentAfterDrag { get; protected set; }

    public DraggableItemImage DraggableItem { get; protected set; }

    private void Awake()
    {
        DraggableItem = GetComponentInChildren<DraggableItemImage>();
    }

    public void Initialize(Item item, int index)
    {
        Index = index;
        UpdateItem(item);
    }

    public void UpdateItem(Item item)
    {
        DraggableItem.Setup(item);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItemImage item = dropped.GetComponent<DraggableItemImage>();
        DraggableItem.transform.SetParent(item.transform.parent);
        item.ParentAfterDrag = transform;
    }
}
