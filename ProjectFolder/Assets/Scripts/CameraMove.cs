using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour 
{
	/// <summary>
	/// Controls the camera.
	/// </summary>

	private GameObject fighter1;		//refence to the fighter.
	private GameObject motherShip;		//reference to the mothership.
	private Camera camera;				//refenences the game camera as a type of Camera.
	private GameObject mainCamera;		//references the camera as a GameObject.


	private GameController gameController;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");		//setting up the declarations..
		gameController = gameControllerObject.GetComponent<GameController> ();
		fighter1 = GameObject.FindWithTag ("Fighter1");
		motherShip = GameObject.FindWithTag ("DirectionSet");
		mainCamera = GameObject.FindWithTag ("MainCamera");
		camera = mainCamera.GetComponent<Camera> (); 
	}

	void FixedUpdate ()
	{
		if (gameController.gameStart1 == true && gameController.restart == false && fighter1 != null) {	//checks to see if the fighter has been selected.
			Vector3 temp1 = transform.position;
			temp1.x = fighter1.transform.position.x;
			temp1.z = fighter1.transform.position.z + 3.0f;		//sets the camera position to 300 px in front of the player.
			transform.position = temp1;

			if (Input.GetKey (KeyCode.LeftShift)) {
				Vector3 MouseWorldPosition = camera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.0f));
				transform.position = MouseWorldPosition;		//if left shift is pressed, the camera follows the mouseposition.
			}
		}

		if (gameController.gameStart2 == true && gameController.restart == false && motherShip != null) {//checks to see if the mothership has been selected.
			Vector3 temp2 = transform.position;
			temp2.x = motherShip.transform.position.x;
			temp2.z = motherShip.transform.position.z + 3.0f;		//sets the camera position to 300 px in front of the player.
			transform.position = temp2;

			if (Input.GetKey (KeyCode.LeftShift)) {
				Vector3 MouseWorldPosition = camera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.0f));
				transform.position = MouseWorldPosition;		//if left shift is pressed, the camera follows the mouseposition.
			}
		}
	}
}
	
