using UnityEngine;
using System.Collections;

public class Fighter1EmitOnOffR : MonoBehaviour 
{
	/// <summary>
	/// Controls the thruster object on the left side of the fighter pointing backwards.
	/// Applies force to the attached rigidbody, which is itelf attached to the fighter parent parent object by a fixed joint.
	/// </summary>

	public ParticleSystem particleSystem;
	public float rThrust;
	
	private GameController gameController;		// gets the gamecontroller script in order to access the gameStart1 and 2 variables

	void Start ()
	{
		GetComponent<Renderer>().enabled = false;
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController> ();

	}

	void Update()
	{
		if (gameController.gameStart1 == true) {								// enables the particle system if the ship is rotating.

			if (Input.GetKey (KeyCode.D)) {
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * rThrust * Time.deltaTime), ForceMode.Impulse);		// force applied.
				GetComponent<Renderer> ().enabled = true;
			} else if (Input.GetKey (KeyCode.W)) {
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * rThrust * Time.deltaTime), ForceMode.Impulse);
				GetComponent<Renderer> ().enabled = true;					// enables the particle system if the ship is given forward thrust.
			} else {
				GetComponent<Renderer> ().enabled = false;
				GetComponent<AudioSource> ().mute = true;
			}   
		}
	}
}
