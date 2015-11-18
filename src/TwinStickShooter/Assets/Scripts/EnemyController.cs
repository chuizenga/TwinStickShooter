using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public float enemyHealth = 100F;
	public float enemyMaxHealth = 100F;
	public float enemyLevel = 1F;
	public float enemyHealthMod = 2F;
	public float expValue;

	private Image healthBar;
	public GameObject CBTprefab;

	//GameController Variables
	float chunkPlayerX;
	float chunkPlayerY;
	float storeChunkX;
	float storeChunkY;
	//Position Variables
	float posX;
	float posY;
	private GameObject gameController;
	private GameController gameController2;


	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController2 = gameControllerObject.GetComponent<GameController> ();
		gameController = GameObject.Find("GameController");
		healthBar = transform.FindChild ("EnemyCanvas").FindChild ("HealthBG").FindChild ("Health").GetComponent<Image> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		if (enemyHealth <= 0) {
			gameController2.AddExp(expValue);
			Destroy (gameObject);
		}



		//Get Variables From GameController
		chunkPlayerX = gameController.GetComponent<GameController>().chunkPlayerX;
		chunkPlayerY = gameController.GetComponent<GameController>().chunkPlayerY;
		storeChunkX = gameController.GetComponent<GameController>().storeChunkX;
		storeChunkY = gameController.GetComponent<GameController>().storeChunkY;
		//Store Position Variables
		posX = transform.position.x;
		posY = transform.position.z;
		//CHECKS TO SEE IF OFFSCREEN
		//1 RIGHT
		if (posX < (chunkPlayerX * 5 - 20)) {
			Destroy (gameObject);
		}
		//2 UP
		if (posY < (chunkPlayerY * 5 - 20)) {
			Destroy (gameObject);
		}
		
		//3 LEFT
		if (posX > (chunkPlayerX * 5 + 20)) {
			Destroy (gameObject);
		}
		//4 DOWN
		if (posY > (chunkPlayerY * 5 + 20)) {
			Destroy (gameObject);
		}

	
	}
	public void Hit(float damage, bool isCrit)
	{
		if (isCrit)
			damage *= 2;
		enemyHealth -= damage;
		healthBar.fillAmount = enemyHealth / enemyMaxHealth;
		if (!isCrit) {
			InitCBT (damage.ToString ()).GetComponent<Animator> ().SetTrigger ("Hit");
		} else {
			InitCBT (damage.ToString ()).GetComponent<Animator> ().SetTrigger ("Crit");
		}
	}
	/// <summary>
	/// Floating Combat Text
	/// </summary>
	/// <returns>The CB.</returns>
	/// <param name="text">Text.</param>
	GameObject InitCBT(string text)
	{
		GameObject temp = Instantiate (CBTprefab) as GameObject;
		RectTransform tempRect = temp.GetComponent<RectTransform> ();
		tempRect.transform.SetParent (transform.FindChild ("EnemyCanvas"));
		tempRect.transform.localPosition = CBTprefab.transform.localPosition;
		tempRect.transform.localScale = CBTprefab.transform.localScale;
		tempRect.transform.localRotation = CBTprefab.transform.localRotation;

		temp.GetComponent<Text> ().text = text;
		Destroy (temp.gameObject, 2);

		return temp;
	}

}
