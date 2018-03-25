using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour {

    public int speed = 10;
    public float xAxis;
    public float jumpFactor = 20.0f;
    private bool jumpReady = false;
    public Vector2 startTouch;
    public Vector2 endTouch;
    public bool grounded;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void Move()
    {
        Rigidbody2D physicsBody = gameObject.GetComponent<Rigidbody2D>();

        // This gets the horizontal axis and applies velocity to the player's physics
        xAxis = Input.GetAxis("Horizontal");
        if (!jumpReady)
            physicsBody.velocity = new Vector2(xAxis * speed, physicsBody.velocity.y);

        // This block controlls jumping. As it is here the user presses LMB (which should
        // also register as a touch) and then drags to a position in the opposite direction
        // he/she wants to leap - ie dragging down-left will leap up and to the right. On
        // release of the button/touch we grab the point and then use the angle and distance
        // between the points to add a force making the player jump.
        if (Input.GetMouseButtonDown(0) && grounded)
        {
            jumpReady = true;
            startTouch = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (Input.GetMouseButtonUp(0) && grounded)
        {
            endTouch = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            float thrust = Vector2.Distance(endTouch, startTouch) * jumpFactor;
            physicsBody.AddForce((startTouch-endTouch).normalized * thrust);
            grounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}
