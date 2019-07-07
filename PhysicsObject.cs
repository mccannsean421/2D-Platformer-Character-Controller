using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f; // allows to control the gravity 

    protected bool grounded;
    protected Vector2 groundNormal;

    /* 
        Other classes will inherit velocity
        but we don't want it to be accessible
        from outside the class
     */
     protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.1f;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);
    private ContactFilter2D contactFilter;


    void OnEnable () 
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }


    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask (gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() 
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime; //using default gravity

        grounded = false;

        //predict the next position the object will be in, based on gravity 
        Vector2 deltaPosition = velocity * Time.deltaTime; //change in position

        Vector2 move = Vector2.up * deltaPosition.y;

        Movement (move, true);
    } 

    void Movement(Vector2 move, bool yMovement) 
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance) 
        {
            int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear ();
            for (int i = 0; i < count; i++) {
                hitBufferList.Add (hitBuffer [i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++) 
            {
                Vector2 currentNormal = hitBufferList [i].normal;
                if (currentNormal.y > minGroundNormalY) 
                {
                    grounded = true;
                    if (yMovement) 
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot (velocity, currentNormal);
                if (projection < 0) 
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList [i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
