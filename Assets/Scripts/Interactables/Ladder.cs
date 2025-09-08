using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class Ladder : MonoBehaviour, IInteractable
{

    [SerializeField]
    private ShadowCasterGroup2D shadowCaster;
    [SerializeField]
    private MovementRestriction restriction;
    private PlayerUnitController currentUser;
    private bool inUse = false;

    #region Properties
    public bool IsInteractable { get; private set; }
    public string InteractText { get { return "Use Ladder"; } }
    #endregion

    public int Interact(PlayerUnitController controller)
    {
        if(!IsInteractable)
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
        controller.transform.localPosition = new Vector2(this.transform.localPosition.x, controller.transform.localPosition.y);
        controller.SetXMovementEnable(false);
        controller.SetYMovementEnable(true);
        controller.SetGravityEnable(false);
        currentUser = controller;
        inUse = true;
        if(shadowCaster)
        {
            shadowCaster.enabled = false;
        }
        
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
        currentUser = null;
        inUse = false;
        if (shadowCaster)
        {
            shadowCaster.enabled = true;
        }
    }

    public void OnReachable(PlayerUnitController controller)
    {
        IsInteractable = true;
    }

    public void OnUnreachable(PlayerUnitController controller)
    {
        IsInteractable = false;
        if (inUse)
        {
            GetOff(controller, true);
        }
    }

    public void OnSelected()
    {
        
    }

    public void OnUnselected()
    {
        
    }

    private void Update()
    {
        if(!inUse)
        {
            return;
        }

        if((currentUser.InputY < 0 && currentUser.transform.localPosition.y < restriction.MinY) ||
            currentUser.InputY > 0 && currentUser.transform.localPosition.x > restriction.MaxY)
        {
            GetOff(currentUser, true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(restriction.MinX, restriction.MinY), new Vector3(restriction.MaxX, restriction.MaxY));
    }
}
