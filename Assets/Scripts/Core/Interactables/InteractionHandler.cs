using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI interactableDisplay;
    [SerializeField]
    private TextMeshProUGUI switchableDisplay;

    public PlayerUnitController PlayerUnit { get; private set; }

    private List<Interactable> interactions = new List<Interactable>();
    private int currentIndex = 0;
    private PlayerInteraction currentInteractionSelected;
    private bool canInteract = true;
    private bool canSwitchInteraction = true;

    private void Awake()
    {
        PlayerUnit = GetComponentInParent<PlayerUnitController>();
        PlayerUnit.EvtInteract += Interact;
        PlayerUnit.EvtSwitch += Switch;
        canInteract = true;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!canInteract)
        {
            return;
        }

        if (currentInteractionSelected != null)
        {
            InteractableStatus res = (InteractableStatus)currentInteractionSelected.Interact(this);
            if(res == InteractableStatus.Failed)
            {
                Debug.LogError(res.ToString());
            }
            if(res == InteractableStatus.InProgress)
            {
                canSwitchInteraction = false;
            }
        }
    }

    public void Switch(InputAction.CallbackContext context)
    {
        if (interactions.Count > 1)
        {
            IncrementIndex();
        }
    }

    public void IncrementIndex()
    {
        if (!canSwitchInteraction)
        {
            return;
        }

        if (currentIndex < interactions.Count)
        {
            currentIndex++;
            SwitchSelected();
        }
        else
        {
            currentIndex = 0;
            SwitchSelected();
        }
    }

    private void SwitchSelected()
    {
        if(currentIndex > interactions.Count)
        {
            return;
        }

        if (currentInteractionSelected != null)
        {

        }
        if (currentIndex < interactions.Count && interactions.Count > 0)
        {

        }
    }

    public void RegisterInteraction(Interactable interactable)
    {
        if(!interactions.Contains(interactable))
        {
            interactions.Add(interactable);
        }
    }

    public void DeregisterInteraction(Interactable interactable)
    {
        if(interactions.Contains(interactable))
        {
            interactions.Remove(interactable);
        }
    }

    public void SetCanSwitchInteractable(bool value)
    {
        canSwitchInteraction = value;
    }

    public void SetCanInteract(bool value)
    {
        canInteract = value;
    }

    private void OnDestroy()
    {
        PlayerUnit.EvtInteract -= Interact;
        PlayerUnit.EvtSwitch -= Switch;
    }
}
