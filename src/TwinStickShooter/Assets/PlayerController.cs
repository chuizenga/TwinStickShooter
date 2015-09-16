using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float movspd = 0;
	bool moving = false;
	float horspd = 0;
	float vertspd = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float hdir = Input.GetAxisRaw ("Horizontal");
		float vdir = Input.GetAxisRaw ("Vertical");
		Vector3 hpos = transform.position;
		Vector3 vpos = transform.position;
		if (hdir != 0 && vdir != 0) {
			moving = true
		}
		else 
		{
			moving = false;
		}
		if (moving == true)
		{
			if (hdir > 0)
			{
				transform.position = pos2;
			}
		}
		
	}
}
