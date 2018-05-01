using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretAI : MonoBehaviour {
	//integers
	public int currentHealth;
	public int maxHealth;

	//floats
	public float distance;
	public float wakeRange;
	public float shootInterval;
	public float bulletSpeed = 10;
	public float bulletTimer;

	//booleans
	public bool awake = false;
	public bool lookingRight;

	//references
	public GameObject bullet;
	public Transform target;
	public Animator anim;
	public Transform shootPointLeft;
	public Transform shootPointRight;
	public Image healthBar;

	void Awake() {
		anim = gameObject.GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;	
		wakeRange = 26;
		shootInterval = 0.5f;

	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("Awake", awake);
		anim.SetBool ("LookingRight", lookingRight);

		RangeCheck ();

		if (target.transform.position.x > transform.position.x) {
			lookingRight = true;
		}
		if (target.transform.position.x < transform.position.x) {
			lookingRight = false;
		}
	}

	void RangeCheck() {
		distance = Vector3.Distance (transform.position, target.transform.position);

		if (distance < wakeRange) {
			awake = true;
		}

		if (distance > wakeRange) {
			awake = false;
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

				bulletTimer = 0;
			}

			if (attackingRight) {
				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointRight.transform.position, shootPointRight.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;

				bulletTimer = 0;

			}
		}
	}
}
