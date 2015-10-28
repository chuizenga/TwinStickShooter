using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
	private GameObject gameController;
	public float speed = 10f;
	public float lifetime;
	private GameObject aimController;
	//private GameController gameController;
	private GameObject player;
	float weaponType;
	//AimControllerScript
	float dir;
	float xPos;
	float yPos;
	// Use this for initialization
	void Start()
	{

		gameController = GameObject.Find("GameController");
		aimController  = GameObject.Find("aimingArrow");
		player = GameObject.Find ("Player");
		float dir = aimController.GetComponent<AimController>().angle;
		Destroy(gameObject, lifetime);
		transform.position += transform.up * .3F;
		transform.rotation = Quaternion.Euler (90, dir, 0);
		weaponType = aimController.GetComponent<AimController>().gameController.currentWeapon;
	}

	// Update is called once per frame
	void LateUpdate () {
		//Staff and Bow
		if (weaponType == 1F || weaponType == 3F) {
			transform.position += transform.up * speed * Time.deltaTime;
		}
		//Sword
		if (weaponType == 2F) {
			Vector3 pos = transform.position;
			pos.x = player.transform.position.x;
			pos.z = player.transform.position.z;
			transform.position = pos;
		}
			
	}
	void OnTriggerEnter(Collider other)
	{
		other.gameObject.GetComponent<EnemyController> ().Hit (10,CritChance ());
		//Destroy (other.gameObject);
		Destroy (gameObject);
		//Add EXP
		//gameController2.AddExp(expValue);
		
	}
	bool CritChance()
	{
		int temp = Random.Range (0, 100);
		if (temp < 50) {
			return true;
		}
		return false;
	}

}
