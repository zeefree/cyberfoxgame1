using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCheck : MonoBehaviour {
	private SimplePlayerMovement player;

	// Use this for initialization
	void Start () {
		player = gameObject.GetComponentInParent<SimplePlayerMovement>();	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		player.state = SimplePlayerMovement.PlayerState.Ground;
	}	

	void OnTriggerExit2D(Collider2D col)
	{
        player.state = SimplePlayerMovement.PlayerState.InAir;
    }

    void OnTriggerStay2D(Collider2D col)
	{
        player.state = SimplePlayerMovement.PlayerState.Ground;
    }
}
