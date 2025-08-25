using UnityEngine;

public class GetOnLadderAction : CustomAddedPlayerAction
{
    private PlayerUnitController controller;
    
   public GetOnLadderAction(GameObject source, bool isEnabled = true)
    {
        this.source = source;
        IsEnabled = isEnabled;
    }

    public override void Do()
    {

    }

    public override void Initialize(PlayerUnitController controller)
    {
        this.controller = controller;
    }
}
