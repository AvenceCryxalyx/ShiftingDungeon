using UnityEngine;
using UnityEngine.UI;

public class ResultSalesPanel : MonoBehaviour
{
    [SerializeField]
    private Image itemImagePrefab;
    [SerializeField]
    private Transform itemImageParent;

    public void AddItemSoldImage(Sprite sprite)
    {
        Instantiate(itemImagePrefab, itemImageParent).sprite = sprite;
    }
}
