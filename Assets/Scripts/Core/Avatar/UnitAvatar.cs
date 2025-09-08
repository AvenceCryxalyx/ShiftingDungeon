using UnityEngine;

public class UnitAvatar : Avatar
{
    private UnitController boundController;

    protected override void Awake()
    {
        base.Awake();
        boundController = GetComponentInParent<UnitController>();    
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool("grounded", boundController.IsGrounded);
            animator.SetFloat("moveX", Mathf.Abs(boundController.Direction));
            animator.SetFloat("moveY", boundController.MoveY);
            animator.SetInteger("speedMod", boundController.SpeedModifier);
        }
        if (boundController.MoveX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (boundController.MoveX < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
