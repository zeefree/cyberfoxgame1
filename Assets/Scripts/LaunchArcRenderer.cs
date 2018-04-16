using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaunchArcRenderer : MonoBehaviour {

    LineRenderer lr;

    public float velocity;
    public float angle;
    public int segments = 15;

    float radAngle;
    float jumpFactor;
    float g;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
        jumpFactor = GetComponentInParent<SimplePlayerMovement>().jumpFactor;
    }

    void OnValidate()
    {
        if (lr != null && Application.isPlaying)
            RenderArc();
    }

	// Use this for initialization
	void Start () {
        RenderArc();	
	}

    // Setting up LineRenderer
    void RenderArc()
    {
        lr.positionCount = segments + 1;
        lr.SetPosition(0, transform.position);
        lr.SetPositions(CalculateArcArray());
    }

    // create an array of positions for the arc
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[segments + 1];

        radAngle = Mathf.Deg2Rad * angle;
        float maxDist = (velocity * velocity * Mathf.Sin(2 * radAngle)) / g;
        Vector2 pos = transform.position;
        Vector2 ang = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
        Vector2 vel =  ang * velocity;

        for (int i = 0; i <= segments; i++)
        {
            /*
            float t = (float)i / (float)segments;
            velocity += g * Time.fixedDeltaTime;
            arcArray[i] = transform.position + CalculateArcPoint(t, maxDist);
            */
            arcArray[i] = pos;
            vel += Physics2D.gravity * GetComponentInParent<Rigidbody2D>().gravityScale * Time.fixedDeltaTime;
            pos += vel * Time.fixedDeltaTime;
        }

        return arcArray; 
    }
	
    // calculate each vertex in array
    Vector3 CalculateArcPoint(float t, float maxDist)
    {
        float tan = Mathf.Tan(radAngle);
        float cos = Mathf.Cos(radAngle);
        float x = t * maxDist;
        float y = x * tan - ((g * x * x) / (2 * velocity * velocity * cos * cos));
        return new Vector3(x, y);
    }

    public void MakeArc(Vector2[] touches)
    {
        float thrust =  Vector2.Distance(touches[0], touches[1]) * jumpFactor;
        Vector2 force = (touches[0] - touches[1]).normalized * thrust;
        velocity = (( thrust / GetComponentInParent<Rigidbody2D>().mass) - 
        GetComponentInParent<Rigidbody2D>().gravityScale * g) * Time.fixedDeltaTime;
        //velocity = v.magnitude;
        //velocity = thrust;
        Vector2 diff = touches[0] - touches[1];
        float sign = (touches[0].y < touches[1].y) ? -1.0f : 1.0f;
        angle =  Vector2.Angle(Vector2.right, diff) * sign;
        RenderArc();
    }
}
