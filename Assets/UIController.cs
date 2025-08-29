using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryUI;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryUI.gameObject.SetActive(!InventoryUI.activeSelf);
        }
    }
}
