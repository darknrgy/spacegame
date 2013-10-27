using UnityEngine;
using System.Collections;
using System;

public class CarDriver : MonoBehaviour {
	
	public DebugOutput debugOutput;
	public GameObject aligner;
	public float thrustPower = 100.0f;
	public float thrustAcceleration = 0.1f;
	public float hoverVerticalThrustDelta = 2.0f;
	public float mouseSensivityX = 1.0f;
	public float mouseSensivityY = 1.0f;
	public TextMesh velocityHUD;
	public TextMesh verticalVelocityHUD;
	public TextMesh altitudeHUD;
	public TextMesh hoverHUD;
	public GameObject mapToggle;
	
	public GameObject soundUp;
	public GameObject soundDown;
	public GameObject soundLeft;
	public GameObject soundRight;
	public GameObject soundForward;
	public GameObject soundReverse;
	public GameObject soundHover;
	
	public GameObject soundAnnounceHover;
	public GameObject soundAnnounceOrbit;
	
	protected const float GRAVITATIONAL_CONSTANT = 1;
	protected float planetMass;
	
	
	
	
	public GameObject cockpitModule;
	
	//public Component fpvCamera;
	public Component mainCamera;
	
	protected bool hoverMode = true;
	protected float xAcceleration = 0;
	protected float yAcceleration = 0;
	protected float zAcceleration = 0;
	
	protected float xMouseAccumulator = 0;
	protected float lastVPosition = 0;
	protected float vVelocity = 0;
	
	public void Start () {
		planetMass = 1100 / GRAVITATIONAL_CONSTANT;
		rigidbody.centerOfMass = new Vector3(0, 0, 0);
		if (hoverMode) yAcceleration = GravityForce();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		xMouseAccumulator *= 0.85f;
		xMouseAccumulator += Input.GetAxis("Mouse X") * mouseSensivityX;
		
		transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensivityX, 0);
		
		
		mainCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * mouseSensivityX, 0,  0);
		
		if (Input.GetKeyUp(KeyCode.E)){
			hoverMode = !hoverMode;
			if (hoverMode) GetSound(soundAnnounceHover).Play();
			else GetSound(soundAnnounceOrbit).Play();
			
		}
		
		if (Input.GetKeyUp(KeyCode.F)){
			Renderer[] renderers = mapToggle.GetComponentsInChildren<Renderer>();
			foreach (Renderer childRenderer in renderers){
				childRenderer.enabled = !childRenderer.enabled;
			}			
		}
		
		velocityHUD.text = "VEL: " + (rigidbody.velocity.magnitude * 2).ToString("F0") + "m/s";
		verticalVelocityHUD.text = "vVEL: " + (vVelocity * 2).ToString("F0") + "m/s";
		altitudeHUD.text = "ALT: " + (rigidbody.position.magnitude * 2 - 336).ToString("F0") + "m";
		hoverHUD.text = hoverMode ? "HOVER" : "ORBIT";
		hoverHUD.color = hoverMode ? new Color(0, 1.0f, 0, 1.0f) :  new Color(1.0f, 0, 0, 1.0f);
		
		float gravityForce = GRAVITATIONAL_CONSTANT * planetMass / transform.position.magnitude;
		debugOutput.queue("planet mass  : " + planetMass);
		debugOutput.queue("gravity force: " + gravityForce);
		
		
		
		
	}
	
	void FixedUpdate(){
		
		float vPosition = rigidbody.position.magnitude;
		vVelocity = (vPosition - lastVPosition) * (1/ Time.deltaTime);
		lastVPosition = vPosition;
		
		float gravityForce = GravityForce();
		
		Vector3 gravity = (new Vector3(0,0,0) - aligner.rigidbody.position.normalized) * gravityForce;
		aligner.rigidbody.AddForce(gravity);
		
		
		
		cockpitModule.transform.localEulerAngles = new Vector3(
			0, 
			-xMouseAccumulator, 
			0);
		
		
		// forward and reverse
		bool zInput = false;
		if (Input.GetKey(KeyCode.Space)){
			zAcceleration += thrustAcceleration;
			if (zAcceleration > thrustPower) zAcceleration = thrustPower;
			GetSound(soundForward).mute = false;
			zInput = true;
		}else{
			GetSound(soundForward).mute = true;
		}
		if (Input.GetKey(KeyCode.LeftAlt)){
			zAcceleration -= thrustAcceleration;
			if (zAcceleration < -thrustPower) zAcceleration = -thrustPower;
			GetSound(soundReverse).mute = false;
			zInput = true;
		}else{
			GetSound(soundReverse).mute = true;	
		}
		if (!zInput){
			if (Math.Abs(zAcceleration) < thrustAcceleration) zAcceleration = 0;
			else zAcceleration += (zAcceleration < 0) ? thrustAcceleration : -thrustAcceleration;
		}
		rigidbody.AddRelativeForce(0, 0, zAcceleration);		
		
		// left and right
		bool xInput = false;
		if (Input.GetKey(KeyCode.D)){
			xAcceleration += thrustAcceleration;
			if (xAcceleration >  thrustPower) xAcceleration = thrustPower;
			GetSound(soundRight).mute = false;
			xInput = true;
		}else{
			GetSound(soundRight).mute = true;
		}
		
		if (Input.GetKey(KeyCode.A)){
			xAcceleration -= thrustAcceleration;
			if (xAcceleration <  -thrustPower) xAcceleration = -thrustPower;
			GetSound(soundLeft).mute = false;
			xInput = true;
		}else{
			GetSound(soundLeft).mute = true;	
		}
		if (!xInput){
			if (Math.Abs(xAcceleration) < thrustAcceleration) xAcceleration = 0;
			else xAcceleration += (xAcceleration < 0) ? thrustAcceleration : -thrustAcceleration;
		}
		rigidbody.AddRelativeForce(xAcceleration, 0, 0);
			
		
		// up and down
		bool yInput = false;
		float maxDownThrust = hoverMode ? (gravityForce - hoverVerticalThrustDelta) : -thrustPower;
		float maxUpThrust = hoverMode ? (gravityForce + hoverVerticalThrustDelta) : thrustPower;
		if (maxUpThrust > thrustPower) maxUpThrust = thrustPower;
		
		float yThrustCenter = hoverMode ? gravityForce : 0;
		if (Input.GetKey(KeyCode.W)){
			yAcceleration += thrustAcceleration;
			if (yAcceleration >  maxUpThrust) yAcceleration = maxUpThrust;
			GetSound(soundUp).mute = false;
			yInput = true;
		}else{
			GetSound(soundUp).mute = true;
		}	
		
		if (Input.GetKey(KeyCode.S)){
			yAcceleration -= thrustAcceleration;
			if (yAcceleration <  maxDownThrust) yAcceleration = maxDownThrust;
			GetSound(soundDown).mute = false;
			yInput = true;
		}else{
			GetSound(soundDown).mute = true;
		}
		
		if (!yInput){
			if (Math.Abs(yAcceleration - yThrustCenter) < thrustAcceleration) yAcceleration = yThrustCenter;
			else yAcceleration += (yAcceleration < yThrustCenter) ? thrustAcceleration : -thrustAcceleration;
		}

		rigidbody.AddRelativeForce(0, yAcceleration, 0);
		
		GetSound(soundHover).mute = !hoverMode;
		
	}
	
	protected float GravityForce(){
		return GRAVITATIONAL_CONSTANT * planetMass * rigidbody.mass / transform.position.magnitude;
	}
	
	protected AudioSource GetSound(GameObject myGameObject){
		return myGameObject.GetComponent<AudioSource>();		
	}
}