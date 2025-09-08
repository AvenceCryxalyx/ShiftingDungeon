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
    #endregion

    #region Properties
    public bool IsMoving { get; protected set; }
    public Tuple<int,int> Coordinates {  get; protected set; }
    public Vector2 LastPosition { get; protected set; }
    public bool IsMovementLocked { get; protected set; }
    public MapAreaSO AreaInfo { get; protected set; }
    #endregion

    #region Unity Methods
    protected virtual void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        if (offset != Vector2.zero)
        {
            boxCol.offset = offset;
        }
        if (dimensions != Vector2.zero)
        {
            boxCol.size = dimensions;
        }
        if (so != null)
        {
            AreaInfo = Instantiate(so);
        }

        //childObjects = GetComponentsInChildren<GameObject>().ToList();
        DungeonMode.Master.Map.Register(this);
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
    public virtual void ChangeCoordinates(Tuple<int,int> newCoord)
    {
        Coordinates = newCoord;
    }
    public virtual void Load() { }
    public virtual void Unload() { }
    #endregion

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, dimensions);   
    }
    #endif
}
