using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCheck : MonoBehaviour {
    private SimplePlayerMovement player;
    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponentInParent<SimplePlayerMovement>();
        rb2d = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Geometry")
        {
            player.state = SimplePlayerMovement.PlayerState.HangingWallL;
            rb2d.velocity = Vector2.zero;
            rb2d.gravityScale = 0;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Geometry")
        {
            player.state = SimplePlayerMovement.PlayerState.InAir;
            rb2d.gravityScale = SimplePlayerMovement.GRAVITY_SCALE;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Geometry")
        {
            player.state = SimplePlayerMovement.PlayerState.HangingWallL;
        }
    }
}