using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{

    public enum PlayerState
    {
        Ground,
        InAir,
        HangingWallL,
        HangingWallR,
        HangingCeiling
    }

    public const float GRAVITY_SCALE = 10f;
    public int speed = 10;
    public float xAxis = 0f;
    public float yAxis = 0f;
    public float jumpFactor = 20.0f;
    public Vector2 startTouch;
    public Vector2 endTouch;
    public bool grounded;
    public GameObject inputHandler;
    public PlayerState state;
    private Animator anim;
    private Rigidbody2D physicsBody;


    // Use this for initialization
    void Start()
    {
        state = PlayerState.Ground;
        physicsBody = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        //Move();
    }

    public void Leap(Vector2 start, Vector2 end)
    {
        
        float thrust = Vector2.Distance(start, end) * jumpFactor;
        if ( (state == PlayerState.Ground && start.y > end.y) ||
             (state == PlayerState.HangingCeiling && start.y < end.y) ||
             (state == PlayerState.HangingWallL && start.x > end.x) ||
             (state == PlayerState.HangingWallR && start.x < end.x) )
        {
            physicsBody.AddForce((start - end).normalized * thrust);
            state = PlayerState.InAir;

        }
    }

    // I had to basically disable these controlls for the touch stuff
    // but I figured it's probably good to keep it around for testing off android.
    // This only works in the editor platform, but all the canvas and buttons I
    // added eat up the jump button presses. Will fix later, but hopefully this
    // helps keep my changes from breaking things.
    public void Move()
    {
        physicsBody.velocity = new Vector2(xAxis * speed, yAxis * speed);

        /*
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
        */

    }

    // MoveLeft and MoveRight are called on the UI button down presses
    public void MoveLeft()
    {
        switch (state)
        {
		     case PlayerState.HangingCeiling:
		     case PlayerState.Ground:
			    anim.SetBool ("Grounded", true);
			    anim.SetBool ("WallClimbing", false);
                xAxis = -1.0f;
                break;

	     	case PlayerState.HangingWallR:
				yAxis = 1.0f;
				anim.SetBool ("Grounded", false);
				anim.SetBool ("WallClimbing", true);
           		break;

            case PlayerState.HangingWallL:
                yAxis = -1.0f;
				anim.SetBool ("Grounded", false);
				anim.SetBool ("WallClimbing", true);
                break;

            case PlayerState.InAir:
            default:
                break;

        }
    }

    public void MoveRight()
    {
        switch (state)
        {
            case PlayerState.HangingCeiling:
            case PlayerState.Ground:
				anim.SetBool ("Grounded", true);
				anim.SetBool ("WallClimbing", false);
                xAxis = 1.0f;
                break;

            case PlayerState.HangingWallR:
				anim.SetBool ("Grounded", false);
				anim.SetBool ("WallClimbing", true);
                yAxis = -1.0f;
                break;

            case PlayerState.HangingWallL:
				anim.SetBool ("Grounded", false);
				anim.SetBool ("WallClimbing", true);
                yAxis = 1.0f;
                break;

            case PlayerState.InAir:
            default:
                break;

        }

    }

    // Stop is called on UI button up and a few times in TouchHandling.cs
    public void Stop()
    {
        xAxis = 0f;
        yAxis = 0f;
		anim.SetBool ("Grounded", true);

        if (state == PlayerState.HangingWallR || state == PlayerState.HangingWallL ||
            state == PlayerState.HangingCeiling)
        {
            physicsBody.velocity = Vector2.zero;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.tag == "Geometry" && state == PlayerState.InAir)
        {
            Vector2 center = gameObject.GetComponent<Collider2D>().bounds.center;
            Vector2 contactPoint = collision.contacts[0].point;
            Vector2 normal = collision.contacts[0].normal;
            Vector2 dir = transform.position - collision.gameObject.transform.position;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                    state = PlayerState.HangingWallR;
                else
                    state = PlayerState.HangingWallL;
            }
            else
            {
                if (dir.y > 0)
                    state = PlayerState.HangingCeiling;
                else
                    state = PlayerState.Ground;
            }

            
            if (normal.y == 1)
            {
                state = PlayerState.Ground;
            }
            else if (normal.y == -1)
            {
                state = PlayerState.HangingCeiling;
            }
            else if (normal.x < 0)
            {
                state = PlayerState.HangingWallR;
            }
            else if (normal.x > 0)
            {
                state = PlayerState.HangingWallL;
            
            
        }
            //grounded = true;
            //state = PlayerState.Ground; */
    }

    /*
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Geometry")
            state = PlayerState.InAir;
    }*/

}
