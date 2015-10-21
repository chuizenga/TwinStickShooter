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

	public GameController gameController;



    // Use this for initialization
    void Start()
    {

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
				h = Mathf.Cos (Mathf.Deg2Rad*45)*(Input.GetAxis ("Horizontal"));
				v = Mathf.Sin (Mathf.Deg2Rad*45)*(Input.GetAxis("Vertical"));
			//Only Left and Right
			} else {
				h = Input.GetAxis("Horizontal");
				v = Input.GetAxis("Vertical");
			}
		//Using Game Pad (XBox)
		} else {
			h = Input.GetAxis("Horizontal");
			v = Input.GetAxis("Vertical");
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


    }

}
