using UnityEngine;
using System.Collections;

public class CarDriver : MonoBehaviour {
	
	public DebugOutput debugOutput;
	public float thrustPower = 100.0f;
	public float mouseSensivityX = 1.0f;
	public float mouseSensivityY = 1.0f;
	
	//public Component fpvCamera;
	public Component mainCamera;
	
	protected bool hoverMode = true;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
			
		//transform.Rotate(Input.GetAxis("Mouse Y") * -mouseSensivityX, 0, 0);
		//fpvCamera.transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensivityX, 0);
		
		/* working mouse rotation logic */
		transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensivityX, 0);
		//transform.Rotate (0, 0, +Input.GetAxis("Mouse Y") * mouseSensivityY);
	
		
		mainCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * mouseSensivityX, 0,  0);
		//fpvCamera.camera.transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensivityX, 0);
		
		
		
		
		
		
		
        //}
		
		if (Input.GetKeyUp(KeyCode.R)){
			hoverMode = !hoverMode;
		}
	}
	
	void FixedUpdate(){
		
		debugOutput.queue("Camera rotation: " + mainCamera.transform.localEulerAngles.ToString());
		
		Vector3 cg = new Vector3(0, -5, 0);
		
		
		if (Input.GetKey(KeyCode.Space)){
			rigidbody.AddRelativeForce(0, 0, thrustPower);
			debugOutput.queue ("thrusting forward");
		}
		
		if (Input.GetKey(KeyCode.LeftAlt)){
			rigidbody.AddRelativeForce(0, 0, -thrustPower);
		}
		
		if (Input.GetKey(KeyCode.W)){
			rigidbody.AddRelativeForce(0, thrustPower, 0);
		}
		
		if (Input.GetKey(KeyCode.S)){
			rigidbody.AddRelativeForce(0, -thrustPower, 0);
		}
		
		
		float yThrust = 0.0f;
		if (Input.GetKey(KeyCode.W)) yThrust = thrustPower;
		
		
		if (hoverMode){
			yThrust = 5.0f;
			
			if (Input.GetKey(KeyCode.S)) yThrust = 3.0f;
			debugOutput.queue("HOVER MODE");
		}else{
			if (Input.GetKey(KeyCode.S)) yThrust = -thrustPower;
		}
		
		rigidbody.AddRelativeForce(0, yThrust, 0);
		
		if (Input.GetKey(KeyCode.A)){
			rigidbody.AddRelativeForce(-thrustPower, 0, 0);
		}
		
		if (Input.GetKey(KeyCode.D)){
			rigidbody.AddRelativeForce(thrustPower, 0, 0);
		}
		
		//rigidbody.angularVelocity.magnitude = 0;
		
		
		
		
		
		
		
	}
}
