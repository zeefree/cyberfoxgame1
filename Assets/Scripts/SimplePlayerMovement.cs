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
    public float maxJump = 100000.0f;
    public GameObject inputHandler;
    public PlayerState state;
    private Animator anim;
    private Rigidbody2D physicsBody;


    // Use this for initialization
    void Awake()
    {
        state = PlayerState.Ground;
        physicsBody = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        inputHandler = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        // For the editor to allow for keyboard controlled movement
#if UNITY_EDITOR
        float edX = Input.GetAxis("Horizontal");
        if (edX != 0)
            physicsBody.velocity = new Vector2(edX * speed, physicsBody.velocity.y);
#endif
            
        Move();
    }

    // Method that makes the player jump
    public void Leap(Vector2 start, Vector2 end)
    {
        float thrust = Vector2.Distance(start, end) * jumpFactor;
        thrust = Mathf.Clamp(thrust, 0.0f, maxJump);

        // These checks basically keep the player from jumping into the 
        // walls /ground and getting stuck
        if ( (state == PlayerState.Ground && start.y > end.y) ||
             (state == PlayerState.HangingCeiling && start.y < end.y) ||
             (state == PlayerState.HangingWallL && start.x > end.x) ||
             (state == PlayerState.HangingWallR && start.x < end.x) )
        {
            physicsBody.AddForce((start - end).normalized * thrust);
            state = PlayerState.InAir;

        }
    }

    // Method that moves the player when he uses controls
    public void Move()
    {
        if (state != PlayerState.InAir)
            physicsBody.velocity = new Vector2(xAxis * speed, yAxis * speed);

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

}
