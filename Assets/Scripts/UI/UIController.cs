using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryUI;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(!InventoryUI.activeSelf)
            {
                Player.instance.PausedInputs();
            }
            else
            {
                Player.instance.ResumeInputs();
            }
                InventoryUI.gameObject.SetActive(!InventoryUI.activeSelf);
        }
    }
}
