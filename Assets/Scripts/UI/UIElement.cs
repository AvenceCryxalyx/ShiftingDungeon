using UnityEngine;
using System;
using System.Collections;

public class UIElement : MonoBehaviour
{
    [SerializeField]
    private string id;

    public Action<UIElement> EvtShown;
    public Action<UIElement> EvtHidden;

    public string Id { get; protected set; }

    public bool IsShown { get; private set; }

    protected virtual void Start()
    {
        UIManager.Register(Id, this);
    }

    protected virtual void OnDestroy()
    {
        UIManager.Deregister(Id);
    }

    public void Show()
    {
        OnShow();

        gameObject.SetActive(true);

        IsShown = true;

        if (EvtShown != null)
        {
            EvtShown.Invoke(this);
        }

        OnShown();
    }
    public void Hide()
    {
        OnHide();

        gameObject.SetActive(false);

        IsShown = false;

        if (EvtHidden != null)
        {
            EvtHidden.Invoke(this);
        }

        OnHidden();
    }

    protected virtual void OnShow() { }
    protected virtual void OnShown() { }
    protected virtual void OnHide() { }
    protected virtual void OnHidden() { }
}
