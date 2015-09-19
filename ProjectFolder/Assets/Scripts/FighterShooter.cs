using UnityEngine;
using System.Collections;

public class FighterShooter : MonoBehaviour
{
	/// <summary>
	/// Controls the gun turret object attached to the fighter. It rotates the turret to face the mouse pointer as well as 
	/// instantiates the projectile when the player fires. The script also holds the variables for the amount of ammunition the 
	/// player has as well as the time (in seconds) between shots. The UI of the ammunition and reload time is also updated from this script. 
	/// A recoil force is also applied to the attached rigidbody and the on to the parent fighter object through a customizable joint.
	/// </summary>

	public GameObject shot;								// public reference to the projectile prefab.
	public float fireRate;								// time between shots. set in the editor.
	private float nextFire;								// private variable to store the time until the player can shoot again.
	public int totShots;								// total shots the player can use. set in the editor.
	public float recoil;								// amount of force applied in the opposite direction of each shot. set in the editor.
	public Transform fighterShooter;					// reference to the transform of the object.
	
	private Camera camera1;								// camera referenced as a camera type and gameobject type.
	private GameObject mainCamera;
	private GameController gameController;				// refence to the gamecontroller script.
	
	private GUIText reload1;							// references to the reload-time text and ammo text.
	private GUIText ammoText;
	private GameObject reload1Text;
	private GameObject scoreTextObj;
	
	private int shots;									// private integer to store the number of shots taken by the player.
	
	void Start()
	{
		shots = 0;										// the number of shots taken and the time to the next shot are set to '0'.			
		nextFire = 0.0f;

		reload1Text = GameObject.FindWithTag ("Reload1Text");
		scoreTextObj = GameObject.FindWithTag ("ScoreText");
		reload1 = reload1Text.GetComponent<GUIText> ();
		ammoText = scoreTextObj.GetComponent<GUIText> ();
		reload1.text = "<Ready to Fire>";								// the reload text is set to "<Ready to Fire>" at the start of the game.
		mainCamera = GameObject.FindWithTag ("MainCamera");
		camera1 = mainCamera.GetComponent<Camera> (); 
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController> ();
	}
	
	void Update ()
	{
		if (gameController.gameStart1 == true) {			// makes the turret look at the mouse pointer once the player has selected the fighter
			Vector3 MouseWorldPosition = camera1.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.0f));
			transform.LookAt (MouseWorldPosition); 
			transform.rotation = Quaternion.Euler (new Vector3 (0, transform.rotation.eulerAngles.y, 0));
			
			
			if (Input.GetButtonDown ("Fire1") && Time.time > nextFire && shots < totShots) {
				nextFire = Time.time + fireRate;			// checks to see if time is greater than the 'nextFire' variable, if so, adds the ..
																// .. fireRate to the current time (in seconds).
				Instantiate (shot, fighterShooter.position, fighterShooter.rotation);			// instantiates the projectile.
				GetComponent<AudioSource> ().Play ();
				totShots -= 1;																	// shots var is reduced by 1.
				ammoText.text = "Charges: " + totShots;											// updates the ammo text.
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * recoil * Time.deltaTime), ForceMode.Impulse); // applies the recoil force.
				
			} 
		}
	}
	
	void FixedUpdate()
	{
		if (nextFire - Time.time >= 0.0f) {
			float rel1 = nextFire - Time.time;			// updates the reload text with the time until the player can fire. 
			if (rel1 <= 0.05f)
			{
				reload1.text = "<Ready to Fire>";
			}
			else {
				string str = rel1.ToString ("F1");
				reload1.text = "Reloading: " + str + " s";
			}
		}
	}
}
