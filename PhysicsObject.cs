using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{

    public float gravityModifier = 1f; // allows to control the gravity 

    /* 
        Other classes will inherit velocity
        but we don't want it to be accessible
        from outside the class
     */
     protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected const float minMove = 0.001f;
    protected const float shellRadius = 0.1f;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
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

        //predict the next position the object will be in, based on gravity 
        Vector2 deltaPosition = velocity * Time.deltaTime; //change in position

        Vector2 move = Vector2.up * deltaPosition.y;

        Movement (move);
    } 

    void Movement(Vector2 move) 
    {

        float distance = move.magnitude;

        if (distance > minMove) {
            //Check to see if rigid body 2d will overlap with anything in the next frame
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
        }

        rb2d.position = rb2d.position + move;
    }
}
