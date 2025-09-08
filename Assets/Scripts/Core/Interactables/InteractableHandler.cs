using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableHandler : MonoBehaviour
{
    private PlayerUnitController controller;
    private List<IInteractable> interactableList = new List<IInteractable>();

    private int currentIndex = 0;
    private IInteractable currentSelected = null;

    [SerializeField]
    private TextMeshProUGUI interactableDisplay;
    [SerializeField]
    private TextMeshProUGUI switchableDisplay;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerUnitController>();
        controller.EvtInteract += Interact;
        controller.EvtSwitch += Switch;
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

    public void Switch(InputAction.CallbackContext context)
    {
        if (interactableList.Count > 1)
        {
            IncrementIndex();
        }
    }

    public void IncrementIndex()
    {
        if (currentIndex < interactableList.Count)
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
        if(currentIndex >  interactableList.Count)
        {
            return;
        }

        if (currentSelected != null)
        {
            currentSelected.OnUnselected();
        }
        if (currentIndex < interactableList.Count && interactableList.Count > 0)
        {
            currentSelected = interactableList[currentIndex];
            currentSelected.OnSelected();
            interactableDisplay.text = "[E] " + currentSelected.InteractText;
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
            currentSelected.OnSelected();
            interactableDisplay.text = "[E] " + currentSelected.InteractText;
        }
        if(interactableList.Count > 1 && !switchableDisplay.transform.parent.gameObject.activeSelf)
        {
            switchableDisplay.transform.parent.gameObject.SetActive(true);
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
                IncrementIndex();
                interactableDisplay.text = "[E] " + currentSelected.InteractText;
            }
            else
            {
                currentSelected = null;
            }
        }
    }

    private void Update()
    {
        if(currentSelected == null)
        {
            if(interactableList.Count > 0)
            {
                IncrementIndex();
            }
            if(interactableDisplay.transform.parent.gameObject.activeSelf)
            {
                interactableDisplay.transform.parent.gameObject.SetActive(false);
            }
            if (switchableDisplay.transform.parent.gameObject.activeSelf && interactableList.Count < 1)
            {
                switchableDisplay.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            if(interactableList.Count > 0)
            {
                switchableDisplay.gameObject.SetActive(true);
            }
            interactableDisplay.gameObject.SetActive(true);
        }
    }
}
