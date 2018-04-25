using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour {

	Rigidbody2D rb2d;
	public Transform originPoint;
	private Vector2 dir = new Vector2(1, 0);

	public float range;
	public float speed;


	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);


		Debug.DrawRay (originPoint.position, dir * range);
		RaycastHit2D hit = Physics2D.Raycast (originPoint.position, dir, range);
		if (hit == true) {
			if (hit.collider.CompareTag ("Geometry")) {
				flip ();
				speed *= -1;
				dir *= -1;
			}
		}
	}

	void fixedUpdate() {
		//rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
		//rb2d.AddForce = new Vector2 (speed, rb2d.velocity.y);
	}

	void flip() {
		Vector3 theScale = transform.localScale;
		theScale.x *= 1;
		transform.localScale = theScale;


		transform.GetComponentInChildren<Transform> ();

	}
}
