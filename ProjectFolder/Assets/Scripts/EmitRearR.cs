using UnityEngine;
using System.Collections;

public class EmitRearR : MonoBehaviour {

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
		if (gameController.gameStart2 == true) {							// the audiosource is only played if the ship is rotating as this ..
																				// is handled by the 'EmitRearL' script.
			if (Input.GetKey (KeyCode.D)) {
				GetComponent<Renderer> ().enabled = true;
				GetComponent<AudioSource> ().mute = false;
			} 
			else if (Input.GetKey (KeyCode.S)) {
				GetComponent<ParticleSystem> ().startRotation = 0.0f;
				GetComponent<Renderer> ().enabled = true;
			}
			else {
				GetComponent<Renderer> ().enabled = false;
				GetComponent<AudioSource> ().mute = true;
			} 
		}
	}
}
