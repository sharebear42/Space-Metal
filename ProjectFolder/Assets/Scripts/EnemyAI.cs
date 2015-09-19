using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	/// <summary>
	/// Controls the behavior of the the 'Enemy1' gameobject. They are activated by either the player coming within a
	/// certain distance, or shooting the enemy. The enemy will then chase the player until it or the player is dead.
	/// </summary>

	public bool playerInSight;                  // Whether or not the player is in sight.

	public float enemySpeed;					// variable setting force applied to the rigidbody during movement.
	public float damp;							// controls the speed that the enemy turns.
	public float maxMag;						// maximum velocity limit.	
	public float lookAtDistance;				// distance at which the enemy will 'see' the player.
	public float attackDistance;				// distance to maintain while shooting.
	private float Distance;						// distance from the enemy to the target.
	public float enemy1Hp;						// the enemy's hit points.
	public float armor;							// the armor variable is subtracted from the potential damage of each collision.
	public float recoil;						// the amount of force applied in the opposite direction of every shot fired.

	public GameObject explosion;				// the explosion prefab.

	public Rigidbody shotRb;					// public reference to the projectile rigidbody fired by the battleship.
	public Rigidbody shot2Rb;					// public reference to the projectile rigidbody fired by the fighter.

	private GameObject fighter1;				// reference to the player as the fighter.
	private GameObject directionSet;   			// Reference to the player as the battleship, it is targeting a child object close to the ..
													// visual center of the object.          		
	private GameController gameController;		// reference to the GameController.
	public GameObject shotEnemy1;
	private GameObject shotEnemyIgnore;			// reference to the enemy's projectile prefab.

	public Vector3 target;						// variable that holds the vector3 that the enemy will look towards.
	
	private float enemy1Orig;					// enemy's starting hit points.
	private float targetDistance;				// distance to the player.
	private float fireRate;						// the time between shots.
	private float nextFire;						// a variable to hold the time in seconds in order to calculate the reloading time.
	
	private GameObject asteroid;				// refernce to the asteroid prefab.
	private GameObject motherShip;				// reference to the mothership (battleship) gameobject.


	void Awake ()
	{
	playerInSight = false;
	targetDistance = 100.0f;					// sets the 'targetDistance' to 100.0 until the player is selected.
	fighter1 = GameObject.FindWithTag ("Fighter1");
	directionSet = GameObject.FindWithTag ("DirectionSet");
	GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
	gameController = gameControllerObject.GetComponent <GameController> ();
	nextFire = 0.0f;							// initializes 'nextFire' at 0.0.
	}
	
	// called once per frame, controlls the movement of the enemy.
	void Update ()
	{
		if (gameController.gameStart1 == false && gameController.gameStart2 == false) {
			Distance = Vector3.Distance (target, transform.position);		// makes sure the enemy stays close to its starting position while..
			if (Distance > attackDistance - 6.0f) {									// .. the player chooses a ship.
				lookAt ();
				if (Distance > lookAtDistance - 20.0f) {
					Chasing();
				}
			}
				
		}
		if (gameController.gameStart1 == true && gameController.restart == false && fighter1 != null) {
			Distance = Vector3.Distance (target, transform.position);			
			targetDistance = Vector3.Distance (fighter1.transform.position, transform.position);		
			if (Distance > lookAtDistance && targetDistance > lookAtDistance) {
				if (Distance > attackDistance - 6.0f && playerInSight == false) {
					lookAt ();													// makes sure the enemy stays close to its starting position while..
					if (Distance > lookAtDistance - 20.0f) {						// .. the player is out of sight.
						Chasing();
					}
				}
			} 
			if (targetDistance < lookAtDistance) {							// if the players distance from the enemy is less than the ..
				Vector3 temp = fighter1.transform.position;						// .. 'lookAtDistance', the enemy will look at the player ..
				target = temp;														// .. and then begin chasing them.
				lookAt ();
				playerInSight = true;
				Chasing ();
			} 
			if (targetDistance < attackDistance + 10.0f) {					// the enemy will begin firing 1,000 px farther than the 'attackDistance'.
				Shoot ();													// firing is done by callng the Shoot() function.
			} 
			else if (playerInSight == true) {								// .. else look at and then chase the player.
				lookAt ();

				Chasing ();
			}
		}

		if (gameController.gameStart2 == true && gameController.restart == false && directionSet != null) {
			Distance = Vector3.Distance (target, transform.position);
			targetDistance = Vector3.Distance (directionSet.transform.position, transform.position);
			if (Distance > lookAtDistance && targetDistance > lookAtDistance) {
				if (Distance > attackDistance - 6.0f && playerInSight == false) {
					lookAt ();										// makes sure the enemy stays close to its starting position while..
					if (Distance > lookAtDistance - 20.0f) {			// .. the player is out of sight.
						Chasing();
					}
				}
			}
			if (targetDistance < lookAtDistance) {							// if the players distance from the enemy is less than the ..
				Vector3 temp = directionSet.transform.position;					// .. 'lookAtDistance', the enemy will look at the player ..
				target = temp;														// .. and then begin chasing them.
				lookAt ();
				playerInSight = true;
				Chasing ();
			} 
			if (targetDistance < attackDistance + 10.0f) {					// the enemy will begin firing 1,000 px farther than the 'attackDistance'.
				Shoot ();													// firing is done by callng the Shoot() function.
			} else if (playerInSight == true) {
				lookAt ();													// .. else look at and then chase the player.

				Chasing ();				
			}
		}
	}

	// called once per frame, checks whether the enemy's velocity as well as if their hit points are greater than zero.
	void FixedUpdate ()	
	{
		if (GetComponent<Rigidbody>().velocity.magnitude > maxMag) {
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxMag;
		}

		if (enemy1Hp <= 0.0f) {
			Destroy (this.gameObject);
			Instantiate (explosion, transform.position, transform.rotation);
		}
	}

	// rotates the enemy toward the player/target.
	void lookAt ()															
	{
		Quaternion rotationAngle = Quaternion.LookRotation (target - transform.position);
		
		transform.rotation = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * damp);
	}
	void Chasing () {														// applies force to the enemy in the forward direction.
		GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.forward * enemySpeed * Time.deltaTime), ForceMode.Impulse);
		if (targetDistance < attackDistance) {

			Shoot ();
		}

	}

	// executed when the Shoot () function is called.
	void Shoot()													
	{
		fireRate = Random.Range (2.0f, 5.0f);       				// the 'fireRate' is set for each shot as a random float between 2 and 5.

		if (Time.time > nextFire && targetDistance < lookAtDistance) {
			nextFire = Time.time + fireRate;						// the fireRate is added to time.time to determine when the enemy can fire next.
			Vector3 pos = new Vector3 (transform.position.x, 0.0f, transform.position.z);
			Instantiate (shotEnemy1, pos, transform.rotation);		// the enemy's projectile is instantiated..
			GetComponent<AudioSource> ().Play ();
			GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * recoil * Time.deltaTime), ForceMode.Impulse);  // recoil from the shot.
		} else if (targetDistance < attackDistance) {
			GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * enemySpeed * 0.5f), ForceMode.Impulse);        // backward force is ..
		}																								// .. applied to maintain attackDistance.			
		else if (targetDistance > lookAtDistance) {
			Chasing ();
		}
	}

	// Handles the physical collisions with projectiles, asteroids and the players ship.

	void OnCollisionEnter(Collision hit) {									
		if (hit.gameObject.tag == "Shot2" && gameController.restart == false) {
			fighter1 = GameObject.FindWithTag ("Fighter1");				// the fighters projectile.

			Vector3 temp = fighter1.transform.position;					// if the enemy is struck, the playerInSight is set to true.
			target = temp;
			lookAt ();
			playerInSight = true;
			Chasing ();
			float damShot = hit.relativeVelocity.magnitude;				// Damage to the enemy is calculated by multiplying the difference in ..	
			float shotMass = shot2Rb.GetComponent<Rigidbody> ().mass;		// .. velocities by the mass of the other object.
			float damSM = damShot * shotMass;
			float dam = damSM;
			GetComponent<AudioSource> ().Play ();
			if (dam > armor) {											// checks to see if the damage is greater than the armor value.
				float damt = dam - armor;
				enemy1Hp -= damt;
			}
		}
		if (hit.gameObject.tag == "Shot" && gameController.restart == false) {
			directionSet = GameObject.FindWithTag ("DirectionSet");		// the battleship's projectile.
			Vector3 temp = directionSet.transform.position;
			target = temp;
			lookAt ();
			playerInSight = true;
			Chasing ();
			float damShot = hit.relativeVelocity.magnitude;
			float shotMass = shotRb.GetComponent<Rigidbody> ().mass;
			float damSM = damShot * shotMass;
			float dam = damSM;
			GetComponent<AudioSource> ().Play ();
			if (dam > armor) {
				float damt = dam - armor;
				enemy1Hp -= damt;
				
				string test = dam.ToString ("F3");
				Debug.Log ("Enemy is hit by MShot= " + test + " time = " + Time.time);
			}
		}
		if (hit.gameObject.tag == "Asteroid") {
			asteroid = GameObject.FindWithTag ("Asteroid");
			Rigidbody asteroidRb = asteroid.GetComponent<Rigidbody>();
			float damShot = hit.relativeVelocity.magnitude;
			float shotMass = asteroidRb.mass;
			float damSM = damShot * shotMass;
			float dam = (damSM / 2.5f) - 1.0f;									// potential damage with asteroids is devided by 2.5 and then ..
			if (dam > armor) {														// .. reduced by 1 as the enemy will not avoid them.
				float damt = dam - armor;
				enemy1Hp -= damt;
				
				string test = damt.ToString ("F1");
				Debug.Log ("Enemy is hit by ast = " + test + " time = " + Time.time);
			}
		}
		if (hit.gameObject.tag == "MotherShip") {
			motherShip = GameObject.FindWithTag ("MotherShip");			// a collision with the battleship itself.

			float damShot = hit.relativeVelocity.magnitude;
			float shotMass = motherShip.GetComponent<Rigidbody> ().mass;
			float damSM = damShot * shotMass;
			float dam = damSM - 2.0f;
			if (dam > armor) {
				float damt = dam - armor;
				enemy1Hp -= damt;
				
				string test = damt.ToString ("F1");
				Debug.Log ("Enemy is hit by MS = " + test + " time = " + Time.time);
			}
		}
		if (hit.gameObject.tag == "Fighter1") {							// a collision with the fighter.
			fighter1 = GameObject.FindWithTag ("Fighter1");

			float damShot = hit.relativeVelocity.magnitude;
			float shotMass = fighter1.GetComponent<Rigidbody> ().mass;
			float damSM = damShot * shotMass;
			float dam = damSM - 2.0f;
			if (dam > armor) {
				float damt = dam - armor;
				enemy1Hp -= damt;
				
				string test = damt.ToString ("F1");
				Debug.Log ("Enemy is hit by Fighter = " + test + " time = " + Time.time);
			}
		}
		if (hit.gameObject.tag == "ShotEnemy1") {
			shotEnemyIgnore = GameObject.FindWithTag ("ShotEnemy1"); 	// makes sure the collider does not interfere with the enemy's shot.
			if (shotEnemyIgnore.GetComponent<Collider>() != null) {			// checking to make sure its not null.
				Collider shot2C = shotEnemyIgnore.GetComponent<Collider>();
				Physics.IgnoreCollision(GetComponent<Collider>(), shot2C);   
			}

		}
	}
}



	