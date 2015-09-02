using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour 
{
	/// <summary>
	/// Attached to projectile gameobjects. Makes sure they are destroyed.
	/// </summary>

	public float lifetime;		// .. in seconds.

	void Start ()
	{
		Destroy (gameObject, lifetime);
	}
}
