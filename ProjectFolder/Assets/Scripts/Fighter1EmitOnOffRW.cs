using UnityEngine;
using System.Collections;

public class Fighter1EmitOnOffRW : MonoBehaviour 
{
	/// <summary>
	/// Controls the thruster object on the side of the fighter pointing forwards, opposite to the direction of the forward thrusters.
	/// Applies force to the attached rigidbody, which is itelf attached to the fighter parent parent object by a fixed joint.
	/// </summary>

	public ParticleSystem particleSystem;
	public float rThrust;
	
	private GameController gameController;				// gets the gamecontroller script in order to access the gameStart1 and 2 variables.

	void Start ()
	{
		GetComponent<Renderer>().enabled = false;
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController> ();

	}

	void Update()
	{
		if (gameController.gameStart1 == true) {	

			if (Input.GetKey (KeyCode.A)) {						// enables the particle system if the ship is rotating.	
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * rThrust * Time.deltaTime), ForceMode.Impulse);
				GetComponent<Renderer> ().enabled = true;
			} 

			else if (Input.GetKey (KeyCode.S)) {
				GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * rThrust * Time.deltaTime), ForceMode.Impulse);
				GetComponent<Renderer> ().enabled = true;		// the particle system is enabled when the ship is given backward thrust.
			} 

			else {
				GetComponent<Renderer> ().enabled = false;
				GetComponent<AudioSource> ().mute = true;
			}   
		}
	}
}
