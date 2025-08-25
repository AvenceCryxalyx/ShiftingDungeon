using UnityEngine;
using UnityEngine.InputSystem.XR;
using static Unity.VisualScripting.Member;

public class Ladder : MonoBehaviour, IInteractable
{
    private bool inUse = false;
    private bool canInteract = false;

    public int Interact(PlayerUnitController controller)
    {
        if(!IsInteractable())
        {
            return 1;
        }
        controller.Avatar.SetTrigger("LadderInteract");
        if (!inUse)
        {
            GetOn(controller);
        }
        else
        {
            GetOff(controller, false);
        }
        return 0;
    }

    public void GetOn(PlayerUnitController controller)
    {
        controller.ChangeLayer(LayerMask.NameToLayer("IgnoreFlooring"));
        controller.GetComponent<Collider2D>().enabled = false;
        controller.GetComponent<Collider2D>().enabled = true;
        controller.Avatar.SetBool("onLadder", true);
        controller.transform.localPosition = new Vector2(this.transform.localPosition.x, controller.transform.position.y);
        controller.SetXMovementEnable(false);
        controller.SetYMovementEnable(true);
        controller.SetGravityEnable(false);
        inUse = true;
    }

    public void GetOff(PlayerUnitController controller, bool forced)
    {
        controller.ChangeLayer(LayerMask.NameToLayer("Player"));
        controller.GetComponent<Collider2D>().enabled = false;
        controller.GetComponent<Collider2D>().enabled = true;
        controller.Avatar.SetBool("onLadder", false);
        if (forced)
        {
            controller.Avatar.SetTrigger("LadderInteract");
        }
        controller.SetXMovementEnable(true);
        controller.SetYMovementEnable(false);
        controller.SetGravityEnable(true);
        inUse = false;
    }
    public string InteractText()
    {
        return "Climb";
    }

    public bool IsInteractable()
    {
        return canInteract;
    }

    public void OnReachable(PlayerUnitController controller)
    {
        canInteract = true;
    }

    public void OnUnreachable(PlayerUnitController controller)
    {
        canInteract = false;
        if (inUse)
        {
            GetOff(controller, true);
        }
    }
}
