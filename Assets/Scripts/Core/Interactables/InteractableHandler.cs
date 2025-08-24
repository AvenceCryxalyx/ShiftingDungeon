using System.Collections.Generic;
using UnityEngine;

public class InteractableHandler : MonoBehaviour
{
    private PlayerUnitController controller;

    private Dictionary<IInteractable, CustomAddedPlayerAction> actions;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.GetComponent<IInteractable>() != null)
            {

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.GetComponent<IInteractable>() != null)
            {

            }
        }
    }
}
