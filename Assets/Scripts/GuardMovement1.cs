using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement1 : MonoBehaviour {
	public float bulletTimer;
	public float bulletSpeed = 10;
	public float speed;
	public float shootInterval;
	private bool movingRight = true;

	public Transform groundDetection;

	//references
	public Transform shootPointLeft;
	public Transform shootPointRight;
	public Transform target;
	public GameObject bullet;
	public GameObject leftCone;
	public GameObject rightCone;

	// Use this for initialization
	void Start () {
		shootInterval = 0.2f;
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.right * speed * Time.deltaTime);

		RaycastHit2D groundInfo = Physics2D.Raycast (groundDetection.position, Vector2.right, 4f);
		if (groundInfo.collider == true) {
			if (movingRight == true) {
				transform.eulerAngles = new Vector3 (0, -180, 0);
				movingRight = false;
			} else {
				transform.eulerAngles = new Vector3 (0, 0, 0);
				movingRight = true;
			}
		}
	}

	public void attack(bool attackingRight) {
		bulletTimer += Time.deltaTime;

		if (bulletTimer >= shootInterval) {
			Vector2 direction = target.transform.position - transform.position;
			direction.Normalize ();

			if (!attackingRight) {
				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;
				Debug.Log ("Attacking");
				bulletTimer = 0;
			}

			if (attackingRight) {
				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointRight.transform.position, shootPointRight.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;
				Debug.Log ("Attacking");
				bulletTimer = 0;

			}
		}
	}
}
