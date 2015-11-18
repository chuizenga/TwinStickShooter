using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {


	#region Weapons and Equipment
	//Equipment
	public float staffEquip = 0;
	public float swordEquip = 0;
	public float bowEquip = 0;

	public float staffDamage= 0;
	public float swordDamage= 0;
	public float bowDamage= 0;

	//Weapons
	public float currentWeapon = 1;
	private float numWeapons = 3;
	public float staffLevel = 1;
	public float bowLevel = 1;
	public float swordLevel = 1;
	public float currentWepLv = 1;
	public float currentWepXP = 0;
	private float currentWepMaxXP = 0;
	public float staffxp = 0;
	private float staffMaxXP = 100;
	public float swordxp = 0;
	private float swordMaxXP = 100;
	public float bowxp = 0;
	private float bowMaxXP = 100;
	#endregion
	#region Player Stats "Static"
	//Experience
	public float level = 1;
	public float exp = 0;
	private float maxExp;

	public float health = 100;
	public float maxHealth = 100;
	private float healthRegen = 1;

	public float mana = 100;
	public float maxMana = 100;
	private float manaRegen = 1;

	float attackPower = 10;
	float critChance = 1;
	public float defense = 0;
	public float movementSpeed = 0;
	#endregion
	#region Player State "Calculated"
	public float damage;
	public float currentWepDamage;
	public float calcCritChance;
	#endregion

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
	private Image weaponxpBar;
	private Text weaponLvText;
	private Image skillL;
	private Image skillR;
	private Image skill1;
	private Image skill2;
	private Image skill3;
	private Image skill4;
	float fade = .5F; //Alpha Level for unlearned Skills

	private Image healthBar;
	private Image manaBar;


	float healthRegenTime;
	float manaRegenTime;

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

		//Get UI Components
		xpBar = transform.FindChild ("UICanvas").FindChild ("xpBG").FindChild ("xp").GetComponent<Image> ();
		weaponIcon = transform.FindChild ("UICanvas").FindChild ("weaponIcon").GetComponent<Image> ();
		levelText = transform.FindChild ("UICanvas").FindChild ("levelText").GetComponent<Text> ();
		weaponxpBar = transform.FindChild ("UICanvas").FindChild ("weaponxpBG").FindChild ("weaponxp").GetComponent<Image> ();
		weaponLvText = transform.FindChild ("UICanvas").FindChild ("weaponIcon").FindChild ("weaponLv").GetComponent<Text> ();
		skillL = transform.FindChild ("UICanvas").FindChild ("SkillL").GetComponent<Image> ();
		skillR = transform.FindChild ("UICanvas").FindChild ("SkillR").GetComponent<Image> ();
		skill1 = transform.FindChild ("UICanvas").FindChild ("Skill1").GetComponent<Image> ();
		skill2 = transform.FindChild ("UICanvas").FindChild ("Skill2").GetComponent<Image> ();
		skill3 = transform.FindChild ("UICanvas").FindChild ("Skill3").GetComponent<Image> ();
		skill4 = transform.FindChild ("UICanvas").FindChild ("Skill4").GetComponent<Image> ();


		healthBar = transform.FindChild ("UICanvas").FindChild ("HealthBG").FindChild ("Health").GetComponent<Image> ();
		manaBar = transform.FindChild ("UICanvas").FindChild ("manaBG").FindChild ("mana").GetComponent<Image> ();

	
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
			LevelUp ();
			level++;
		}
		if (staffxp >= staffMaxXP) {
			staffxp-=staffMaxXP;
			staffLevel++;
		}
		if (swordxp >= swordMaxXP) {
			swordxp-=swordMaxXP;
			swordLevel++;
		}
		if (bowxp >= bowMaxXP) {
			bowxp-=bowMaxXP;
			bowLevel++;
		}
		//Display
		//experienceText.text = ("Lv: " + level + " XP: " + exp+"/"+maxExp);
		xpBar.fillAmount = exp / maxExp; //XP Bar
		levelText.text = "Lv. " + level; //Level Text
		weaponLvText.text = "Lv. " + currentWepLv; //Weapon Level Text
		weaponxpBar.fillAmount = currentWepXP / currentWepMaxXP; //Weapon XP Bar
		UpdateWeapon (currentWeapon); //Updates Weapon Icon, Level and XP Bar
		SkillBarUpdate(); //Shows which spells are available to use

		healthBar.fillAmount = health / maxHealth; //Health Bar
		manaBar.fillAmount = mana / maxMana; //Health Bar

		if (health < maxHealth) {
			healthRegenTime+=Time.deltaTime;
			if (healthRegenTime > 5)
			{
				health+=healthRegen;
				healthRegenTime=0;
			}
		}
		if (mana < maxMana) {
			manaRegenTime+=Time.deltaTime;
			if (manaRegenTime > 5)
			{
				mana+=manaRegen;
				manaRegenTime=0;
			}
			
		}


		damage = Mathf.RoundToInt((attackPower + currentWepDamage)*((100+currentWepLv)/100));
		calcCritChance = critChance + currentWepLv;


	}

	/// <summary>
	/// Updates the Terrain as the player moves
	/// </summary>
	/// <param name="moveDir">Move dir.</param>
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

	/// <summary>
	/// Weapon Changing via Mousewheel
	/// </summary>
	/// <param name="d">D.</param>
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
	/// <summary>
	/// Weapon UI
	/// </summary>
	/// <param name="w">The width.</param>
	void UpdateWeapon(float w)
	{
		//weaponIcon.GetComponent<Image> () = weapon1;
		if (w == 1) {
			//weaponText.text = "Staff";
			weaponIcon.sprite = weapon1;
			currentWepLv = staffLevel;
			currentWepXP = staffxp;
			currentWepMaxXP = staffMaxXP;

		}
		if (w == 2) {
			//weaponText.text = "Sword";
			weaponIcon.sprite = weapon2;
			currentWepLv = swordLevel;
			currentWepXP = swordxp;
			currentWepMaxXP = swordMaxXP;
		}
		if (w == 3) {
			//weaponText.text = "Bow";
			weaponIcon.sprite = weapon3;
			currentWepLv = bowLevel;
			currentWepXP = bowxp;
			currentWepMaxXP = bowMaxXP;
		}

	}
	//Adding Experience
	public void AddExp(float expValue)
	{
		exp += expValue;

	}
	/// <summary>
	/// Adds the weapon exp.
	/// </summary>
	/// <param name="expValue">Exp value.</param>
	public void AddWeaponExp(float expValue)
	{
		if (currentWeapon == 1) {
			staffxp +=expValue;
		}
		if (currentWeapon == 2) {
			swordxp += expValue;
		}
		if (currentWeapon == 3) {
			bowxp += expValue;
		}
	}
	/// <summary>
	/// Updates to show which spells are currently available
	/// </summary>
	public void SkillBarUpdate()
	{
		//L
		if (currentWepLv >= 0) {
			ChangeAlpha (1, 1);
		} else {
			ChangeAlpha (1, fade);
		}
		//R
		if (currentWepLv >= 5) {
			ChangeAlpha (2, 1);
		} else {
			ChangeAlpha (2, fade);
		}
		//1
		if (currentWepLv >= 10) {
			ChangeAlpha (3, 1);
		} else {
			ChangeAlpha (3, fade);
		}
		//2
		if (currentWepLv >= 15) {
			ChangeAlpha (4, 1);
		} else {
			ChangeAlpha (4, fade);
		}
		//3
		if (currentWepLv >= 20) {
			ChangeAlpha (5, 1);
		} else {
			ChangeAlpha (5, fade);
		}
		//4
		if (currentWepLv >= 25) {
			ChangeAlpha (6, 1);
		} else {
			ChangeAlpha (6, fade);
		}

	}
	/// <summary>
	/// Changes the alpha of Skill Icons
	/// </summary>
	/// <param name="skillNum">Skill number.</param>
	/// <param name="alpha">Alpha.</param>
	public void ChangeAlpha(float skillNum, float alpha)
	{	
		Image image;
		Color c;
		//L
		if (skillNum == 1) {
			image = skillL.GetComponent<Image> ();
			c = image.color;
			c.a = alpha;
			image.color = c;
		}
		//R
		if (skillNum == 2) {
			image = skillR.GetComponent<Image> ();
			c = image.color;
			c.a = alpha;
			image.color = c;
		}
		//1
		if (skillNum == 3) {
			image = skill1.GetComponent<Image> ();
			c = image.color;
			c.a = alpha;
			image.color = c;
		}
		//2
		if (skillNum == 4) {
			image = skill2.GetComponent<Image> ();
			c = image.color;
			c.a = alpha;
			image.color = c;
		}
		//3
		if (skillNum == 5) {
			image = skill3.GetComponent<Image> ();
			c = image.color;
			c.a = alpha;
			image.color = c;
		}
		//4
		if (skillNum == 6) {
			image = skill4.GetComponent<Image> ();
			c = image.color;
			c.a = alpha;
			image.color = c;
		}

	}
	public void LevelUp()
	{
		maxHealth += 10;
		maxMana += 10;
		healthRegen += 1;
		manaRegen += 1;
		attackPower += 2;
		critChance += 1;
		defense += 1;
	}
	public void SpendMana(float manaValue)
	{
		mana -= manaValue;
	}
}

