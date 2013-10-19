using UnityEngine;
using System.Collections;
using System;

public class CarDriver : MonoBehaviour {
	
	public DebugOutput debugOutput;
	public float thrustPower = 100.0f;
	public float thrustAcceleration = 0.1f;
	public float maxDownThrustHover = 3.0f;
	public float hoverThrust = 5.0f;
	public float mouseSensivityX = 1.0f;
	public float mouseSensivityY = 1.0f;
	
	public GameObject cockpitModule;
	
	//public Component fpvCamera;
	public Component mainCamera;
	
	protected bool hoverMode = true;
	protected float xAcceleration = 0;
	protected float yAcceleration = 0;
	protected float zAcceleration = 0;
	protected float cockpitRotation;
	
	
	
	
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
		
		
		
		// fucking works!!!
		cockpitRotation += Input.GetAxis("Mouse X") * mouseSensivityX;
		
		//cockpitModule.transform.localEulerAngles = new Vector3(cockpitModule.transform.rotation.x, cockpitRotation, cockpitModule.transform.rotation.x);
		
		
		//float cockpitRotation = 90;
		
		
		cockpitModule.transform.localEulerAngles = new Vector3(cockpitModule.transform.rotation.x, -cockpitRotation, cockpitModule.transform.rotation.x);
		
		//cockpitModule.transform.rotation.y = cockpitRotation;
		
		//cockpitModule.transform.rotation.y = cockpitRotation;
		
		
		
        //}
		
		if (Input.GetKeyUp(KeyCode.R)){
			hoverMode = !hoverMode;
		}
	}
	
	void FixedUpdate(){
		
		if (hoverMode) debugOutput.queue("HOVER MODE");
		
		debugOutput.queue("Camera Y Rotation: " + cockpitRotation);
		
		// forward and reverse
		bool zInput = false;
		if (Input.GetKey(KeyCode.Space)){
			zAcceleration += thrustAcceleration;
			if (zAcceleration > thrustPower) zAcceleration = thrustPower;
			zInput = true;
		}
		if (Input.GetKey(KeyCode.LeftAlt)){
			zAcceleration -= thrustAcceleration;
			if (zAcceleration < -thrustPower) zAcceleration = -thrustPower;
			zInput = true;
		}
		if (!zInput){
			if (Math.Abs(zAcceleration) < thrustAcceleration) zAcceleration = 0;
			zAcceleration += (zAcceleration < 0) ? thrustAcceleration : -thrustAcceleration;
		}
		rigidbody.AddRelativeForce(0, 0, zAcceleration);
		
		
		
		// left and right
		bool xInput = false;
		if (Input.GetKey(KeyCode.D)){
			xAcceleration += thrustAcceleration;
			if (xAcceleration >  thrustPower) xAcceleration = thrustPower;
			xInput = true;
		}
		if (Input.GetKey(KeyCode.A)){
			xAcceleration -= thrustAcceleration;
			if (xAcceleration <  -thrustPower) xAcceleration = -thrustPower;
			xInput = true;
		}
		if (!xInput){
			if (Math.Abs(xAcceleration) < thrustAcceleration) xAcceleration = 0;
			xAcceleration += (xAcceleration < 0) ? thrustAcceleration : -thrustAcceleration;
		}
		rigidbody.AddRelativeForce(xAcceleration, 0, 0);
			
		
		// up and down
		bool yInput = false;
		float maxDownThrust = hoverMode ? maxDownThrustHover : -thrustPower;
		float yThrustCenter = hoverMode ? hoverThrust : 0;
		if (Input.GetKey(KeyCode.W)){
			yAcceleration += thrustAcceleration;
			if (yAcceleration >  thrustPower) yAcceleration = thrustPower;
			yInput = true;
		}
		if (Input.GetKey(KeyCode.S)){
			yAcceleration -= thrustAcceleration;
			if (yAcceleration <  maxDownThrust) yAcceleration = maxDownThrust;
			yInput = true;
		}
		
		if (!yInput){
			if (Math.Abs(yAcceleration - yThrustCenter) < thrustAcceleration) yAcceleration = yThrustCenter;
			yAcceleration += (yAcceleration < yThrustCenter) ? thrustAcceleration : -thrustAcceleration;
		}
		rigidbody.AddRelativeForce(0, yAcceleration, 0);
		
	}
}
