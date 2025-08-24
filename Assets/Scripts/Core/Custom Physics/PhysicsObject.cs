using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField]
    protected float minGroundNormalY = 0.65f;
    [SerializeField]
    protected float gravityModifier = 1f;
    #endregion

    #region Fields
    protected Rigidbody2D rb2d;
    protected Vector2 groundNormal;
    protected Vector2 targetVelocity;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitbuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected bool isGravityEnabled = false;

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    #endregion

    #region Properties
    public bool IsGrounded { get; protected set; }
    public float MoveX { get { return targetVelocity.x; } }
    public float MoveY {  get { return velocity.y; } }
    #endregion

    #region Virtual Methods
    protected virtual void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void ComputeVelocity() { }
    protected virtual void OnGrounded() { }
    protected virtual void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        targetVelocity = Vector3.zero;
        ComputeVelocity();
    }
    #endregion

    #region Protected Methods
    protected void FixedUpdate()
    {
        if (isGravityEnabled)
        {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        }
        velocity.x = targetVelocity.x;

        IsGrounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        if (!IsGrounded)
        {
            move = Vector2.up * deltaPosition.y;
        }
        Movement(move, true);
    }

    protected void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitbuffer, distance + shellRadius);
            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitbuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;

                if (currentNormal.y > minGroundNormalY)
                {
                    IsGrounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }
    #endregion

    #region Public Methods
    public void SetGravityEnable(bool enable = true)
    {
        isGravityEnabled = enable;
    }
    #endregion
}

