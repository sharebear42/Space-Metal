using UnityEngine;
using System.Collections;

public class EmitOnOff : MonoBehaviour 
{
	/// <summary>
	/// Controls the particlesystem for the main thrusters of the battleship.
	/// Attached to 'MS Main Thrust', a child object of the mothership gameobject.
	/// </summary>

	public ParticleSystem particleSystem;		// gets the particle system.

	private GameObject motherShip;
	private float conv;
	private AudioSource audio;
	private GameController gameController;		// gets the gamecontroller script in order to access the gameStart1 and 2 variables.
	 

	void Start ()
	{
		GetComponent<Renderer>().enabled = false;
		audio = GetComponent<AudioSource> ();
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent<GameController> ();		
	}

	void Update()
	{
		if (gameController.gameStart2 == true) {			// checks to see if variable from the gamecontroler, 'gameStart2', has been set to true.
			particleSystem = (ParticleSystem)GameObject.FindObjectOfType (typeof(ParticleSystem));
			motherShip = GameObject.FindWithTag ("MotherShip");

			if (Input.GetKey (KeyCode.W)) {								// if the 'W' key is pressed, the renederer on the PS is enabled..
				Quaternion temp = transform.rotation;
				temp.y = motherShip.transform.rotation.y;
				float conv = (float)temp.y;
				GetComponent<ParticleSystem> ().startRotation = conv;
				GetComponent<Renderer> ().enabled = true;
				GetComponent<AudioSource> ().mute = false;				// the audiosource is un-muted..
			}


			else {
				GetComponent<Renderer> ().enabled = false;				// .. else both the the PS renderer.enabled is set to false, and the ..
				GetComponent<AudioSource> ().mute = true;					// the audio source is muted.
			} 
		}
	}
}
