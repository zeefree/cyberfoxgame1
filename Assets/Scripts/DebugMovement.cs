using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMovement : MonoBehaviour {

	public int speed = 5;
    private Rigidbody2D r2d;
    // Use this for initialization
	void Start ()
    {
        r2d = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
     {
        var x = Input.GetAxis("Horizontal");

        r2d.velocity = (transform.right * x * speed);
    }
}
