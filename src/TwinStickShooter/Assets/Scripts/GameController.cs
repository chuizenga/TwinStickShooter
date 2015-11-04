using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	//Weapons
	public float currentWeapon = 1;
	public float numWeapons = 3;

	//Experience
	public float level = 1;
	public float exp = 0;
	float maxExp;

	//Prefabs
	public GameObject groundPrefab;
	public GameObject player;
	public GameObject enemyPrefab;

	//Visual Debug Text
	public GUIText weaponText;
	public GUIText playerCoords;
	public GUIText experienceText;

	//====WORLD STUFF====
	//Initial World Array
	float[,] world = new float[9,9];
	//When moving to new areas
	float[] worldAdd = new float[9];

	public float playerX; //Player X Position
	public float chunkPlayerX; //Player X Position / 5
	public float playerY; //Player Y Position
	public float chunkPlayerY; //Player Y Position / 5
	public float storeChunkX;
	public float storeChunkY;
	// ==END WORLD STUFF==


	//UI STUFF
	private Image xpBar;
	private Image weaponIcon;
	private Text levelText;
	public Sprite weapon1;
	public Sprite weapon2;
	public Sprite weapon3;

	float time;

	// Use this for initialization
	void Start () {
		//Make Initial World
		for (int i = 0; i < world.GetLength(0); i++)
		{
			for (int j = 0; j < world.GetLength(1); j++)
			{
				Instantiate (groundPrefab, new Vector3(i*5-20, 0, j*5-20), Quaternion.Euler (90, 0, 0));
			}
		}		
		storeChunkX = Mathf.Ceil(playerX / 5);
		storeChunkY = Mathf.Ceil(playerY / 5);

		//Initiate UI
		xpBar = transform.FindChild ("UICanvas").FindChild ("xpBG").FindChild ("xp").GetComponent<Image> ();
		weaponIcon = transform.FindChild ("UICanvas").FindChild ("weaponIcon").GetComponent<Image> ();
		levelText = transform.FindChild ("UICanvas").FindChild ("levelText").GetComponent<Text> ();



	
	}
	
	// Update is called once per frame
	void Update () {

		//Switch Weapons
		var d = Input.GetAxis("Mouse ScrollWheel");
		if (d != 0f) {
			MouseScroll (d);
		}
		#region INFINITE WORLD
		//INFINITE WORLD STUFF
		playerX = player.transform.position.x;
		chunkPlayerX = Mathf.Ceil(playerX / 5);
		playerY = player.transform.position.z;
		chunkPlayerY = Mathf.Ceil(playerY / 5);
		playerCoords.text = ("X: " + chunkPlayerX + " Z: " + chunkPlayerY);
		//playerCoords.text = ("X: " + storeChunkX + " Z: " + storeChunkY);

		//1 RIGHT
		if (chunkPlayerX > storeChunkX) {
			TimeToMove(1);
			storeChunkX = chunkPlayerX;
			//Debug.Log (chunkPlayerX + " " + chunkPlayerY + " " + storeChunkX + " " + storeChunkY);
		}
		//2 UP
		if (chunkPlayerY > storeChunkY) {
			TimeToMove(2);
			storeChunkY = chunkPlayerY;
		}
		//3 LEFT
		if (chunkPlayerX < storeChunkX) {
			TimeToMove(3);
			storeChunkX = chunkPlayerX;
		}
		//4 DOWN
		if (chunkPlayerY < storeChunkY) {
			TimeToMove(4);
			storeChunkY = chunkPlayerY;
		}
		#endregion

		//EXPERIENCE AND LEVELING
		maxExp = 10 + 10*(level-1);
		if (exp >= maxExp) {
			exp-=maxExp;
			level++;
		}
		//Display
		//experienceText.text = ("Lv: " + level + " XP: " + exp+"/"+maxExp);
		xpBar.fillAmount = exp / maxExp;
		levelText.text = "Lv. " + level;



	}

	//Loading new terrain
	void TimeToMove(int moveDir)
	{
		if (moveDir == 1) {
			for (int i = 0; i < world.GetLength(0); i++) {
				Vector3 placePoint = new Vector3((chunkPlayerX * 5 + 20), 0, (chunkPlayerY * 5 + i * 5 - 20));
				Instantiate (groundPrefab, placePoint, Quaternion.Euler (90, 0, 0));
				SpawnEnemy(placePoint);
			}	
		}
		if (moveDir == 2) {
			for (int i = 0; i < world.GetLength(0); i++) {
				Vector3 placePoint = new Vector3 ((chunkPlayerX * 5 + i * 5 - 20), 0, (chunkPlayerY * 5 + 20));
				Instantiate (groundPrefab, placePoint, Quaternion.Euler (90, 0, 0));
				SpawnEnemy (placePoint);
			}
		}
		if (moveDir == 3) {
			for (int i = 0; i < world.GetLength(0); i++) {
				Vector3 placePoint = new Vector3((chunkPlayerX * 5 - 20), 0, (chunkPlayerY * 5 + i * 5 - 20));
				Instantiate (groundPrefab, placePoint, Quaternion.Euler (90, 0, 0));
				SpawnEnemy(placePoint);
			}	
		}
		if (moveDir == 4) {
			for (int i = 0; i < world.GetLength(0); i++) {
				Vector3 placePoint = new Vector3((chunkPlayerX * 5 + i * 5 - 20), 0, (chunkPlayerY * 5 - 20));
				Instantiate (groundPrefab, placePoint, Quaternion.Euler (90, 0, 0));
				SpawnEnemy(placePoint);
			}	
		}
		Debug.Log ("Time to Move!");
	}

	void SpawnEnemy(Vector3 placepoint)
	{
		float spawn = Random.Range (0, 10);
		if (spawn % 3 == 0) {
			Instantiate(enemyPrefab, placepoint, Quaternion.identity);
		}
	}

	//Swapping Weapons
	void MouseScroll(float d)
	{
		if (d < 0f)
		{
			if(currentWeapon + 1 <= numWeapons)
			{
				currentWeapon++;
			}
			else currentWeapon = 1;
			//Debug.Log ("You Scrolled Down, Current Weapon:" +currentWeapon);
		}
		else if (d > 0f)
		{
			if(currentWeapon - 1 >= 1)
			{
				currentWeapon--;
			}
			else currentWeapon = 3;
			
			//Debug.Log ("You Scrolled Up, Current Weapon:" +currentWeapon);
		}
		UpdateWeapon (currentWeapon);

	}
	//WeaponGUI Text
	void UpdateWeapon(float w)
	{
		//weaponIcon.GetComponent<Image> () = weapon1;
		if (w == 1) {
			//weaponText.text = "Staff";
			weaponIcon.sprite = weapon1;
		}
		if (w == 2) {
			//weaponText.text = "Sword";
			weaponIcon.sprite = weapon2;
		}
		if (w == 3) {
			//weaponText.text = "Bow";
			weaponIcon.sprite = weapon3;
		}

	}
	//Adding Experience
	public void AddExp(float expValue)
	{
		exp += expValue;

	}
}

