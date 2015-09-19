using UnityEngine;
using System.Collections;

public class Fighter1EmitOnOff : MonoBehaviour 
{
	/// <summary>
	/// Controls the particle system attached to main thruster of the fighter gameobject.
	/// </summary>

	private GameController gameController;

	void Start ()
	{
		GetComponent<Renderer>().enabled = false;
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController> ();

	}

	void Update()
	{
		if (gameController.gameStart1 == true) {	

			if (Input.GetKey (KeyCode.W)) {
				GetComponent<Renderer> ().enabled = true;
				GetComponent<AudioSource> ().mute = false;
			} else {
				GetComponent<Renderer> ().enabled = false;
				GetComponent<AudioSource> ().mute = true;
			} 
		}
	}
}
