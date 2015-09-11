using UnityEngine;
using System.Collections;

public class EmitStraf1 : MonoBehaviour {

	/// <summary>
	/// Controls the particle system while the mothership is applying horizontal thrusters. 
	/// </summary>

	public ParticleSystem particleSystem;
	
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
			particleSystem = (ParticleSystem)GameObject.FindObjectOfType (typeof(ParticleSystem));
			
			if (Input.GetKey (KeyCode.Q)) {
				GetComponent<ParticleSystem> ().startRotation = 0.0f;
				GetComponent<Renderer> ().enabled = true;
				GetComponent<AudioSource> ().mute = false;
			} else {
				GetComponent<Renderer> ().enabled = false;
				GetComponent<AudioSource> ().mute = true;
			} 
		}
	}
}
