using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {


    public Stairs topStairs;
    public Stairs bottomStairs;

    private float groundY;

    // Use this for initialization
	void Start ()
    {
        groundY = FindGround();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void PlaceSprite(GameObject to_move)
    {
        SpriteRenderer object_sprite = to_move.GetComponent<SpriteRenderer>();
        float sprite_bottom = -object_sprite.bounds.extents.y; //Gets y of bottom part of sprite

        float sprite_center = to_move.transform.position.y;
        float sprite_bottom_to_center = Mathf.Abs(sprite_bottom - sprite_center);   //Find the distance b/w center and bottom too se how much space needs to be above the ground.

        to_move.transform.position = new Vector3(transform.position.x, (groundY + sprite_bottom_to_center));
    }

    public void SendOnStairs(GameObject to_move, char direction)
    {
        if(direction == 'u')
        {
            topStairs.PlaceSprite(to_move);
        }
        else if (direction == 'd')
        {
            bottomStairs.PlaceSprite(to_move);
            Debug.Log("Sending Down");
        }


    }

    float FindGround() //Do a raycast to find the ground 
    {
        float ground_y = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.NameToLayer("Ground"));
        if (hit.collider != null) //It shouldn't be null but who knows, should be a safe bet
        {
            ground_y = hit.point.y;
        }
       return ground_y;
    }

}
