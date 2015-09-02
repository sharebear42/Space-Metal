using UnityEngine;
using System.Collections;

public class AstMover : MonoBehaviour 
{
	/// <summary>
	/// Gives the asteroids a randomly selected mass and corresponding size from a choice of 3. 
	/// Then moves the asteroids are set on a random rotation.
	/// </summary>
								
	public float maxMag;		//upper velocity limit for the asteroids.
	public float tumble;		//force to set asteroids rotating.

	private int ran;			//variable to hold the randomized integer between 1 and 3.
	
	void Start ()
	{
		ran = Random.Range (1, 4);
		GetComponent<Rigidbody> ().mass += ran;		//sets the mass of each asteroid
		GetComponent<Rigidbody> ().transform.localScale = new Vector3 (1 + ran, 1 + ran, 1 + ran);		//sets the scale.
		GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * tumble;		//gives it a random tumbling motion.
	}
	void FixedUpdate()
	{
		if (GetComponent<Rigidbody> ().velocity.magnitude > maxMag) {		//checks to see if velocity is greater than the limit, maxMag.
			GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity.normalized * maxMag;
		}
	}
}
