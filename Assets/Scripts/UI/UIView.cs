using UnityEngine;

public class UIVIew : UIElement
{
    [SerializeField]
    private UIViewType menuType;

    public UIViewType Type { get { return menuType; } }

    protected virtual void Start()
    {
        UIManager.Register(this);
    }

    private void OnDestroy()
    {
        UIManager.DeRegister(this);
    }
}
