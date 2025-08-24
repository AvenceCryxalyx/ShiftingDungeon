using UnityEngine;

public class GetOnLadderAction : CustomAddedPlayerAction
{
    private PlayerUnitController controller;
    
    GetOnLadderAction(GameObject source, bool isEnabled = true)
    {
        this.source = source;
        IsEnabled = isEnabled;
    }

    public override void Do()
    {
        controller.GetComponentInChildren<Avatar>().SetBool("onLadder", true);
        controller.transform.localPosition = new Vector2(source.transform.localPosition.x, controller.transform.position.y);
        controller.SetXMovementEnable(false);
        controller.SetGravityEnable(false);
    }

    public override void Initialize(PlayerUnitController controller)
    {
        this.controller = controller;
    }
}
