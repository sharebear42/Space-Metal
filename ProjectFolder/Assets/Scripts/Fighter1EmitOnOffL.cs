using UnityEngine;
using System.Collections;

public class Fighter1EmitOnOffL : MonoBehaviour 
{
	/// <summary>
	/// Controls the thruster object on the right side of the fighter pointing backwards. Enables the PS when the ship is rotating to the left.
	/// Applies force to the attached rigidbody, which is itelf attached to the fighter parent parent object by a fixed joint.
	/// </summary>
	
	public float lThrust;						
	
	private GameController gameController;		// gets the gamecontroller script in order to access the gameStart1 and 2 variables.

	void Start ()
	{
		GetComponent<Renderer>().enabled = false;
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController> ();

	}

	void Update()
	{
		if (gameController.gameStart1 == true) {			// enables the particle system and the audiosource if the ship is rotating.	

			if (Input.GetKey (KeyCode.A)) {
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * lThrust * Time.deltaTime), ForceMode.Impulse);	// force applied.
				GetComponent<Renderer> ().enabled = true;
				GetComponent<AudioSource> ().mute = false;
			} else if (Input.GetKey (KeyCode.W)) {
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * lThrust * Time.deltaTime), ForceMode.Impulse);
				GetComponent<Renderer> ().enabled = true;			// enables the particle system if the ship is given forward thrust.
			} else {
				GetComponent<Renderer> ().enabled = false;
				GetComponent<AudioSource> ().mute = true;
			} 
		}
	}
}
