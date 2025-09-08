using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;

public class MapArea : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] protected Vector2 dimensions;
    [SerializeField] protected Vector2 offset;
    [SerializeField] protected MapAreaSO so;
    #endregion

    #region Fields
    protected BoxCollider2D boxCol;
    protected List<GameObject> objectsInside = new List<GameObject>();
    
    protected Coroutine moveCor;
    private List<GameObject> childObjects;
    private int layer;
    #endregion

    #region Properties
    public bool IsMoving { get; protected set; }
    public Tuple<int,int> Coordinates {  get; protected set; }
    public Vector2 LastPosition { get; protected set; }
    public bool IsMovementLocked { get; protected set; }
    public MapAreaSO AreaInfo { get; protected set; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        if(offset != Vector2.zero)
        {
            boxCol.offset = offset;
        }
        if(dimensions != Vector2.zero)
        {
            boxCol.size = dimensions;
        }
        if (so != null)
        {
            AreaInfo = Instantiate(so);
        }

        //childObjects = GetComponentsInChildren<GameObject>().ToList();
        DungeonMode.Master.Map.Register(this);
        layer = gameObject.layer;
    }

    private void Start()
    {
        if (!AreaInfo)
        {
            return;
        }
        if(!AreaInfo.hasSpecifics)
        {
            return;
        }
        if(AreaInfo.CoordinateX != -1 && AreaInfo.CoordinateY != -1)
        {
            Initialize(new Tuple<int, int>(AreaInfo.CoordinateX, AreaInfo.CoordinateY));
        }
    }
    #endregion

    #region Virtual Methods
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsMoving)
            return;
        if(collision)
        {
            collision.gameObject.transform.SetParent(this.transform);
            if(collision.GetComponent<PlayerUnitController>())
            {
                IsMovementLocked = true;
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (IsMoving)
            return;
        if (collision.GetComponent<PlayerUnitController>())
        {
            IsMovementLocked = false;
        }
    }

    public virtual void Initialize(Tuple<int, int> coordinates)
    {
        so = Instantiate(so);
        LastPosition = new Vector2(coordinates.Item1 * DungeonMaster.MapAreaSizeX, coordinates.Item2* DungeonMaster.MapAreaSizeY);
        Coordinates = coordinates;
        transform.localPosition = LastPosition;
    }

    public virtual void MoveToNewPosition(Vector2 position)
    {
        moveCor = StartCoroutine(MovePositionTask(position, DungeonMaster.MapAreaMoveSpeed));
    }
    public virtual void ChangeCoordinates(Tuple<int,int> newCoord)
    {
        Coordinates = newCoord;
    }
    public virtual void Load() { }
    public virtual void Unload() { }
    #endregion


    public void InterruptMovement()
    {
        if (!IsMoving)
            return;

        StopCoroutine(moveCor);
        moveCor = null;
        IsMoving = false;
        transform.position = LastPosition;
    }

    private IEnumerator MovePositionTask(Vector2 position, float speed)
    {
        Vector3 direction = (position - (Vector2)transform.localPosition);
        while (true)
        {
            transform.localPosition += direction.normalized * speed * Time.deltaTime;
            if (Vector3.Distance(transform.localPosition, position) < 0.3f)
            {
                transform.localPosition = position;
                LastPosition = transform.localPosition;
                Coordinates = new Tuple<int, int>((int)LastPosition.x / DungeonMaster.MapAreaSizeX, (int)LastPosition.y / DungeonMaster.MapAreaSizeY);
                IsMoving = false;
                break;
            }
            yield return null;
        }
        yield return null;
    }
}
