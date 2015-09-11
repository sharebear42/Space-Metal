using UnityEngine;
using System.Collections;

public class ShotSpawn1 : MonoBehaviour {

	/// <summary>
	/// Controls the gun turret object attached to the battleship. It rotates the turret to face the mouse pointer as well as 
	/// instantiates the projectile when the player fires. The script also holds the variables for the amount of ammunition the 
	/// player has as well as the time (in seconds) between shots. The UI of the ammunition and reload time is also updated from this script. 
	/// A recoil force is also applied to the attached rigidbody and the on to the parent battleship object through a customizable joint.
	/// </summary>

	public GameObject shot;								// public reference to the projectile prefab.
	public float fireRate;								// time between shots. set in the editor.
	private float nextFire;								// private variable to store the time until the player can shoot again.
	public int totShots;								// total shots the player can use. set in the editor.
	public float recoil;								// amount of force applied in the opposite direction of each shot. set in the editor.
	public float damp;									// variable used to control the turn speed of the turret.
	
	private Camera camera;								// camera referenced as a camera type and gameobject type.
	private GameObject mainCamera;
	private GameController gameController;				// refence to the gamecontroller script.
	
	private GUIText reload1;							// references to the reload-time text and ammo text.
	private GUIText ammoText;

	private GameObject reload1Text;
	private GameObject ammoTextObj;


	private int shots;									// private integer to store the number of shots taken by the player.

	void Start()
	{
		shots = 0;										// the number of shots taken and the time to the next shot are set to '0'.	
		nextFire = 0.0f;

		reload1Text = GameObject.FindWithTag ("Reload1Text");
		ammoTextObj = GameObject.FindWithTag ("ScoreText");
		reload1 = reload1Text.GetComponent<GUIText> ();
		ammoText = ammoTextObj.GetComponent<GUIText> ();
		reload1.text = "<Ready to Fire>";
		mainCamera = GameObject.FindWithTag ("MainCamera");
		camera = mainCamera.GetComponent<Camera> (); 
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController> ();
	}
	
	void Update ()
	{
		if (gameController.gameStart2 == true) {			// makes the turret look at the mouse pointer once the player has selected the battleship.
			Vector3 MouseWorldPosition = camera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.0f));
			Quaternion rotationAngle = Quaternion.LookRotation (MouseWorldPosition - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * damp); // turn speed is controlled with a var.
			
			
			if (Input.GetButtonDown ("Fire1") && Time.time > nextFire && shots < totShots) {
				nextFire = Time.time + fireRate;			// checks to see if time is greater than the 'nextFire' variable, if so, adds the ..
																// .. fireRate to the current time (in seconds).
				Instantiate (shot, transform.position, transform.rotation);			// instantiates the projectile.
				GetComponent<AudioSource> ().Play ();
				totShots -= 1;														// shots var is reduced by 1.
				ammoText.text = "Charges: " + totShots;								// updates the ammo text.
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * recoil * Time.deltaTime), ForceMode.Impulse);
				
			} 
		}
	}
	
	void FixedUpdate()
	{
		if (nextFire - Time.time >= 0.0f) {
			float rel1 = nextFire - Time.time;								// updates the reload text with the time until the player can fire. 
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
