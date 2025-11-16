using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GatherResultItem : MonoBehaviour
{
    [SerializeField]
    private Image ItemImage;
    [SerializeField]
    private TextMeshProUGUI amountText;

    public Sprite CurrentSprite { get { return ItemImage.sprite; } }

    public void Initialize(Sprite sprite, int amount)
    {
        ItemImage.sprite = sprite;
        amountText.text = amount.ToString();
    }
}
