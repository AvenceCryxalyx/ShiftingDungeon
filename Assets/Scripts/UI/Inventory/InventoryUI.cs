using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : UIVIew
{
    #region  Inspector Fields
    [SerializeField,Header("Prefab")]
    private InventorySlotElement SlotPrefab;
    [SerializeField, Header("Setup")]
    private Transform slotsParent;
    [SerializeField]
    private TextMeshProUGUI selectedItemName;
    [SerializeField]
    private TextMeshProUGUI selectedItemValue;
    [SerializeField]
    private TextMeshProUGUI selectedItemDescription;
    [SerializeField]
    private Image selectedItemImage;
    #endregion

    private Dictionary<int ,InventorySlotElement> slots = new Dictionary<int, InventorySlotElement>();
    private List<int> availableSlots = new List<int>();
    private Inventory inventory;

    protected override void Start()
    {
        base.Start();
        inventory = Player.instance.Inventory;
        inventory.EvtItemDropped += OnRemovedFromInventory;
        inventory.EvtItemRemoved += OnRemovedFromInventory;
        inventory.EvtItemPickedUp += OnAddToInventory;
        LoadSlots();
    }

    public override void OnShow()
    {
        base.OnShow();
        LoadSlots();
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public void LoadSlots()
    {
        CheckSlotCount();
        CheckSlotItems();
        CheckInventoryItems();
    }

    private void CheckSlotCount()
    {
        int maxSlotCount = Player.instance.Inventory.MaxSlotCount;
        int currentSlotCount = slots.Count;
        if (slots.Count < maxSlotCount)
        {
            for (int i = 0; i < maxSlotCount - currentSlotCount; i++)
            {
                InventorySlotElement element = Instantiate(SlotPrefab, slotsParent);
                slots.Add(i, element);
                element.Initialize(null, i);
                element.DraggableItem.EvtClicked += OnItemClicked;
                element.DraggableItem.EvtSwapped += OnSlotSwapped;
            }
        }
    }

    private void CheckSlotItems()
    {
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i].DraggableItem.Item != null)
            {
                continue;
            }
            else
            {
                availableSlots.Add(i);
            }
        }
        availableSlots.Sort();
    }

    private void CheckInventoryItems()
    {
        Queue<Item> newItems = new Queue<Item>();
        bool isNew = false;
        IEnumerable slotsWithItems = slots.Values.Where(x => x.DraggableItem.Item == null);
        for (int i = 0; i < inventory.Items.Count(); i++)
        {
            isNew = true;
            foreach(InventorySlotElement element in slotsWithItems)
            {
                if (element.DraggableItem.Item == inventory.Items.ElementAt(i)) 
                {
                    isNew = false;
                    continue;
                }
            }
            if(isNew)
                newItems.Enqueue(inventory.Items.ElementAt(i));
        }

        foreach (Item item in newItems)
        {
            slots[availableSlots[0]].UpdateItem(item);
            availableSlots.RemoveAt(0);
        }
    }

    public void OnRemovedFromInventory(Item item)
    {
        foreach(InventorySlotElement element in slots.Values)
        {
            if(element.DraggableItem.Item == item)
            {
                element.UpdateItem(null);
                availableSlots.Add(element.Index);
                break;
            }
        }
        availableSlots.Sort();
    }

    public void OnAddToInventory(Item item)
    {
        if (availableSlots.Count > 0)
        {
            slots[availableSlots[0]].UpdateItem(item);
            availableSlots.RemoveAt(0);
        }
        availableSlots.Sort();
    }

    private void OnItemClicked(Item item)
    {
        selectedItemName.text = item.Name;
        selectedItemDescription.text = item.Description;
        selectedItemValue.text = string.Format("{0} Gold", item.Value.Current);
        selectedItemImage.sprite = item.Icon;
    }

    private void OnSlotSwapped()
    {
        CheckSlotItems();
    }
}
