using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GuardIdle : MonoBehaviour {

	public float speed;
	public float currentHealth;
	public float maxHealth = 100;

	private bool movingRight = true;

	public Transform groundDetection;
	public Animator anim;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		anim = GetComponent<Animator> ();
		anim.SetBool ("isAlive", false);
	}

	// Update is called once per frame
	void Update () {
		/*
		transform.Translate (Vector2.right * speed * Time.deltaTime);

		RaycastHit2D groundInfo = Physics2D.Raycast (groundDetection.position, Vector2.down, 2f);
		if (groundInfo.collider == false) {
			if (movingRight == true) {
				transform.eulerAngles = new Vector3 (0, -180, 0);
				movingRight = false;
			} else {
				transform.eulerAngles = new Vector3 (0, 0, 0);
				movingRight = true;
			}
		}*/

		anim.SetFloat ("Speed", speed);

		if (currentHealth <= 0) {
			speed = 0f;
			anim.SetBool ("Attacking", false);
			anim.SetFloat ("Speed", 0);
			anim.SetBool ("Alive", true);
			Destroy (gameObject);
		}
	}

	public void Damage(int dmg) {
		currentHealth -= dmg;
	}
}
