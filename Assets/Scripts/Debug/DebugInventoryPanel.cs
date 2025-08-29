using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DebugInventoryPanel : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Dropdown dropDown;
    [SerializeField]
    DebugMenu menu;
    [SerializeField]
    private UnityEngine.UI.Button addButton;
    [SerializeField]
    private UnityEngine.UI.Button removeButton;

    private void Start()
    {
        dropDown.ClearOptions();
        List<TMPro.TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
        
        foreach(ItemDataSO so in DebugMenu.GetListOfItemsSO())
        {
            TMPro.TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = so.name;
            optionDatas.Add(data);
        }
        
        dropDown.AddOptions(optionDatas);
        addButton.onClick.AddListener(OnAddClicked);
        removeButton.onClick.AddListener(OnRemoveClicked);

    }

    private void OnAddClicked()
    {
        Player.instance.Inventory.AddItem(ItemManager.instance.GetItem(dropDown.captionText.text));
    }

    private void OnRemoveClicked()
    {
        Player.instance.Inventory.RemoveItem(ItemManager.instance.GetItem(dropDown.captionText.text));
    }
}
