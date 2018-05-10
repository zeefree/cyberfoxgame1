using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCheck : MonoBehaviour {
	private SimplePlayerMovement player;
	private Animator anim;

	// Use this for initialization
	void Start () {
		player = gameObject.GetComponentInParent<SimplePlayerMovement>();	
		anim = gameObject.GetComponentInParent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{/*
        if (col.gameObject.tag == "Geometry")
        {
            anim.SetBool("Grounded", true);
            player.state = SimplePlayerMovement.PlayerState.Ground;
        }
      */
	}	

	void OnTriggerExit2D(Collider2D col)
	{
        if (col.gameObject.tag == "Geometry")
        {
            anim.SetBool("Grounded", false);
            player.state = SimplePlayerMovement.PlayerState.InAir;
        }

    }

    void OnTriggerStay2D(Collider2D col)
	{
        /*
        if (col.gameObject.tag == "Geometry")
        {
            anim.SetBool("Grounded", true);
            player.state = SimplePlayerMovement.PlayerState.Ground;
        }
        */
	
    }
}
