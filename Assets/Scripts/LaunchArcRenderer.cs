using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaunchArcRenderer : MonoBehaviour {

    LineRenderer lr;
    public float velocity;
    public float angle;
    public int segments = 30;
    float radAngle;
    float jumpFactor;
    float maxJump;
    float g;
	Rigidbody2D rb2d;

    // Sets up references and gets some values from the physics and player
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
        jumpFactor = GetComponentInParent<SimplePlayerMovement>().jumpFactor;
        maxJump = GetComponentInParent<SimplePlayerMovement>().maxJump;
		rb2d = GetComponentInParent<Rigidbody2D>();
    }

    // This makes it change the arc whenever you change settings in inspector
    // usefull for debugging but won't do anything with the hide arc method 
    // in TouchHandling.cs. To use this uncomment it AND comment out contents of
    // clearArc (but not the method itself).
    /*void OnValidate()
    {
        if (lr != null && Application.isPlaying)
            RenderArc();
    }*/

	// Use this for initialization
	void Start () {
        //RenderArc();	
	}

    // Renders the arc by setting the position count and init position and then uses
    // CalculateArcArray to get the rest of the points
    void RenderArc()
    {
        lr.positionCount = segments + 1;
        lr.SetPosition(0, transform.position);
        lr.SetPositions(CalculateArcArray());
    }

    // Calculate the positions of the arc and returns an array of points to be used to render
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[segments + 1];

        radAngle = Mathf.Deg2Rad * angle;
        //float maxDist = (velocity * velocity * Mathf.Sin(2 * radAngle)) / g;
        Vector2 pos = transform.position;
        Vector2 ang = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
        Vector2 vel =  ang * velocity;

        // This is just mimicing the player physics to get the points. May be slightly off
        // depending on the physics in Unity if they use drag or whatever.
        for (int i = 0; i <= segments; i++)
        {
            arcArray[i] = pos;
            vel += Physics2D.gravity * SimplePlayerMovement.GRAVITY_SCALE * Time.fixedDeltaTime;
            pos += vel * Time.fixedDeltaTime;
        }

        return arcArray; 
    }

    // A method to make the arc based on touches collected from the Jump State of the
    // TouchHandler.
    public void MakeArc(Vector2[] touches)
    {
        // This gets the initial velocity and set's the velocity
        float thrust =  Vector2.Distance(touches[0], touches[1]) * jumpFactor;
        thrust = Mathf.Clamp(thrust, 0.0f, maxJump);
    
        velocity = (( thrust / rb2d.mass) - 
            rb2d.gravityScale * g) * Time.fixedDeltaTime;
        
        //basically getting the corrected angle
        Vector2 diff = touches[0] - touches[1];
        float sign = (touches[0].y < touches[1].y) ? -1.0f : 1.0f;
        angle =  Vector2.Angle(Vector2.right, diff) * sign;

        //Rendering arc
        RenderArc();
    }

    // Hides the arc by zeroing the number of points
    public void clearArc()
    {
        lr.positionCount = 0;
    }
}
