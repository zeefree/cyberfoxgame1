using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCheck : MonoBehaviour {
    private SimplePlayerMovement player;
    private Rigidbody2D rb2d;
	private Animator anim;

    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponentInParent<SimplePlayerMovement>();
        rb2d = gameObject.GetComponentInParent<Rigidbody2D>();
		anim = gameObject.GetComponentInParent<Animator> ();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Geometry")
        {
            player.state = SimplePlayerMovement.PlayerState.HangingWallL;
            rb2d.velocity = Vector2.zero;
            rb2d.gravityScale = 0;
			anim.SetBool ("Grounded", false);
			anim.SetBool ("WallClimbing", true);
        }
        else if (col.gameObject.tag == "VGlass" && player.state == SimplePlayerMovement.PlayerState.InAir)
        {
            col.gameObject.GetComponent<GlassWall>().Explode();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Geometry")
        {
            player.state = SimplePlayerMovement.PlayerState.InAir;
            rb2d.gravityScale = SimplePlayerMovement.GRAVITY_SCALE;
			anim.SetBool ("WallClimbing", false);
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