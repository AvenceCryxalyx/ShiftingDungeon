using UnityEngine;

public class ExitRoom : MonoBehaviour, IInteractable
{
    [SerializeField]
    private bool isActive = true;

    private Animator portalAnimator;
    private bool isSelected = false;
    private bool isReacable = false;

    public string InteractText { get { return "Exit Dungeon"; } }
    public bool IsInteractable { get; protected set; }

    #region IInteractable Implementations
    public int Interact(PlayerUnitController controller)
    {
        return DoExit();
    }

    public void OnReachable(PlayerUnitController controller)
    {
        IsInteractable = true;
    }

    public void OnSelected()
    {
        isSelected = true;
    }

    public void OnUnreachable(PlayerUnitController controller)
    {
        IsInteractable = false;
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
