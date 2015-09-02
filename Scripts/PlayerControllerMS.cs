using UnityEngine;
using System.Collections;


public class PlayerControllerMS : MonoBehaviour 
{
	/// <summary>
	/// Controls the movement, collisions, fuel use and hit points of the battleship. Also updates the gui text and color for the player ships .. 
	/// health and fuel. 
	/// </summary>

	public float thrust;									// force applied in the forward direction. set in the editor.
	public float backThrust;								// force applied during backward thrusting.
	public float turnSpeed;									// rotational force applied.
	public float strafeSpeed;								// horizontal force applied.
	public float motherShipHp;								// sets the battleships hit points, when this float reaches zero, the player is dead.
	public float armor;										// armor value is subtracted from potential collision damage.
	public float fuel;										// sets the battleships fuel, if fuel runs to zero, the player loses the game.

	public GameObject explosion;							// reference to the explosion prefab instantiated upon the players death.

	public Rigidbody asteroidRb;							// public references to collidable rigidbodies.
	public Rigidbody shotEnemy1Rb;
	public Rigidbody rear;
	public Rigidbody enemy1Rb;

	public GUIText fuelText;								// references to the fuel, hit points and ammunition guitexts..
	public GUIText hP;
	private GUIText ammoText;

	private ShotSpawn1 shotSpawn1;							// references to the gamecontroller and shotspawn1 scripts.
	private GameController gameController;
	
	private GameObject asteroid;
	private GameObject shotEnemy1;

	private float fuelOrig;									// variables to store the original values for the hit points and fuel.
	private float motherShipOrig;

	private GameObject shot;
	private GameObject enemy1;

	public Color red1;										// public reference to the color presets.
	public Color yellow1;
	public Color green1;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		GameObject ammoTextObj = GameObject.FindWithTag( "ScoreText");
		ammoText = ammoTextObj.GetComponent<GUIText> ();
		GameObject shotSpawn1Obj = GameObject.FindWithTag( "ShotSpawn1");
		shotSpawn1 = shotSpawn1Obj.GetComponent<ShotSpawn1> ();

		motherShipOrig = motherShipHp;										// setting the hit points and fuel original vaules..
		fuelOrig = fuel;


		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent <GameController> ();
		}
		if (gameController == null) 
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	public void Setup2() {													// called if the player selects the battleship.
		float percent = (motherShipHp / motherShipOrig) * 100.0f;			// sets up the ui for the battleship.
		string hi1 = percent.ToString ("F0");
		hP.text = "Hull Integrity: " + hi1 + " %";
		fuelText.text = "Fuel: " + fuel + " kg";
		ammoText.text = "Charges: " + shotSpawn1.totShots;
	}

	void Update ()					// controls all of the forces applied to move the battleship in addition to controlling the fuel use.				
	{
		if (gameController.gameStart2 == true && fuel > 0.0f) {	
			float fuelD = fuel * 10.0f;
			string temp1 = fuelD.ToString ("F1");
			fuelText.text = "Fuel: " + temp1 + " kg";
			if (Input.GetKey (KeyCode.W)) {							// 'W' gives the ship a forward impulse multiplied by the var 'thrust'.
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.forward * thrust * Time.deltaTime), ForceMode.Impulse);
				float burn = Time.deltaTime;
				fuel -= burn;										// forward thrust uses 1 fuel per second.
				float fuelF = fuel * 10.0f;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.S)) {
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * backThrust * Time.deltaTime), ForceMode.Impulse);
				float burn = Time.deltaTime * 0.5f;
				fuel -= burn;										// backward thrust uses 0.5 fuel per second.
				float fuelF = fuel * 10.0f;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.D)) {
				rear.GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.left * turnSpeed * Time.deltaTime), ForceMode.Impulse); //turn right
				float burn = Time.deltaTime * 0.33f;
				fuel -= burn;										// rotational thrust uses 0.33 fuel per second.
				float fuelF = fuel * 10.0f;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.A)) {
				rear.GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.right * turnSpeed * Time.deltaTime), ForceMode.Impulse); //turn left
				float burn = Time.deltaTime * 0.33f;
				fuel -= burn;
				float fuelF = fuel * 10.0f;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.E)) {
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.right * strafeSpeed * Time.deltaTime), ForceMode.Impulse); //turn right (fine)
				float burn = Time.deltaTime * 0.5f;
				fuel -= burn;										// horizontal thrust uses 0.5 fuel per second.
				float fuelF = fuel * 10.0f;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
			if (Input.GetKey (KeyCode.Q)) {
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.left * strafeSpeed * Time.deltaTime), ForceMode.Impulse);//turn left (fine)
				float burn = Time.deltaTime * 0.5f;
				fuel -= burn;
				float fuelF = fuel * 10.0f;
				string temp = fuelF.ToString ("F1");
				fuelText.text = "Fuel: " + temp + " kg";
			}
		}
	}

	void OnCollisionEnter(Collision hit) {							// controls the players collisions with other objects.
		if (gameController.gameStart2 == true) {

			if (hit.gameObject.tag == "Asteroid") {					// collision damage to the ship is equal to the mass of the other object..
																	// .. multiplied by the difference in velocity.
				float damAst = hit.relativeVelocity.magnitude;
				float astMass = asteroidRb.GetComponent<Rigidbody> ().mass;
				float damA = damAst * astMass;
				GetComponent<AudioSource> ().Play ();
				if (damA > armor) {
					float damt = damA - armor;
					motherShipHp -= damt;
					float hi = (motherShipHp / motherShipOrig) * 100.0f;
					string hi2 = hi.ToString ("F0");
					hP.text = "Hull Integrity: " + hi2 + " %";				// the player ship health text is updated after taking damage.
				}
			}
			if (hit.gameObject.tag == "ShotEnemy1") {

				float damShot = hit.relativeVelocity.magnitude;
				float shotMass = shotEnemy1Rb.GetComponent<Rigidbody> ().mass;
				float damSM = damShot * shotMass;
				float dam = damSM;
				GetComponent<AudioSource> ().Play ();
				if (dam > armor) {
					float damt = dam - armor;
					motherShipHp -= damt;
					float hi = (motherShipHp / motherShipOrig) * 100.0f;
					string hi2 = hi.ToString ("F0");
					hP.text = "Hull Integrity: " + hi2 + " %";
				}
			}
			if (hit.gameObject.tag == "Enemy1") {

				float damShot = hit.relativeVelocity.magnitude;
				float shotMass = enemy1Rb.GetComponent<Rigidbody> ().mass;
				float damSM = damShot * shotMass;
				float dam = damSM;
				GetComponent<AudioSource> ().Play ();
				if (dam > armor) {
					float damt = dam - armor;
					motherShipHp -= damt;
					float hi = (motherShipHp / motherShipOrig) * 100.0f;
					string hi2 = hi.ToString ("F0");
					hP.text = "Hull Integrity: " + hi2 + " %";
				}
			}
			else if (hit.gameObject.tag == "BoundR") {
				float mod = hit.relativeVelocity.magnitude * 8.0f;
				GetComponent<Rigidbody> ().AddForce ((Vector3.left * mod), ForceMode.Impulse);
			}
			else if (hit.gameObject.tag == "BoundL") {
				float mod = hit.relativeVelocity.magnitude * 8.0f;
				GetComponent<Rigidbody> ().AddForce ((Vector3.right * mod), ForceMode.Impulse);
			}
			else if (hit.gameObject.tag == "BoundUp") {
				Destroy (gameObject);									// touching the end boundary object destroys the player.
				gameController.GameOver ();
				Instantiate (explosion, transform.position, transform.rotation);
			}
			else if (hit.gameObject.tag == "BoundDown") {
				float mod = hit.relativeVelocity.magnitude * 8.0f;
				GetComponent<Rigidbody> ().AddForce ((Vector3.forward * mod), ForceMode.Impulse);
			}
			else if (hit.gameObject.tag == "Shot") {
				Physics.IgnoreCollision(GetComponent<Collider>(), hit.gameObject.GetComponent<Collider>());
			}
		}
	}
	void FixedUpdate() {
		float hi = (motherShipHp / motherShipOrig) * 100.0f;
		string hi2 = hi.ToString ("F0");
		hP.text = "Hull Integrity: " + hi2 + " %";
		if (motherShipHp <= 0.0f && gameController.gameStart2 == true) {			// destroys the player if hit points fall to zero.
			Destroy (gameObject);
			gameController.GameOver ();
			Instantiate (explosion, transform.position, transform.rotation);
		}
		if (motherShipHp < motherShipOrig * 0.5f) {							// changes the color of the player health text based on hit points.
			hP.color = yellow1;
		}
		if (motherShipHp < motherShipOrig * 0.2f) {
			hP.color = red1;
		}
		if (fuel < fuelOrig * 0.5f) {
			fuelText.color = yellow1;
		}
		if (fuel < fuelOrig * 0.2f) {
			fuelText.color = red1;
		}
		if (fuel <= 0.0f && gameController.gameStart2 == true) {			// ends the game if the players fuel falls to zero.
			gameController.GameOver ();
		}
	}
}