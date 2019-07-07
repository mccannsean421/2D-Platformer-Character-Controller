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


    void OnEnable () 
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }


    // Start is called before the first frame update
    void Start()
    {
        
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
        rb2d.position = rb2d.position + move;
    }
}
