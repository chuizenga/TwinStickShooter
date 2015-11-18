using UnityEngine;
using System.Collections;

public class AimController : MonoBehaviour {

	float rotation = 0;
	float currentRotationVelocity;

	public Plane groundPlane = new Plane(Vector2.up, Vector3.zero);
	public GameObject mouseSprite;
	public GameController gameController;


	public GameObject prefabMagicMissile;
	public GameObject prefabSlash;
	public GameObject prefabArrow;
	public GameObject prefabFireBall;
	public GameObject prefabEnergyBlade;
	public GameObject prefabPiercingShot;

	public float bulletDelay = .25f;
	float timeUntilNextBullet=0;
	public float angle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//=====AIM=====
		MoveMouseSprite ();
		//CALCULATE ANGLE
		float deltaX = mouseSprite.transform.position.x - transform.position.x;
		float deltaY = mouseSprite.transform.position.z - transform.position.z;
		angle = Mathf.Atan2 (deltaX, deltaY) * Mathf.Rad2Deg;
		//Correct Weird turning
		while(angle < rotation - 180) rotation -= 360;
		while(angle > rotation + 180) rotation += 360;
		//Debug.Log (angle);
		//Slow Rotation
		rotation = Mathf.SmoothDamp(rotation, angle, ref currentRotationVelocity, 0);
		//Rotates to point
		transform.rotation = Quaternion.Euler (90, rotation, 0);


		//Shoot
		if (Input.GetAxis ("Fire1") > 0) {
			if (timeUntilNextBullet <= 0) {
				RayCastShot (); //Shoots a Ray
				LAttack ();
				timeUntilNextBullet = bulletDelay;
			} else {
				timeUntilNextBullet -= Time.deltaTime;
			}
		} else if (Input.GetAxis ("Fire2") > 0) {
			if (timeUntilNextBullet <= 0) {
				RayCastShot (); //Shoots a Ray
				RAttack ();
				timeUntilNextBullet = bulletDelay;
			} else {
				timeUntilNextBullet -= Time.deltaTime;
			}
		} else {
			timeUntilNextBullet = 0;
		}

	}

	void MoveMouseSprite ()
	{
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float rayDistance;
		if (groundPlane.Raycast (ray, out rayDistance)) {
			//Debug.Log("Plane Raycast hit at distance: " + rayDistance);
			var hitPoint = ray.GetPoint (rayDistance);
			mouseSprite.transform.position = hitPoint;
			Debug.DrawRay (ray.origin, ray.direction * rayDistance, Color.green);
		}
		else {
			Debug.DrawRay (ray.origin, ray.direction * rayDistance, Color.red);
		}
	}

	void RayCastShot ()
	{
		Ray rayShot = new Ray (transform.position, transform.up);
		Debug.DrawRay (rayShot.origin, rayShot.direction * 5, Color.black, 1F);
		RaycastHit hit;
		//From Tutorial
		/*
		if (Physics.Raycast (rayShot, out hit, 5)) {
			if (hit.collider.gameObject.tag == "Enemy") {
				Destroy (hit.collider.gameObject);
			}
		}*/
	}

	void LAttack ()
	{
		//1 Staff
		if (gameController.currentWeapon == 1) {
			Instantiate (prefabMagicMissile, transform.position, Quaternion.identity);
		}
		//2 Sword
		if (gameController.currentWeapon == 2) {
			Instantiate (prefabSlash, transform.position, Quaternion.identity);
		}
		//3 Bow
		if (gameController.currentWeapon == 3) {
			Instantiate (prefabArrow, transform.position, Quaternion.identity);
		}
	}
	void RAttack ()
	{
		//1 Staff
		if ((gameController.currentWeapon == 1)&& (gameController.currentWepLv >=5) && (gameController.mana > 10)) {
			Instantiate (prefabFireBall, transform.position, Quaternion.identity);
		}
		//2 Sword
		if ((gameController.currentWeapon == 2)&& (gameController.currentWepLv >=5)) {
			Instantiate (prefabEnergyBlade, transform.position, Quaternion.identity);
		}
		//3 Bow
		if ((gameController.currentWeapon == 3)&& (gameController.currentWepLv >=5)) {
			Instantiate (prefabPiercingShot, transform.position, Quaternion.identity);
		}
	}
}
