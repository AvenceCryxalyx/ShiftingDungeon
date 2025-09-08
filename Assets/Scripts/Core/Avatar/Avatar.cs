using UnityEngine;

public class Avatar : MonoBehaviour
{
    #region Fields
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected bool isFlipped = false;
    #endregion

    #region Unity Methods
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Public Methods
    public void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }
    public void SetBool(string name, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(name, value);
        }
    }

    public void SetInt(string name, int value)
    {
        if (animator != null)
        {
            animator.SetInteger(name, value);
        }
    }

    public void SetFloat(string name, float value)
    {
        if(animator != null)
        {
            animator.SetFloat(name, value);
        }
    }

    public void SetTrigger(string name)
    {
        if(animator != null)
        {
            animator.SetTrigger(name);
        }
    }
    #endregion
}
