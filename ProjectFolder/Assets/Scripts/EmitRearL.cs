using UnityEngine;
using System.Collections;

public class EmitRearL : MonoBehaviour {

	/// <summary>
	/// Activates the particle system when the 'MotherShip' gameobject is given rotation thrust to the left.
	/// </summary>
	

	private GameController gameController;		// gets the gamecontroller script to check if the bool 'gameStart2' is true.
	
	void Start ()
	{
		GetComponent<Renderer>().enabled = false;
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController> ();
	}
	
	void Update()
	{
		if (gameController.gameStart2 == true) {	
																		// the 'A' key rotates the player to the left.
			if (Input.GetKey (KeyCode.A)) {								// the audiosource is only played if the ship is rotating; as this ..
				GetComponent<Renderer> ().enabled = true;				// is handled by the main thruster object.
				GetComponent<AudioSource> ().mute = false;
			} 
			else if (Input.GetKey (KeyCode.S)) {
				GetComponent<Renderer> ().enabled = true;
				GetComponent<AudioSource> ().mute = false;
			}
			else {
				GetComponent<Renderer> ().enabled = false;
				GetComponent<AudioSource> ().mute = true;
			} 
		}
	}
}
