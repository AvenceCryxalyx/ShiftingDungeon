using UnityEngine;

public class ExitPortal : Interactable
{
    [SerializeField]
    private bool isActive = true;

    private Animator portalAnimator;
    private bool isSelected = false;
    private bool isReacable = false;

    private void Awake()
    {
        InteractText = "Exit Dungeon";
    }

    #region IInteractable Implementations
    public int Interact(PlayerUnitController controller)
    {
        return DoExit();
    }
    #endregion

    #region Virtual Methods
    protected virtual void ActivatePortal()
    {

    }

    protected virtual int DoExit()
    {
        AppInstance.ActiveGameMode.GetComponent<DungeonMode>().BeginExitProcedures();
        return 0;
    }
    #endregion
}
