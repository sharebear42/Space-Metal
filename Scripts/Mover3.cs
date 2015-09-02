using UnityEngine;
using System.Collections;

public class Mover3 : MonoBehaviour 
{
	/// <summary>
	/// Controls the velocity and collisions of the enemy's projectile.
	/// </summary>

	public float speed;											// initial force given to the rigidbody.
	public GameObject explosion;								// reference to the explosion prefab, instantiated upon collision.

	public void Start ()										// setting the projectile object in motion.
	{
		GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * speed);
	}

	void OnCollisionEnter (Collision hit)						// instantiates the explosion prefab and destroys the projectile object upon collision.
	{
		if (hit.gameObject.tag == "Asteroid") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (hit.gameObject.tag == "MotherShip") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (hit.gameObject.tag == "Player") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (hit.gameObject.tag == "Fighter1") {
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}

		if (hit.gameObject.tag == "BoundR") {										// checking to see if the projectile hit the boundaries..
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (hit.gameObject.tag == "BoundL") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (hit.gameObject.tag == "BoundUp") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (hit.gameObject.tag == "BoundDown") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}
