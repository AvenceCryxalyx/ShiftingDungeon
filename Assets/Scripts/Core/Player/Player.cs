using UnityEngine;

public class Player : SimpleSingleton<Player>
{
    [SerializeField]
    private PlayerUnitController UnitPrefab;

    public Health Health { get; protected set; }
    public PlayerUnitController Unit { get; protected set; }
    public Inventory Inventory { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Inventory = GetComponentInChildren<Inventory>();

    }
}
