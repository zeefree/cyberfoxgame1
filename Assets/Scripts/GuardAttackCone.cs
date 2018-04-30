using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAttackCone : MonoBehaviour {
	public GuardMovement1 guardScript;

	public bool isLeft = false;

	void Start() {
		guardScript = gameObject.GetComponentInParent<GuardMovement1> ();
		Debug.Log ("Got the main Script!");
	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			Debug.Log ("Player in Cone!");
			if (isLeft) {
				guardScript.attack (false);
			} else {
				Debug.Log ("Attacking");
				guardScript.attack (true);
			}
		}
	}
}
