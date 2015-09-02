using UnityEngine;
using System.Collections;

public class MoverBolt2 : MonoBehaviour 
{
	/// <summary>
	/// Controls the velocity and collisions of the fighter's projectile.
	/// </summary>

	public float speed;											// initial force given to the rigidbody.
	public GameObject explosion;								// reference to the explosion prefab, instantiated upon collision.

	public void Start ()
	{
		GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * speed);			// setting the projectile object in motion.
	}

	void OnCollisionEnter (Collision hit)
	{
		if (hit.gameObject.tag == "Asteroid") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (hit.gameObject.tag == "Enemy1") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (hit.gameObject.tag == "Enemy2") {
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}

		if (hit.gameObject.tag == "BoundR") {											// checking to see if the projectile hit the boundaries..
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
