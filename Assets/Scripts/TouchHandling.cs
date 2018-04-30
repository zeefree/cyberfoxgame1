// Note: code styleing/formatting messed up. I used the normal
// MonoDevelop editor Unity is packaged with and the autocompletion
// really preferred this style. If it is an issue can change later
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// A really messy class for handling touch controls/multitouch
// as well as having it play nice with the UI elements on the canvas
// a real pain in the ass, probably needs refactoring later.
public class TouchHandling : MonoBehaviour
{

    // Use a simple state machine to handle touch inputs, not the
    // Move state is actually probably what we'd use for any other UI
    // elements we wish to add in the future (may need to rename). Basic flow:
    // Starts w/ None - no touch inputs - then if the user presses in the 'Jump
    // region' around the player object once it will go to the Jump State. If 
    // Touched anywhere else it goes to the Move State. During the Move State
    // if a second touch is recognized close enough to the previous touch
    // it goes to the yet to be implemented Camera State for pinching and panning.
    // The second touch distance can be tweaked by cameraActivateDist
    public enum TouchState
    {
        None,
        Jump,
        Move,
        Camera,
    };

    public string[] pStates = new string[]
    {
        "Ground",
        "InAir",
        "WallL",
        "WallR",
        "Ceiling"
    };

    public float zoomSpeed = 0.1f;
    public float zoomInCap = 5f;
    public float zoomOutCap = 40f;
    public Camera camera;
    private int touchInputMask;
    public TouchState state;
    private Vector2[] touchPoints;
    public GameObject player;
    public GameObject stateText;
    public GameObject hitText;
    public GameObject playerText;
    private LaunchArcRenderer lar;
    private float haxis;
    public float cameraActivateDist;

    private float dragDistance = 100f; //Distance required to drag for it to count as a drag

    public float jumpActivateDist;


    //Chase's new lines begin
    private Rigidbody2D rb2d;
    private Animator anim;
    public bool grounded;
    //Chase's new lines end

    // The None State function. Just looks for 'Began' phase touches and sets the
    // next state according to what was touched
    void NoneState()
    {
        stateText.GetComponent<Text>().text = "None";

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
		{
			Vector2 touch = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector3 v3 = Camera.main.ScreenToWorldPoint (touch);
			RaycastHit2D hit = Physics2D.Raycast (new Vector2 (v3.x, v3.y), Vector2.zero,
				10f, touchInputMask);
			
			if (hit && hit.collider.tag == "JumpRegion")
			{
				hitText.GetComponent<Text> ().text = hit.collider.tag;
				state = TouchState.Jump;
				touchPoints[0] = touch;
			}
			else 
			{
				state = TouchState.Move;
				touchPoints [0] = touch;
				hitText.GetComponent<Text> ().text = "No hit";
			}
		}
#endif

#if UNITY_ANDROID
        // Basically the same for all of these, we need to loop through the touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.touches[i];

            //This is just raycasting
            Vector3 v3 = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(v3.x, v3.y), Vector2.zero,
                10f, touchInputMask);


            // If you start touching the player, go to Jump State while setting init point
            if (hit && touch.phase == TouchPhase.Began &&
                hit.collider.tag == "JumpRegion" && Input.touchCount == 1)
            {
                hitText.GetComponent<Text>().text = hit.collider.tag;
                state = TouchState.Jump;
                touchPoints[0] = touch.position;
                break;
            }
            // If not hitting the player, go to Move State
            else if (touch.phase == TouchPhase.Began && Input.touchCount == 1)
            {
                state = TouchState.Move;
                touchPoints[0] = touch.position;
                hitText.GetComponent<Text>().text = "No hit";
                break;
            }
            else
            {
                hitText.GetComponent<Text>().text = "No hit";
            }

        }

        // Just in case, check for no touches
        if (Input.touchCount == 0)
        {
            hitText.GetComponent<Text>().text = "No hit";
            stateText.GetComponent<Text>().text = "None";
        }
#endif
    }


    // Jump State, basically the mouse code from Simple Player movement,
    // adapted for touch.
    void JumpState()
    {
        stateText.GetComponent<Text>().text = "Jump";

#if UNITY_EDITOR
		
		if (Input.GetMouseButtonUp(0))
		{
			Vector2 touch = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			touchPoints [1] = touch;
			state = TouchState.None;

            if (Vector2.Distance(touchPoints[0], touchPoints[1]) > jumpActivateDist)
			    player.GetComponent<SimplePlayerMovement>().Leap (touchPoints [0], touchPoints [1]);

            hideArc();
		}
        else if (Input.GetMouseButton(0))
        {
            Vector2 touch = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            touchPoints[1] = touch;
            if (Vector2.Distance(touchPoints[0], touchPoints[1]) > jumpActivateDist)
                player.GetComponentInChildren<LaunchArcRenderer>().MakeArc(touchPoints);
            else
                hideArc();
        }

#endif

#if UNITY_ANDROID

		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.touches [i];

			// If you make a second touch stop the Jump
			if (touch.phase == TouchPhase.Began) {
				state = TouchState.None;
                hideArc();
                
				break;

			// Else if you end the Touch get the endpoint and make the leap
			} else if (touch.phase == TouchPhase.Ended) {
				touchPoints [1] = touch.position;
				state = TouchState.None;
                if (Vector2.Distance(touchPoints[0], touchPoints[1]) > jumpActivateDist)
				    player.GetComponent<SimplePlayerMovement>().Leap (touchPoints [0], touchPoints [1]);
                
                hideArc();
				break;

			// This is in case something goes wrong
			} else if (touch.phase == TouchPhase.Canceled) {
				state = TouchState.None;
                hideArc();
				break;
			}
            else
            {
                touchPoints[1] = touch.position;
                
                if (Vector2.Distance(touchPoints[0], touchPoints[1]) > jumpActivateDist)
                    player.GetComponentInChildren<LaunchArcRenderer>().MakeArc(touchPoints);
                else
                    hideArc();
            }
		}

		if (Input.touchCount == 0) {
			hitText.GetComponent<Text> ().text = "No hit";
			stateText.GetComponent<Text> ().text = "None";
            hideArc();
			state = TouchState.None;
		}

#endif
    }

    // Move state, seems to be working now. Most complex so far. Basically if you
    // start a second touch it will check and see if it the two are close enough to
    // start the camera state, if not it just lets the user interact with the UI
    // controlls. A lot of messing around in the Player's movement script, defenitely
    // should be reworked at some point.
    void MoveState()
    {
        stateText.GetComponent<Text>().text = "Move";

#if UNITY_EDITOR

		if (Input.GetMouseButton(0))
		{
			//Vector2 touch = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			var s = player.GetComponent<SimplePlayerMovement>();

			//Chase's new lines begin
			anim = player.GetComponent<Animator>();
			grounded = true;
			anim.SetBool ("Grounded", grounded);
			anim.SetFloat ("Speed", Mathf.Abs(s.xAxis * s.speed));
			if ((s.xAxis * s.speed)  < -0.1f) {
                //player.transform.localScale =  new Vector3 (-2, 2, 1);
		       player.GetComponent<SpriteRenderer>().flipX = true;
			}

			if ((s.xAxis * s.speed) > 0.1f) {
               //player.transform.localScale =  new Vector3 (2, 2, 1);
                player.GetComponent<SpriteRenderer>().flipX = false;
			}
			//Chase's new lines ens
		}
		else
		{
			state = TouchState.None;
		}

#endif


#if UNITY_ANDROID

		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.touches [i];

			// Aforementioned check for camera state
			if (touch.phase == TouchPhase.Began && Input.touchCount == 2) {
				
				float diffx = 
					Math.Abs (Input.touches [0].position.x - Input.touches [1].position.x);
				float diffy = 
					Math.Abs (Input.touches [0].position.y - Input.touches [1].position.y);

				if (diffx < cameraActivateDist && diffy < cameraActivateDist) {
					state = TouchState.Camera;
					player.GetComponent<SimplePlayerMovement> ().xAxis = 0f;
					touchPoints [0] = Input.touches [0].position;
					touchPoints [1] = Input.touches [1].position;
					break;
				} else {
					player.GetComponent<SimplePlayerMovement> ().xAxis = 0f;
				}
			
			// Check if you are ending the last remaining touch.
			} else if (touch.phase == TouchPhase.Ended && Input.touchCount == 1) {
				state = TouchState.None;
                touchPoints[1] = touch.position;

                float point_dif = (touchPoints[0].y - touchPoints[1].y); //take the differnce between the start point and our end point

                if (Mathf.Abs(point_dif) >= dragDistance)
                {

                    Player the_player = player.GetComponent<Player>();
                    //Now it should be a swipe
                    if (point_dif < 0)
                    {
                        the_player.stair_direction = 'u';
                    }
                    else if (point_dif > 0)
                    {
                        the_player.stair_direction = 'd';
                    }
                   
                    //The player will reset the stair_direction itself
                }

                break;

			// This bit moves the player on the horizontal axis while you are touching
			// the ui buttons that set the axis value.
			} else if ((touch.phase == TouchPhase.Moved ||
			           touch.phase == TouchPhase.Stationary) && Input.touchCount == 1) {

				var s = player.GetComponent<SimplePlayerMovement>();

                player.GetComponent<Player>().stair_direction = 'n'; //Make sure the player doesn't think it needs to go in any sort of direction while moving

                //Chase's new lines begin
                anim = player.GetComponent<Animator>();
				grounded = true;
				anim.SetBool ("Grounded", grounded);
				anim.SetFloat ("Speed", Mathf.Abs(s.xAxis * s.speed));
				if ((s.xAxis * s.speed)  < -0.1f) {
                    //player.transform.localScale =  new Vector3 (-2, 2, 1);
                    player.GetComponent<SpriteRenderer>().flipX = true;
				}

				if ((s.xAxis * s.speed) > 0.1f) {
                    //player.transform.localScale =  new Vector3 (2, 2, 1);
                    player.GetComponent<SpriteRenderer>().flipX = false;
				}
                //Chase's new lines end
                //Need to store the point that we touched so I can make a comparison when we let go
                

            }
        }

		if (Input.touchCount == 0) {
			hitText.GetComponent<Text> ().text = "No hit";
			stateText.GetComponent<Text> ().text = "None";
			player.GetComponent<SimplePlayerMovement> ().xAxis = 0f;
			state = TouchState.None;


            


            
		}

#endif

    }

    // Camera State. Deceptively annoying. This is probably where Pinch Zoom and pan
    // would go. Have an idea that you would get the two touchPoints and compare the
    // positions for the controlls. IE the distance between the positions would work
    // the zoom, while the center between the points might be able to pan - dragging
    // touches without changing the distance will pan until letting go on which the
    // camera would return to the player
    void CameraState()
    {
        stateText.GetComponent<Text>().text = "Camera";



        if (Input.touchCount == 2)
        {
            //Store the two touches
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            //Get prvious positions
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            //Get magintudes of those differneces
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            camera.orthographicSize += deltaMagnitudeDiff * zoomSpeed;


            camera.orthographicSize = Mathf.Max(camera.orthographicSize, zoomInCap);
          
            if(camera.orthographicSize >= zoomOutCap)
            {
                camera.orthographicSize = Mathf.Min(camera.orthographicSize, zoomOutCap);
            }
        }

        for (int i = 0; i < Input.touchCount && Input.touchCount == 2; i++)
            {
                Touch touch = Input.touches[i];
                touchPoints[i] = touch.position;
            }


        
     


        // Go back to None State when you don't have exactly two fingers touching
        if (Input.touchCount != 2)
        {
            hitText.GetComponent<Text>().text = "No hit";
            stateText.GetComponent<Text>().text = "None";
            state = TouchState.None;
        }
    }

    // This will be changed later. Probably
    void Start()
    {
        state = TouchState.None;
        touchInputMask = 1 << LayerMask.NameToLayer("UI");
        touchPoints = new Vector2[] { Vector2.zero, Vector2.zero };
        cameraActivateDist = (float)Screen.width / 2.5f;
        lar = player.GetComponentInChildren<LaunchArcRenderer>();
        dragDistance = 200f;
    }

    // All the Update function has is a simple switch for the states
    void Update()
    {
        playerText.GetComponent<Text>().text = pStates[(int)player.GetComponent<SimplePlayerMovement>().state];

        //test

        //player.GetComponent<SimplePlayerMovement>().Move();

        //var s = player.GetComponent<SimplePlayerMovement>();

        //anim = player.GetComponent<Animator>();
        //anim.SetFloat ("Speed", Mathf.Abs(s.xAxis * s.speed));

    


            switch (state)
        {
            case TouchState.None:
                NoneState();
                break;

            case TouchState.Jump:
                JumpState();
                break;

            case TouchState.Move:
                MoveState();
                break;

            case TouchState.Camera:
                CameraState();
                break;

            default:
                break;
        }
    }

    public void hideArc()
    {
        lar.clearArc();
    }


}
