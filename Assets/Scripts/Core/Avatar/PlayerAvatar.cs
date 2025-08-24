using UnityEngine;

public class PlayerAvatar : Avatar
{
    private PlayerUnitController _playerUnitController;

    protected override void Awake()
    {
        base.Awake();
        _playerUnitController = GetComponentInParent<PlayerUnitController>();    
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool("grounded", _playerUnitController.IsGrounded);
            animator.SetFloat("moveX", Mathf.Abs(_playerUnitController.InputX));
            animator.SetFloat("moveY", _playerUnitController.MoveY);
            animator.SetInteger("speedMod", _playerUnitController.SpeedModifier);
        }
        if (_playerUnitController.MoveX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (_playerUnitController.MoveX < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
