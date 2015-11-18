using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
	private GameObject gameController;
	public float speed = 10f;
	public float lifetime;
	private GameObject aimController;
	//private GameController gameController;
	private GameObject player;
	public float weaponType;
	//AimControllerScript
	float dir;
	float xPos;
	float yPos;
	public float damage;
	public float critChance;
	public float manaValue;

	float expValue = 1; // Weapon Experience
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
		//Get weapon type
		//weaponType = aimController.GetComponent<AimController>().gameController.currentWeapon;
		damage = aimController.GetComponent<AimController>().gameController.damage;
		critChance = aimController.GetComponent<AimController>().gameController.calcCritChance;
		AddWeaponExperience (expValue);

		if (weaponType == 4) {
			aimController.GetComponent<AimController>().gameController.SpendMana(manaValue);
		}

	}

	// Update is called once per frame
	void LateUpdate () {
		//Staff and Bow
		if (weaponType != 2F) {
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
	//Collide with enemy
	void OnTriggerEnter(Collider other)
	{
		other.gameObject.GetComponent<EnemyController> ().Hit (damage,CritChance ());
		//Destroy (other.gameObject);
		if (weaponType != 2) {
			Destroy (gameObject);
		}
		//Add EXP
		//gameController2.AddExp(expValue);
		
	}

	/// <summary>
	/// Calculate if hit was critical
	/// </summary>
	/// <returns><c>true</c>, if chance was crited, <c>false</c> otherwise.</returns>
	bool CritChance()
	{
		int temp = Random.Range (0, 100);
		if (temp < critChance) {
			return true;
		}
		return false;
	}
	void AddWeaponExperience(float expValue)
	{
		gameController.GetComponent<GameController> ().AddWeaponExp (expValue);
	}

}
