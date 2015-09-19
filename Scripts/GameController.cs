using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	/// <summary>
	/// Controls the players choice between ships at the start of the game as well as the spawning of the asteroids and the second enemy
	/// character, 'Enemy2'. Also controls the random selection between the four Metallica tracks. Updates the gameover, restart, and 
	/// velocity ui text as well as their color. Controls the restarting of the level if the player either dies or completes the objective
	/// </summary>


	public Vector3 spawnValues;						// controls the x and z positions of the asteroid spawning. set in the editor.

	public int hazards1;							// integer variable for the number of asteroids spawned in the middle quadrant. set in the editor.
	public int hazards2;							// integer variable for the number of asteroids spawned in the left quadrant. set in the editor.
	public int hazards3;							// integer variable for the number of asteroids spawned in the right quadrant. set in the editor.
	public int enemy2Num;							// integer to limit the number of enemy2 objects spawned at one time.

	public GameObject hazard;						// reference to the asteroid prefab.
	public GameObject enemy1;						// reference to the enemy1 prefab.
	public GameObject motherShip;					// reference to the battleship.
	public GameObject fighter1;						// reference to the fighter.
	public GameObject asteroid;						// ref to the asteroid prefab.
	public GameObject enemy2;						// ref to the enemy2 prefab.

	public Rigidbody fighter1Rb;					// reference to the rigidbody attached to the fighter object.
	public Rigidbody motherShipRb;					// reference to the rigidbody attached to the battleship.
	
	public GUIText restartText;						// reference to the restart, gameover and velocity guitexts.
	public GUIText gameOverText;
	public GUIText playerVel;

	public bool restart;							// public bools controlling the restart of the game as well as ship choice by the player.
	public bool gameStart1;
	public bool gameStart2;

	public GameObject track;						// reference to the Metallica tracks.
	public GameObject track1;
	public GameObject track2;

	public Color red1;								// color references.
	public Color yellow1;
	public Color green1;

	private bool gameOver;							// accessed through the public GameOver() function.

	private int ships;								// private integers to make sure that only one ship can be selected by the player.
	private int totalShips;

	private PlayerControllerMS playerControllerMS;	// references to the playercontroller scripts.
	private PlayerControllerFighter playerControllerFighter;

	public float ranAttack;						// stores the random time until the next enemy2 is instantiated.

	void Start ()					// *** Setting up the game.. ***
	{
		gameStart1 = false;							// setting the ints and bools..
		gameStart2 = false;
		ships = 0;
		totalShips = 1;
		gameOver = false;
		restart = false;

		restartText.text = "";						// setting up the guitexts..
		gameOverText.text = "Cross the Asteroid Field -> -> \n Then Reduce Velocity to < 100 m/s";
		playerVel.text = "Velocity: ";

		playerControllerMS = motherShip.GetComponent<PlayerControllerMS> ();
		playerControllerFighter = fighter1.GetComponent<PlayerControllerFighter> ();

		int ran = Random.Range (0, 4);				// randomly choosing 1 of 4 Metallica songs..
		if (ran == 0) {
			GetComponent<AudioSource>().Play();		
		}
		if (ran == 1) {
			track.GetComponent<AudioSource>().Play();
		}
		if (ran == 2) {
			track1.GetComponent<AudioSource>().Play();
		}
		if (ran == 3) {
			track2.GetComponent<AudioSource>().Play();
		}

		while (hazards1 > 0) {				// randomly spawning the asteroids into middle(hazards1), left(hazards2) and right(hazards3) quadrants.
			if (hazards1 > 0) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range (40.0f, spawnValues.z));
				Quaternion spawnRotation = Quaternion.Euler (0.0f, Random.Range (0.0f, 360.0f), 0.0f);
				Instantiate (hazard, spawnPosition, spawnRotation);
			}
			hazards1 -= 1;
		}
		while (hazards2 > 0) {
			if (hazards2 > 0) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x - 55.0f, spawnValues.x - 55.0f), spawnValues.y, Random.Range (40.0f, spawnValues.z));
				Quaternion spawnRotation = Quaternion.Euler (0.0f, Random.Range (0.0f, 360.0f), 0.0f);
				Instantiate (hazard, spawnPosition, spawnRotation);
			}
			hazards2 -= 1;
		}
		while (hazards3 > 0) {
			if (hazards3 > 0) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x + 55.0f, spawnValues.x + 55.0f), spawnValues.y, Random.Range (40.0f, spawnValues.z));
				Quaternion spawnRotation = Quaternion.Euler (0.0f, Random.Range (0.0f, 360.0f), 0.0f);
				Instantiate (hazard, spawnPosition, spawnRotation);
			}
			hazards3 -= 1;
		}

		ranAttack = Random.Range (30, 70) + Time.time;				// sets the time until enemy2 spawns to a random int between 30 and 69 seconds.
	}

	void Update ()							// based on the input, destroys the ship object not chosen by the player.
	{
		if (gameStart1 == false && gameStart2 == false) {
			if (Input.GetKeyDown (KeyCode.F) && ships < totalShips) {
				Destroy (motherShip.gameObject);
				gameStart1 = true;
				ships = + 1;
				playerControllerFighter.Setup1();
				gameOverText.text = "";											// resets the gameover text to "".
				GetComponent<AudioSource> ().volume = 0.33f;					// and turns it up to 11..
				track.GetComponent<AudioSource> ().volume = 0.33f;
				track1.GetComponent<AudioSource> ().volume = 0.33f;
				track2.GetComponent<AudioSource> ().volume = 0.33f;
			}
			
			if (Input.GetKeyDown (KeyCode.B) && ships < totalShips) {
				Destroy (fighter1);
				gameStart2 = true;
				ships = + 1;
				playerControllerMS.Setup2();
				gameOverText.text = "";
				GetComponent<AudioSource> ().volume = 0.33f;
				track.GetComponent<AudioSource> ().volume = 0.33f;
				track1.GetComponent<AudioSource> ().volume = 0.33f;
				track2.GetComponent<AudioSource> ().volume = 0.33f;
			}
		}

		if (gameOver && restart == false)										// restarts the game if the gameover bool is set to true.
		{
			restartText.text = "Press 'R' for Restart";
			restart = true;	
		}
		if (restart) 
		{
			if (Input.GetKeyDown (KeyCode.R)) 									// reloads the level when the player presses 'R' when restart is true.
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}

	void FixedUpdate ()
	{
		if (gameStart1 == true && restart == false && fighter1 != null) {		// sets the text and color of the velocity UI, the color is set to ..
			Vector3 v3Velocity = fighter1Rb.velocity;							// .. green at speeds under 1,500 m/s, yellow between 1,500 and ..
			float vel1 = v3Velocity.magnitude * 200.0f;							// .. 2,500 m/s, and red if it is over 2,500 m/s.
			string str1 = vel1.ToString ("N0");
			playerVel.text = "Velocity: " + str1 + " m/s";
			if (v3Velocity.magnitude <= 7.5f) {
				playerVel.color = green1;
			}
			if (v3Velocity.magnitude > 7.5f) {
				playerVel.color = yellow1;
			}
			if (v3Velocity.magnitude > 12.5f) {
				playerVel.color = red1;
			}
			if (fighter1.transform.position.z > 270.0f && fighter1.transform.position.z < 285.0f && fighter1.GetComponent<Rigidbody>().velocity.magnitude < 0.5f) {
				gameOverText.text = "You're Winner!";							// resets the the gameover text and sets restart equal to true ..
				gameOverText.color = yellow1;									// .. if the player is able to bring their velocity to below .. 
				restartText.text = "Press 'R' for Restart";						// .. 100 m/s within the target zone on the z axis.
				restart = true;	
			}

			if (Time.time > ranAttack && enemy2Num > 0) {
				Instantiate (enemy2, new Vector3(Random.Range (-70, 70), 0.25f, -20.0f), transform.rotation);		// instantiates the enemy2 prefab at a random point ..
				enemy2Num -= 1;																						//.. along the bottom of the game area.														
			}
		}
		
		if (gameStart2 == true && restart == false && motherShip != null) {                                          // same as above for the battleship
			Vector3 v3Velocity = motherShipRb.velocity;
			float vel2 = v3Velocity.magnitude * 200.0f;
			string str2 = vel2.ToString ("N0");
			playerVel.text = "Velocity: " + str2 + " m/s";
			if (v3Velocity.magnitude <= 7.5f) {
				playerVel.color = green1;
			}
			if (v3Velocity.magnitude > 7.5f) {
				playerVel.color = yellow1;
			}
			if (v3Velocity.magnitude > 12.5f) {
				playerVel.color = red1;
			}
			if (motherShip.transform.position.z > 270.0f && motherShip.transform.position.z < 285.0f && motherShip.GetComponent<Rigidbody>().velocity.magnitude < 0.5f) {
				gameOverText.text = "You're Winner!";
				gameOverText.color = yellow1;
				restartText.text = "Press 'R' for Restart";
				restart = true;	
			}
			if (Time.time > ranAttack && enemy2Num > 0) {
				Instantiate (enemy2, new Vector3(Random.Range (-70, 70), 0.25f, -20.0f), transform.rotation);
				enemy2Num -= 1;
			}
		}

		if (Input.GetKey (KeyCode.LeftAlt) && Input.GetKeyDown (KeyCode.R)) {	// restarts the game if the left alt and 'R' keys are pressed together.
			GameOver ();
		}

		if (Input.GetKey ("escape")) {											// exits the game.
			Application.Quit ();
		}
	}

	public void GameOver ()														// public gameover() function.
	{
		if (restart == false) {	
			gameOverText.text = "Game Over!";
			gameOverText.color = red1;
			gameOver = true;
		}
	}
}
