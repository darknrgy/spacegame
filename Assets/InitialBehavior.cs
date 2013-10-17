using UnityEngine;
using System.Collections;

public class InitialBehavior : MonoBehaviour {
	
	public float initialXVelocity = 0.0f;
	public float initialYVelocity = 0.0f;
	public float initialZVelocity = 0.0f;
	
	

	// Use this for initialization
	void Start () {		
		rigidbody.velocity = new Vector3 (initialXVelocity, initialYVelocity, initialZVelocity);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
