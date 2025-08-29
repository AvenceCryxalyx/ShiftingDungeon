using UnityEngine;
using System;

public class UIElement : MonoBehaviour
{
    [SerializeField]
    private string id;

    public Action<UIElement> EvtShown;
    public Action<UIElement> EvtHidden;

    public bool IsShown { get; private set; }

    public void Show()
    {
        OnShow();

        if(EvtShown != null)
        {
            EvtShown.Invoke(this);
        }
        IsShown = true;
    }
    public void Hide()
    {
        OnHide();

        if (EvtHidden != null)
        {
            EvtHidden.Invoke(this);
        }
        IsShown = false;
    }

    public virtual void OnShow() { }
    public virtual void OnHide() { }
}
