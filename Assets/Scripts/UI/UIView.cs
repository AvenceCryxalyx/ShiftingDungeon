using UnityEngine;

public class UIVIew : UIElement
{
    [SerializeField]
    private UIViewType menuType;

    public UIViewType Type { get { return menuType; } }

    protected override void Start()
    {
        UIManager.Register(this);
    }

    protected override void OnDestroy()
    {
        UIManager.DeRegister(this);
    }
}
