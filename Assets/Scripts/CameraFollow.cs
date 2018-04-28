using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	private Vector2 velocity;

	public float smoothTimeY = 0.05f;
	public float smoothTimeX = 0.05f;
	public float offset = 0.05f;

	public GameObject player;

	// Use this for initialization
	void Start () {

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
			
	}

	void FixedUpdate()
	{
		float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

		transform.position = new Vector3 (posX, posY, transform.position.z);
	}
}

