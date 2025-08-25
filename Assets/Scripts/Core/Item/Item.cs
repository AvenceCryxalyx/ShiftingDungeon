using UnityEngine;

public class Item : MonoBehaviour
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }   
    public float CurrentValue { get; protected set; }
    public Sprite Icon { get; protected set; }
    public virtual void Initialize() { }
    public virtual void Use() { }
}
