using UnityEngine;
using System.Collections;

public class YawAligner : MonoBehaviour {
	
	public GameObject velocityVector;
	public GameObject yawAligner;
	
	void FixedUpdate(){
		
		//velocityVector.transform.localPosition = -rigidbody.velocity.normalized;
		velocityVector.transform.position = transform.position + rigidbody.velocity.normalized;
		
		float angle = Vector3.Angle(new Vector3(1,0,0), velocityVector.transform.localPosition);
		
		
		float zDelta = velocityVector.transform.localPosition.z;
		//Debug.Log("xDelta: " + zDelta);
		if (zDelta > 0) angle *= -1;
		rigidbody.transform.Rotate(0, angle*0.001f,0);
		
		
		//yawAligner.rigidbody.AddRelativeForce(new Vector3(0, 0, 10000.0f));
		
	}
}
