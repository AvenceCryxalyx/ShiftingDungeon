using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationView : UIVIew
{
    [SerializeField]
    protected Button confirmButton;
    [SerializeField]
    protected Button cancelButton;

    public Action EvtOnConfirm;
    public Action EvtOnCancel;

    protected virtual void Awake()
    {
        confirmButton.onClick.AddListener(Confirm);
        cancelButton.onClick.AddListener(Cancel);
    }

    private void Confirm()
    {
        if(EvtOnConfirm != null)
        {
            EvtOnConfirm.Invoke();
        }
    }

    private void Cancel()
    {
        if (EvtOnCancel != null)
        {
            EvtOnCancel.Invoke();
        }
    }
}
