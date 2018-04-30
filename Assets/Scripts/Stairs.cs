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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Guard")
        {
            collision.gameObject.SendMessage("stairsHere", this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Guard")
        {
            collision.gameObject.SendMessage("stairsHere", this.gameObject);
        }
    }

    void PlaceSprite(GameObject to_move) //Recieving the placement
    {
        SpriteRenderer object_sprite = to_move.GetComponent<SpriteRenderer>();
        float sprite_bottom = object_sprite.bounds.extents.y; //Gets the distance from the bottom part of the sprite to center
        

        to_move.transform.position = new Vector3(transform.position.x, (groundY + sprite_bottom));
    }

    public void SendOnStairs(GameObject to_move, char direction)
    {
        if(direction == 'u')
        {
            if(topStairs != null)
            {
                topStairs.PlaceSprite(to_move);
            }
            
        }
        else if (direction == 'd')
        {
            if(bottomStairs != null)
            {
                bottomStairs.PlaceSprite(to_move);
                
            }
           
        }


    }

    float FindGround() //Do a raycast to find the ground 
    {
        float ground_y = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 10f, LayerMask.GetMask("Ground"));
        if (hit.collider != null) //It shouldn't be null but who knows, should be a safe bet
        {
            ground_y = hit.point.y;
        }
        else
        {
            ground_y = transform.position.y;
        }
       return ground_y;
    }

}
