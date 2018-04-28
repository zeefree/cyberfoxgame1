using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    public float delta;
    private float oldPosition;

    private Vector2 velocity;

    public float smoothTimeY = 0.05f;
    public float smoothTimeX = 0.05f;
    public float offset = 0.05f;

    public GameObject player;

    void Start()
    {
        oldPosition = transform.position.x;
    }
    void Update()
    {
        

        if (transform.position.x != oldPosition)
        {
             delta = oldPosition - transform.position.x;

             Debug.Log("delta is now " + delta);
               
            
             oldPosition = transform.position.x;
        }
        else
        {
            delta = 0;
        }
    }

    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }

    
}