using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	//=====MOVEMENT=====
	float hspd;
	float vspd;
	float h;
	float v;
    public float movspd = 2F;
	//Movement animation
	private Animator animator;

	public GameController gameController;



    // Use this for initialization
    void Start()
    {
		animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		#region MOVEMENT
		//=====MOVEMENT=====
		//Horizontal Inputs
		// if [LeftArrow or A] or [RightArrow or D]
		if ((Input.GetKey (KeyCode.LeftArrow) || (Input.GetKey (KeyCode.A))) || ((Input.GetKey (KeyCode.RightArrow)) ||(Input.GetKey (KeyCode.D)))) {
			//Vertical Inputs
			// if [UpArrow or W] or [DownArrow or S]
			if ((Input.GetKey (KeyCode.UpArrow) || (Input.GetKey (KeyCode.W))) || ((Input.GetKey (KeyCode.DownArrow)) ||(Input.GetKey (KeyCode.S)))) {
				//Calculate Component Vectors of hypotenuse (1)
				//Makes diagonal speed same as horizontal/vertical
				h = Mathf.Cos (Mathf.Deg2Rad*45)*(Input.GetAxisRaw ("Horizontal"));
				v = Mathf.Sin (Mathf.Deg2Rad*45)*(Input.GetAxisRaw("Vertical"));
			//Only Left and Right
			} else {
				h = Input.GetAxisRaw("Horizontal");
				v = Input.GetAxisRaw("Vertical");
			}
		//Using Game Pad (XBox)
		} else {
			h = Input.GetAxisRaw("Horizontal");
			v = Input.GetAxisRaw("Vertical");
		}
		//Calculate Speed
		hspd = h * movspd;
		vspd = v * movspd;
		//Movement Programming
        Vector3 pos = transform.position;
        pos.x += hspd * Time.deltaTime;
        pos.z += vspd * Time.deltaTime;
        transform.position = pos;
		//END OF MOVEMENT
		#endregion
		ManageMovement (h, v);


    }
	/// <summary>
	/// Lets animator know if player is moving or not.
	/// </summary>
	/// <param name="horizontal">Horizontal.</param>
	/// <param name="vertical">Vertical.</param>
	void ManageMovement(float horizontal, float vertical)
	{
		if (horizontal != 0f || vertical != 0f) {
			animator.SetBool ("isMoving", true);
			animateWalk (horizontal, vertical);
		} else {
			animator.SetBool ("isMoving", false);
		}
	}
	/// <summary>
	/// Lets the animator know what direction the player is facing.
	/// </summary>
	/// <param name="h">The height.</param>
	/// <param name="v">V.</param>
	void animateWalk(float h,float v) 
	{
		if (animator) {
			if ((h > 0) && (v < h)) { 
				animator.SetInteger ("moveDir", 1); 
			}
			if ((v > 0) && (v > h)) { 
				animator.SetInteger ("moveDir", 2); 
			}
			if ((h < 0) && (v > h)) { 
				animator.SetInteger ("moveDir", 3); 
			} 
			if ((v < 0) && (v < h)) { 
				animator.SetInteger ("moveDir", 4); 
			}
		}
	}
}
