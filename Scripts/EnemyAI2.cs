using UnityEngine;
using System.Collections;

public class EnemyAI2 : MonoBehaviour {

	/// <summary>
	/// Controls the behavior of the the 'Enemy2' gameobject. They are activated upon instantiation and will 
	/// chase the player until it or the player is dead. They will also target the more vulnerable rear end of 
	/// the battleship if it is closer than the 'DirectionSet' child object.
	/// </summary>

	public bool playerInSight;                      // Whether or not the player is in sight.
	public float enemySpeed;
	public float damp;									// same as the EnemyAI script.
	public float maxMag;
	public float lookAtDistance;
	public float attackDistance;
	private float Distance;
	public float enemy2Hp;
	public float armor;
	public float recoil;
	
	public GameObject explosion;
	
	public Rigidbody shotRb;
	public Rigidbody shot2Rb;
	
	private GameObject fighter1;
	private GameObject directionSet;   			
	          
	private GameController gameController;
	public GameObject shotEnemy2;
	private GameObject shotEnemyIgnore;
	
	public Vector3 target;

	private float enemy1Orig;
	private float targetDistance;
	
	private GameObject shot;
	private GameObject shot2;
	
	private float fireRate;
	private float nextFire;
	private GameObject asteroid;
	private GameObject motherShip;
	
	
	void Awake ()
	{
		playerInSight = true;												// *** playerInSight is automatically set to true.
		fighter1 = GameObject.FindWithTag ("Fighter1");
		directionSet = GameObject.FindWithTag ("DirectionSet");
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController> ();
		shot = GameObject.FindWithTag ("Shot");
		shot2 = GameObject.FindWithTag ("Shot2");
		nextFire = 0.0f;
	}
	
	void Update ()
	{
		if (gameController.gameStart1 == true && gameController.restart == false && fighter1 != null) {
			Vector3 temp = fighter1.transform.position;
			target = temp;
			Distance = Vector3.Distance (target, transform.position);
			targetDistance = Vector3.Distance (fighter1.transform.position, transform.position);
			if (Distance > lookAtDistance && targetDistance > lookAtDistance) {
				if (Distance > attackDistance) {
					lookAt ();
					if (Distance > lookAtDistance) {
						Chasing();
					}
				}
			} 
			if (targetDistance < lookAtDistance) {
				lookAt ();
				playerInSight = true;
				Chasing ();
			} 
	
			if (targetDistance < attackDistance + 10.0f) {
				Shoot ();
			} 
			else if (playerInSight == true) {
				lookAt ();
				
				Chasing ();
			}
		}
		
		if (gameController.gameStart2 == true && gameController.restart == false && directionSet != null) {
			motherShip = GameObject.FindWithTag ("MotherShip");
			float temp = Vector3.Distance (directionSet.transform.position, transform.position);	// checks the distance of both the directionSet ..
			float temp1 = Vector3.Distance (motherShip.transform.position, transform.position);		// .. object AND the mothership object..
			if (temp < temp1) {																		// .. whichever is closer is set to the target.
				target = directionSet.transform.position;
				Distance = Vector3.Distance (target, transform.position);
				targetDistance = Vector3.Distance (directionSet.transform.position, transform.position);
				if (Distance > lookAtDistance && targetDistance > lookAtDistance) {
					if (Distance > attackDistance) {
						lookAt ();
						if (Distance > lookAtDistance) {
							Chasing();
						}
					}
				}
				if (targetDistance < lookAtDistance) {
					lookAt ();
					playerInSight = true;
					Chasing ();
				} 
				if (targetDistance < attackDistance + 10.0f) {
					Shoot ();
				} else if (playerInSight == true) {
					lookAt ();
					
					Chasing ();				
				}
			}
			if (temp1 < temp) {																	// checking which object is closer..
				target = motherShip.transform.position;
				Distance = Vector3.Distance (target, transform.position);
				targetDistance = Vector3.Distance (motherShip.transform.position, transform.position);
				if (Distance > lookAtDistance && targetDistance > lookAtDistance) {
					if (Distance > attackDistance) {
						lookAt ();
						if (Distance > lookAtDistance) {
							Chasing();
						}
					}
				}
				if (targetDistance < lookAtDistance) {
					lookAt ();
					playerInSight = true;
					Chasing ();
				} 
				if (targetDistance < attackDistance) {
					Shoot ();
				} else if (playerInSight == true) {
					lookAt ();
					
					Chasing ();				
				}
			}
		}
	}
	
	void FixedUpdate ()
	{
		if (GetComponent<Rigidbody>().velocity.magnitude > maxMag) {
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxMag;
		}
		
		if (enemy2Hp <= 0.0f) {
			Destroy (this.gameObject);
			Instantiate (explosion, transform.position, transform.rotation);
			gameController.enemy2Num = 1;
			gameController.ranAttack = Random.Range (10, 20) + Time.time;
		}
	}
	void lookAt ()
	{
		Quaternion rotationAngle = Quaternion.LookRotation (target - transform.position);
		
		transform.rotation = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * damp);
	}
	void Chasing () {
		GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.forward * enemySpeed * Time.deltaTime), ForceMode.Impulse);
		if (targetDistance < attackDistance + 10.0f) {
			Shoot ();
		}	
	}
	void Shoot()
	{
		fireRate = Random.Range (2.0f, 5.0f);
		if (Time.time > nextFire && targetDistance < lookAtDistance) {
			nextFire = Time.time + fireRate;
			Vector3 pos = new Vector3(transform.position.x, 0.0f, transform.position.z);
			Instantiate (shotEnemy2, pos, transform.rotation);
			GetComponent<AudioSource> ().Play ();
			GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * recoil * Time.deltaTime), ForceMode.Impulse);
		} 
		else if (targetDistance < attackDistance) {
		GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.back * enemySpeed * 0.1f), ForceMode.Impulse);
		}
		
		else if (targetDistance > lookAtDistance) {
			Chasing ();
		}
	}

	// controls the collisions with other objects.
	// the enemy2 object does not take damage from collisions with asteroids.
	void OnCollisionEnter(Collision hit) {
		if (hit.gameObject.tag == "Shot2" && gameController.restart == false) {
			fighter1 = GameObject.FindWithTag ("Fighter1");
			
			Vector3 temp = fighter1.transform.position;
			target = temp;
			lookAt ();
			playerInSight = true;
			Chasing ();
			float damShot = hit.relativeVelocity.magnitude;
			float shotMass = shot2Rb.GetComponent<Rigidbody> ().mass;
			float damSM = damShot * shotMass;
			float dam = damSM;
			GetComponent<AudioSource> ().Play ();
			if (dam > armor) {
				float damt = dam - armor;
				enemy2Hp -= damt;
				
				string test = dam.ToString ("F3");
				Debug.Log ("Enemy2 is hit by fshot= " + test + " time = " + Time.time);
			}
		}
		if (hit.gameObject.tag == "Shot" && gameController.restart == false) {
			directionSet = GameObject.FindWithTag ("DirectionSet");
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
				enemy2Hp -= damt;
				
				string test = dam.ToString ("F3");
				Debug.Log ("Enemy2 is hit = " + test + " time = " + Time.time);
			}
		}

		if (hit.gameObject.tag == "MotherShip") {
			motherShip = GameObject.FindWithTag ("MotherShip");
			
			float damShot = hit.relativeVelocity.magnitude;
			float shotMass = motherShip.GetComponent<Rigidbody> ().mass;
			float dam = damShot * shotMass;
			if (dam > armor) {
				float damt = dam - armor;
				enemy2Hp -= damt;
				
				string test = damt.ToString ("F1");
				Debug.Log ("Enemy2 is hit by MS = " + test + " time = " + Time.time);
			}
		}
		if (hit.gameObject.tag == "Fighter1") {
			fighter1 = GameObject.FindWithTag ("Fighter1");
			
			float damShot = hit.relativeVelocity.magnitude;
			float shotMass = fighter1.GetComponent<Rigidbody> ().mass;
			float damSM = damShot * shotMass;
			float dam = damSM - 2.0f;
			if (dam > armor) {
				float damt = dam - armor;
				enemy2Hp -= damt;
				
				string test = damt.ToString ("F1");
				Debug.Log ("Enemy2 is hit by Fighter = " + test + " time = " + Time.time);
			}
		}
		if (hit.gameObject.tag == "ShotEnemy1") {
			shotEnemyIgnore = GameObject.FindWithTag ("ShotEnemy1");
			if (shotEnemyIgnore.GetComponent<Collider>() != null) {	
				Collider shot2C = shotEnemyIgnore.GetComponent<Collider>();
				Physics.IgnoreCollision(this.GetComponent<Collider>(), shot2C);   
			}
			
		}
	}
}
