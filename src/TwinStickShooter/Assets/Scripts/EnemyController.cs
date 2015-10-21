using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float enemyHealth = 10F;
	public float enemyMaxHealth = 10F;
	public float enemyLevel = 1F;
	public float enemyHealthMod = 2F;
	public float expValue;


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
	
	}
	
	// Update is called once per frame
	void Update () {
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
	void OnTriggerEnter(Collider other)
	{
		Destroy (other.gameObject);
		Destroy (gameObject);
		//Add EXP
		gameController2.AddExp(expValue);

	}
}
