using UnityEngine;
using System.Collections;

public class RearDamage : MonoBehaviour {

	/// <summary>
	/// Controls the collider on the rear end of the battleship. Collisions with this object do extra damage.
	/// </summary>

	private GameObject shotEnemy1;							// references to the collidable objects.
	private GameObject asteroid;

	private PlayerControllerMS playerControllerMS;			// reference to the playercontroller scrip in order to update the players hit points.

	void Start()
	{
		GameObject motherShip = GameObject.FindWithTag ("MotherShip");
		playerControllerMS = motherShip.GetComponent<PlayerControllerMS> ();
	}

	void OnCollisionEnter(Collision hit) {					// controls the collisions with other objects.
		if (hit.gameObject.tag == "Asteroid") {
			asteroid = GameObject.FindWithTag ("Asteroid");
			float astMass = asteroid.GetComponent<Rigidbody>().mass;
			float damR = (hit.relativeVelocity.magnitude * astMass) + 2.0f;	// armor is ignored and 2.0 is added to the damage.
			playerControllerMS.motherShipHp -= damR;
			GetComponent<AudioSource> ().Play ();

			string test = damR.ToString ("F3");
			Debug.Log ("R Ast damage = " + test + " time = " + Time.time);
		}
		if (hit.gameObject.tag == "ShotEnemy1") {
			shotEnemy1 = GameObject.FindWithTag ("ShotEnemy1");
			float shotEnemy1Mass = shotEnemy1.GetComponent<Rigidbody>().mass;
			float damR = (hit.relativeVelocity.magnitude * shotEnemy1Mass) + 2.0f;
			playerControllerMS.motherShipHp -= damR;
			GetComponent<AudioSource>().Play ();
			
			string test = damR.ToString ("F3");
			Debug.Log ("Rear shot damage = " + test + " time = " + Time.time);
		}
	}
}
