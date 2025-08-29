using UnityEngine;

public class Player : SimpleSingleton<Player>
{
    [SerializeField]
    private PlayerUnitController UnitPrefab;

    public PlayerInputAction Inputs { get; protected set; }
    public Health Health { get; protected set; }
    public PlayerUnitController Unit { get; protected set; }
    public Inventory Inventory { get; protected set; }
    //public Currency Wallet { get; protected set; }
    protected override void Awake()
    {
        base.Awake();
        Inventory = GetComponentInChildren<Inventory>();
        //Wallet = GetComponentInChildren<Currency>();
        Health = new Health(1, 100);
        Inputs = new PlayerInputAction();
    }

    public void RegisterUnit(PlayerUnitController controller)
    {
        if (Unit != null)
        {
            Debug.LogError("Another PlayerUnitController is present in the scene");
            return;
        }
        Unit = controller;
        Unit.Initialize(Inputs);
    }
}
