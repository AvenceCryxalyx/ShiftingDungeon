using UnityEngine;

public class Effect : MonoBehaviour
{
    public GameObject Source { get; protected set; }

    public void Initialize(GameObject source)
    {
        Source = source;
    }

    public virtual void ApplyEffect() { }
    public virtual void RemoveEffect() { }
}
