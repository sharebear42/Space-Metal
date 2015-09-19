using UnityEngine;
using System.Collections;

public class Enemy2PSOnOff : MonoBehaviour {

	/// <summary>
	/// Controls the particle system attached to the 'Enemy2' prefab child object.
	/// </summary>

	void Start () {
		GetComponent<Renderer>().enabled = false;
	}

	void FixedUpdate () {
		if (this.GetComponent<Rigidbody> () != null && this.GetComponent<Rigidbody> ().velocity.magnitude > 0.2f) {
			GetComponent<Renderer> ().enabled = true;		// enables the PS if the velocity is greater than 0.2.
		} else {
			GetComponent<Renderer> ().enabled = false;
		}
	}
}
