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
        if(x != 0)
        {
            r2d.velocity = (transform.right * x * speed);
        }
       
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        
        if (coll.gameObject.tag == "Stairs")
        {
            
            Stairs well = coll.gameObject.GetComponent<Stairs>();
            float up_input = Input.GetAxis("Vertical");

            Debug.Log("Vert input value " + up_input);
            if(up_input > 0)
            {
                well.SendOnStairs(this.gameObject, 'u');
            }
            else if(up_input < 0)
            {
                well.SendOnStairs(this.gameObject, 'd');
            }
        }
    }
}
