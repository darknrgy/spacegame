using UnityEngine;
using System.Collections;

public class Aligner : MonoBehaviour {

	public DebugOutput debugOutput;
	public float gravityCoefficient = 1.0f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FixedUpdate() {
		
		Vector3 gravity = new Vector3(0,0,0) - rigidbody.position.normalized;
		gravity *= gravityCoefficient;
		
		debugOutput.queue(gravity.ToString());
				
		rigidbody.AddForce(gravity);
		
	}
}
