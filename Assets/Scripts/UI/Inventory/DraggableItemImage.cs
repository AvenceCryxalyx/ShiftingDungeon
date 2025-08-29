using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItemImage : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IDropHandler
{
    public Action<Item> EvtClicked;
    public Action<int> EvtDragEnd;
    public Action EvtSwapped;
    private Image image;
    public Item Item {  get; private set; }

    public Transform ParentAfterDrag { get; set; }

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    public void Setup(Item item)
    {
        Item = item;
        if (item != null)
        {
            image.sprite = item.Icon;
            image.color = Color.white;
        }
        else
        {
            image.color = new Color(image.color.r,image.color.g,image.color.b, 0f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EvtDragEnd != null)
        {
            EvtDragEnd.Invoke(-1);
        }
        transform.SetParent(ParentAfterDrag);
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (EvtClicked != null && Item != null)
        {
            EvtClicked.Invoke(Item);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItemImage draggable = dropped.GetComponent<DraggableItemImage>();
        if (Item is LockedItem)
        {
            LockedItem locked = Item as LockedItem;
            if(!locked.Open(draggable.Item))
            {
                SwapItems(draggable);
            }
        }
        else
        {
            SwapItems(draggable);
        }
    }

    private void SwapItems(DraggableItemImage draggable)
    {
        Item myItem = Item;
        Setup(draggable.Item);
        draggable.Setup(myItem);

        if (EvtSwapped != null)
        {
            EvtSwapped.Invoke();
        }
    }
}
