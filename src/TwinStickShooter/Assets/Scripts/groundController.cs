using UnityEngine;
using System.Collections;

public class groundController : MonoBehaviour {
	private GameObject gameController;


	//GameController Variables
	float chunkPlayerX;
	float chunkPlayerY;
	float storeChunkX;
	float storeChunkY;
	//Position Variables
	float posX;
	float posY;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController");
	
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
}
