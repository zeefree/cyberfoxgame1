using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour {

    public int speed = 10;
    public float xAxis = 0f;
    public float jumpFactor = 20.0f;
    private bool jumpReady = false;
    public Vector2 startTouch;
    public Vector2 endTouch;
    public bool grounded;
	public GameObject inputHandler;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

	public void Leap(Vector2 start, Vector2 end)
	{
		if (grounded && start.y > end.y)
		{
			float thrust = Vector2.Distance(start, end) * jumpFactor;
			Rigidbody2D physicsBody = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D>();
			physicsBody.AddForce((start-end).normalized * thrust);
			grounded = false;
		}
	}

	// I had to basically disable these controlls for the touch stuff
	// but I figured it's probably good to keep it around for testing off android.
	// This only works in the editor platform, but all the canvas and buttons I
	// added eat up the jump button presses. Will fix later, but hopefully this
	// helps keep my changes from breaking things.
    private void Move()
    {

#if UNITY_EDITOR
		Rigidbody2D physicsBody = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D>();
        //Rigidbody2D physicsBody = gameObject.GetComponent<Rigidbody2D>();

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
			jumpReady = false;
        }

#endif

    }

	// MoveLeft and MoveRight are called on the UI button down presses
	public void MoveLeft()
	{
//		if (inputHandler.GetComponent<TouchHandling>().state == 
//			TouchHandling.TouchState.Move)
//		{
			xAxis = -1.0f;
//		}
//		else 
//		{
//			xAxis = 0f;
//		}
	}

	public void MoveRight()
	{
//		if (inputHandler.GetComponent<TouchHandling>().state == 
//			TouchHandling.TouchState.Move)
//		{
			xAxis = 1.0f;
//		}
//
//			xAxis = 0f;
//
	}

	// Stop is called on UI button up and a few times in TouchHandling.cs
	public void Stop()
	{
		xAxis = 0f;
	}


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}
