using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float minGroundNormalY = .65f;
    //Affects how object responds to gravity
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;



    private Vector2 deltaPosition;
    private Vector2 moveAlongGround;
    private Vector2 move;
    private Vector2 currentNormal;

    void OnEnable() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start() {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update() {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity() {

    }

    void FixedUpdate() {
        //Every frame we apply gravity
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        //Handle X Movement
        //Target velocity is set in the Compute velocity function, and this will control our x movement
        velocity.x = targetVelocity.x;

        grounded = false;

        deltaPosition = velocity * Time.deltaTime;

        moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        //Handle Y Movement
        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement) {
        float distance = move.magnitude;

        if (distance > minMoveDistance) {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++) {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++) {
                currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY) {
                    grounded = true;
                    if (yMovement) {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0) {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

}