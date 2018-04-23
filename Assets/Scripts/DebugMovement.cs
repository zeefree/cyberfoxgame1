using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMovement : MonoBehaviour {

	public int speed = 5;
    private Rigidbody2D r2d;
    private float _up_input;

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
        float up_input = Input.GetAxis("Vertical");

        _up_input = up_input;
        
    }

    void stairsHere(GameObject coll_stairs)
    {
        Stairs stair = coll_stairs.GetComponent<Stairs>();

        if (_up_input > 0)
         {
              stair.SendOnStairs(this.gameObject, 'u');
         }
         else if (_up_input < 0)
         {
             stair.SendOnStairs(this.gameObject, 'd');
             Debug.Log("Send down");
          }
        

    }
  
}
