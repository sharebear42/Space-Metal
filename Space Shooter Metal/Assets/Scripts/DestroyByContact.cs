using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour 
{	
	/// <summary>
	/// Attached to the asteroid prefab, the script name is left over from the Space Shooter tutorial.
	/// Causes the them to bounce off of the boundary objects.
	/// </summary>

	private GameObject boundR;		// refences to the boundary objects..
	private GameObject boundL;
	private GameObject boundUp;
	private GameObject boundDown;

	void Start ()
	{
		boundR = GameObject.FindWithTag ("BoundR");
		boundL = GameObject.FindWithTag ("BoundL");
		boundUp = GameObject.FindWithTag ("BoundUp");
		boundDown = GameObject.FindWithTag ("BoundDown");
	}

	void OnCollisionEnter(Collision hit) 
	{					
		float frc = hit.relativeVelocity.magnitude;			// on collision with one of the boundary objects, extra force equal to ..
		float mod = frc + 2.0f;								  // the difference in velocity is applied in the opposite direction of the boundary.
		if (hit.gameObject.tag == "BoundR") {
			GetComponent<Rigidbody> ().AddForce ((Vector3.left * mod), ForceMode.Impulse);
		}
		if (hit.gameObject.tag == "BoundL") {
			GetComponent<Rigidbody> ().AddForce ((Vector3.right * mod), ForceMode.Impulse);
		}
		if (hit.gameObject.tag == "BoundUp") {
			GetComponent<Rigidbody> ().AddForce ((Vector3.down * mod), ForceMode.Impulse);
		}
		if (hit.gameObject.tag == "BoundDown") {
			GetComponent<Rigidbody> ().AddForce ((Vector3.up * mod), ForceMode.Impulse);
		}
	}
}
