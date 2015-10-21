using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public GameObject target;
	
	void LateUpdate () {
		transform.position = target.transform.position;
	}
}