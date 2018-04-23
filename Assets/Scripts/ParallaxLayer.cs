using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    public void Move(float delta)
    {
        Debug.Log("Trying to move");
        Vector3 newPos = transform.localPosition;
        newPos.x += delta * parallaxFactor;
        transform.localPosition = newPos;
    }
}