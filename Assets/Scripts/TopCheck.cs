using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCheck : MonoBehaviour {
    private SimplePlayerMovement player;
    private Rigidbody2D rb2d;
	private Animator anim;
	private SpriteRenderer spriRend;


    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponentInParent<SimplePlayerMovement>();
        rb2d = gameObject.GetComponentInParent<Rigidbody2D>();
		anim = gameObject.GetComponentInParent<Animator> ();
		spriRend = gameObject.GetComponentInParent<SpriteRenderer> ();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if (col.gameObject.tag == "Geometry")
        {
            player.state = SimplePlayerMovement.PlayerState.HangingCeiling;
            rb2d.velocity = Vector2.zero;
            rb2d.gravityScale = 0;
			//
			anim.SetBool("Grounded", true);
			spriRend.flipY = true;
        }
        */
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Geometry")
        {
            player.state = SimplePlayerMovement.PlayerState.InAir;
            rb2d.gravityScale = SimplePlayerMovement.GRAVITY_SCALE;
			anim.SetBool ("Grounded", false);
			spriRend.flipY = false;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        /*
        if (col.gameObject.tag == "Geometry")
        {
            player.state = SimplePlayerMovement.PlayerState.HangingCeiling;
        }
        */
    }
}