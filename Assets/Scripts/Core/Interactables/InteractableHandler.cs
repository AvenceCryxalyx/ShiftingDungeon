using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableHandler : MonoBehaviour
{
    private PlayerUnitController controller;
    private List<IInteractable> interactableList = new List<IInteractable>();

    private int currentIndex = 0;
    private IInteractable currentSelected = null;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerUnitController>();
        controller.EvtInteract += Interact;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (currentSelected != null)
        {
            InteractableError res = (InteractableError)currentSelected.Interact(controller);
            if(res != InteractableError.Success)
            {
                Debug.LogError(res.ToString());
            }
        }
    }

    public void IncrementIndex()
    {
        if (currentIndex < interactableList.Count)
        {
            currentIndex++;
            SwitchSelected();
        }
    }

    public void DecrementIndex()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            SwitchSelected();
        }
    }

    private void SwitchSelected()
    {
        if (currentIndex < interactableList.Count && interactableList.Count > 0)
        {
            currentSelected = interactableList[currentIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
        {
            return;
        }

        IInteractable interactable = collision?.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactableList.Add(interactable);
            interactable.OnReachable(controller);
        }
        
        if(currentSelected == null && interactableList.Count > 0)
        {
            currentSelected = interactableList[currentIndex];
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null)
        {
            return;
        }

        IInteractable interactable = collision?.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.OnUnreachable(controller);
            interactableList.Remove(interactable);
            if(currentIndex > interactableList.Count)
            {
                currentIndex--;
                currentSelected = (currentSelected == interactable) ? interactableList[currentIndex] : currentSelected;
            }
            else
            {
                currentSelected = null;
            }
        }
    }
}
