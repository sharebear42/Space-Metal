using UnityEngine;
using System.Collections;


public class PlayerControllerFighter : MonoBehaviour 
{
	/// <summary>
	/// Controls the movement, collisions, fuel use and hit points of the fighter. Also updates the gui text and color for the player ships .. 
	/// health and fuel. 
	/// </summary>

	public float thrust;								// force applied in the forward direction. set in the editor.
	public float maxMag;								// maximum velocity limit for the fighter.
	public float armor;									// armor value is subtracted from potential collision damage.
	public float fighter1Hp;							// sets the fighters hit points, when this float reaches zero, the player is dead.
	public float fuel;									// sets the fighters fuel, if fuel runs to zero, the player loses the game.

	public GameObject playerExplosion;					// reference to the explosion prefab instantiated upon the players death.
	public GameObject hpText;							// reference to the hit points guitext gameobject.

	public GUIText fuelText;							// references to the fuel, hit points and ammunition guitexts..
	private GUIText hP;
	private GUIText ammoText;

	public Rigidbody asteroidRb;						// public references to collidable rigidbodies.
	public Rigidbody enemy1Rb;
	public Rigidbody enemy2Rb;
	public Rigidbody shotEnemy1Rb;
	
	private GameController gameController;				// references to the gamecontroller and fightershooter scripts.
	private FighterShooter fighterShooter;

	private float fighter1Orig;							// variables to store the original values for the hit points and fuel.
	private float fuelOrig;

	private GameObject fighterShooter1;
	private GameObject shot2;

	public Color red1;									// public reference to the color presets.
	public Color yellow1;
	public Color green1;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");		// making the declarations..
		fighterShooter1 = GameObject.FindWithTag ("FShooter1");
		hpText = GameObject.FindWithTag ("HP");
		GameObject ammoTextObj = GameObject.FindWithTag( "ScoreText");
		ammoText = ammoTextObj.GetComponent<GUIText> ();
		hP = hpText.GetComponent<GUIText> ();
		fighterShooter = fighterShooter1.GetComponent<FighterShooter> ();

		fighter1Orig = fighter1Hp;															// setting the hit points and fuel original vaules..
		fuelOrig = fuel;

		fuelText.text = "Fuel: ";															// updating the guitext.
		hP.text = "Hull Integrity: ";


		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent <GameController> ();
		}
		if (gameController == null) 
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	public void Setup1() {													// called if the player selects the fighter ship.
		float percent = (fighter1Hp / fighter1Orig) * 100.0f;				// sets up the ui for the fighter.
		string hi1 = percent.ToString ("F0");
		hP.text = "Hull Integrity: " + hi1 + " %";
		fuelText.text = "Fuel: " + fuel + " kg";
		ammoText.text = "Charges: " + fighterShooter.totShots;
	}

	void Update ()															// the update function controls the forward thrust forcing as well as ..						
	{																		// .. keeps track of the fuel usage.
		if (gameController.gameStart1 == true) {	
			if (Input.GetKey (KeyCode.W)) {
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.forward * thrust * Time.deltaTime), ForceMode.Impulse);
				float burn = Time.deltaTime;
				fuel -= burn;												// forward thrust uses 1 'fuel' per second that the key is pressed.
				float fuelF = fuel;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.S)) {									// backward thrust uses 0.67 'fuel' per second that the key is pressed.
				float burn = Time.deltaTime * 0.67f;
				fuel -= burn;
				float fuelF = fuel;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.A)) {									// left and right rotation thrust uses 0.5 'fuel' per second.
				float burn = Time.deltaTime * 0.5f;
				fuel -= burn;
				float fuelF = fuel;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.D)) {
				float burn = Time.deltaTime * 0.5f;
				fuel -= burn;
				float fuelF = fuel;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.Q)) {									// left and right horizontal thrust uses 0.33 'fuel' per second.
				float burn = Time.deltaTime * 0.33f;
				fuel -= burn;
				float fuelF = fuel;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.E)) {
				float burn = Time.deltaTime * 0.33f;
				fuel -= burn;
				float fuelF = fuel;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}

			if (GetComponent<Rigidbody> ().velocity.magnitude > maxMag) {		// sets the velocity limit.
				GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity.normalized * maxMag;
			}
		}
	}

	void OnCollisionEnter(Collision hit) {					// controls the players collisions with other objects.
		if (gameController.gameStart1 == true) {
			if (hit.gameObject.tag == "BoundR") {
				float mod = hit.relativeVelocity.magnitude * 2.0f;			// extra force is added to bounce the player off the outside boundaries.
				GetComponent<Rigidbody> ().AddForce ((Vector3.left * mod), ForceMode.Impulse);
			}
			if (hit.gameObject.tag == "BoundL") {
				float mod = hit.relativeVelocity.magnitude * 2.0f;
				GetComponent<Rigidbody> ().AddForce ((Vector3.right * mod), ForceMode.Impulse);
			}
			if (hit.gameObject.tag == "BoundUp") {				
				Destroy (gameObject);							// touching the end boundary object destroys the player.
				gameController.GameOver ();
				Instantiate (playerExplosion, transform.position, transform.rotation);
			}
			if (hit.gameObject.tag == "BoundDown") {
				float mod = hit.relativeVelocity.magnitude * 2.0f;
				GetComponent<Rigidbody> ().AddForce ((Vector3.forward * mod), ForceMode.Impulse);
			}

			if (hit.gameObject.tag == "Asteroid") {

				float damAst = hit.relativeVelocity.magnitude;				// collision damage to the ship is equal to the mass of the other object..
				float astMass = asteroidRb.GetComponent<Rigidbody> ().mass;	// .. multiplied by the difference in velocity.
				float dam = (damAst * astMass);
				GetComponent<AudioSource> ().Play ();
				if (dam > armor) {
					float damt = dam - armor;
					fighter1Hp -= damt;
					float hi = (fighter1Hp / fighter1Orig) * 100.0f;
					string hi2 = hi.ToString ("F0");						// the player ship health text is updated after taking damage.
					hP.text = "Hull Integrity: " + hi2 + " %";
				}
			}
			if (hit.gameObject.tag == "ShotEnemy1") {

				float damShot = hit.relativeVelocity.magnitude;
				float shotMass = shotEnemy1Rb.GetComponent<Rigidbody>().mass;
				float dam = (damShot * shotMass);
				GetComponent<AudioSource> ().Play ();
				if (dam > armor) {
					float damt = dam - armor;
					fighter1Hp -= damt;
					float hi = (fighter1Hp / fighter1Orig) * 100.0f;
					string hi2 = hi.ToString ("F0");
					hP.text = "Hull Integrity: " + hi2 + " %";
				}
			}
			if (hit.gameObject.tag == "Enemy1") {
				
				float damAl = hit.relativeVelocity.magnitude;
				float enemyMass = enemy1Rb.GetComponent<Rigidbody> ().mass;
				float dam = damAl * enemyMass;
				GetComponent<AudioSource> ().Play ();
				if (dam > armor) {
					float damt = dam - armor;
					fighter1Hp -= damt;
					float hi = (fighter1Hp / fighter1Orig) * 100.0f;
					string hi2 = hi.ToString ("F0");
					hP.text = "Hull Integrity: " + hi2 + " %";
				}
			}
			if (hit.gameObject.tag == "Enemy2") {
				
				float damAl = hit.relativeVelocity.magnitude;
				float enemyMass = enemy2Rb.GetComponent<Rigidbody> ().mass;
				float dam = damAl * enemyMass;
				GetComponent<AudioSource> ().Play ();
				if (dam > armor) {
					float damt = dam - armor;
					fighter1Hp -= damt;
					float hi = (fighter1Hp / fighter1Orig) * 100.0f;
					string hi2 = hi.ToString ("F0");
					hP.text = "Hull Integrity: " + hi2 + " %";
				}
			}
			if (hit.gameObject.tag == "Shot2") {									// makes sure the collider does not interfere with the shot.
				shot2 = GameObject.FindWithTag ("Shot2");
				if (shot2.GetComponent<Collider>() != null) {	
					Collider shot2C = shot2.GetComponent<Collider>();
					Physics.IgnoreCollision(this.GetComponent<Collider>(), shot2C);   
				}
			}
		}
	}
	void FixedUpdate () {
		if (fighter1Hp <= 0.0f && gameController.gameStart1 == true) {				// destroys the player if hit points fall to zero.
			Instantiate (playerExplosion, transform.position, transform.rotation);
			Destroy (gameObject);
			gameController.GameOver ();
		}
		if (fighter1Hp < fighter1Orig * 0.5f) {									// changes the color of the player health text based on hit points.
			hP.color = yellow1;
		}
		if (fighter1Hp < fighter1Orig * 0.2f) {
			hP.color = red1;
		}
		if (fuel < fuelOrig * 0.5f) {
			fuelText.color = yellow1;
		}
		if (fuel < fuelOrig * 0.2f) {
			fuelText.color = red1;
		}
		if (fuel <= 0.0f && gameController.gameStart1 == true) {				// ends the game if the players fuel falls to zero.
			gameController.GameOver ();
		}
	}
}