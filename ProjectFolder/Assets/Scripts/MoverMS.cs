using UnityEngine;
using System.Collections;

public class MoverMS : MonoBehaviour 
{
	/// <summary>
	/// Controls the velocity and collisions of the battleship's projectile. Unlike the fighter's projectile, the battleship's adds an additional explosion force that 
	/// throws pushes all nearby objects away from the explosion point. The force script can be found in the Prefabs/VFX/explosions folder  
	/// </summary>

	public float speed;										// initial force given to the rigidbody.
	public GameObject explosion;							// reference to the explosion prefab, instantiated upon collision.

	public void Start ()
	{
		GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * speed);				// setting the projectile object in motion.
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
		if (hit.gameObject.tag == "BoundR") {												// checking to see if the projectile hit the boundaries..
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
