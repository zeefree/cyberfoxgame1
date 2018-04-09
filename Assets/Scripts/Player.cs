using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float maxSpeed = 3;
	public float speed = 50f;
	public float jumpPower = 150f;

	public bool grounded;

	//private Rigidbody2D rb2d;
	private Animator anim;

	public float currentHealth;
	public float maxHealth = 100f;

	public Image healthbar;
	public GameObject gameobject;

	void Start () {
		//rb2d = gameObject.GetComponent<Rigidbody2D> ();

		anim = gameObject.GetComponent<Animator> ();
		gameobject = GameObject.FindGameObjectWithTag ("CurrentHealth");

		currentHealth = maxHealth;
	}

	// Update is called once per frame
	void Update () {
		anim.SetBool ("Grounded", grounded);
		anim.SetFloat ("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));

		if (Input.GetAxis ("Horizontal") < -0.1f) {
			//this is where things need to be changed to make the health
			//bar static and in the next if

			transform.localScale = new Vector3 (-2, 2, 1);
			healthbar.transform.localScale = new Vector3 (-1, 1, 1);
			//gameObject.transform. = new Vector3 (1, 1, 1);
		}

		if (Input.GetAxis ("Horizontal") > 0.1f) {
			transform.localScale = new Vector3 (2, 2, 1);
			healthbar.transform.localScale = new Vector3 (1, 1, 1);
		}

		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}

		if (currentHealth <= 0) {
			Die ();
		}
	}

	void Die() {
		//restarts hopefully?
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);﻿
	}

	public void Damage(int dmg) {
		currentHealth -= dmg;

		healthbar.fillAmount = currentHealth / maxHealth;
	}
		
}