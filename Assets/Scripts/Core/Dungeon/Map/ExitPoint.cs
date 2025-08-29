using UnityEngine;

public class ExitRoom : MonoBehaviour, IInteractable
{
    [SerializeField]
    private bool isActive = true;

    private Animator portalAnimator;
    private bool isSelected = false;
    private bool isReacable = false;

    #region IInteractable Implementations
    public int Interact(PlayerUnitController controller)
    {
        return DoExit();
    }

    public string InteractText()
    {
        return "Exit Dungeon";
    }

    public bool IsInteractable()
    {
        return isSelected;
    }

    public void OnReachable(PlayerUnitController controller)
    {
        isReacable = true;
    }

    public void OnSelected()
    {
        isSelected = true;
    }

    public void OnUnreachable(PlayerUnitController controller)
    {
        isReacable = false;
        isSelected = false;
    }

    public void OnUnselected()
    {
        isSelected = false;
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
