using UnityEngine;
using System.Collections;
using System;

public class PlayerSpacecraft : Body {

	public float thrustPower = 7.0f;
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
	
	public GameObject cockpitModule;
	
	public Component mainCamera;
	
	protected bool hoverMode = true;
	protected float xAcceleration = 0;
	protected float yAcceleration = 0;
	protected float zAcceleration = 0;
	
	protected float xMouseAccumulator = 0;
	protected float lastVPosition = 0;
	protected float vVelocity = 0;
	
	void Start () {
		Initialize ();
		if (hoverMode) ApplyForce(Y_AXIS, GravityForce(), true);
	}
	
	// Update is called once per frame
	void Update () {
		
		HandleMouseInput();
		DrawHud();		
		if (Input.GetKeyUp(KeyCode.E)) ToggleHoverMode();		
		if (Input.GetKeyUp(KeyCode.F)) ToggleMap();
	}
	
	void AlignMap(){
		cockpitModule.transform.localEulerAngles = new Vector3(
			0, -xMouseAccumulator, 0);		
	}
	
	void FixedUpdate(){
		
		float gravityForce = ApplyGravity();
		CalculateVerticalVelocity();
		
		
		// Forward and Reverse
		if (Input.GetKey(KeyCode.Space)){
			ApplyForce(Z_AXIS, thrustPower);	
			GetSound(soundForward).mute = false;
		}		
		if (Input.GetKey(KeyCode.LeftAlt)){
			ApplyForce(Z_AXIS, -thrustPower);
			GetSound(soundReverse).mute = false;
		}		
		if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.Space)){
			ApplyForce(Z_AXIS, 0);
			GetSound(soundReverse).mute = true;
			GetSound(soundForward).mute = true;
		}	
		
		// Left and Right
		GetSound(soundHover).mute = !hoverMode;
		if (Input.GetKey(KeyCode.A)){
			ApplyForce(X_AXIS, -thrustPower);	
			GetSound(soundLeft).mute = false;
		}		
		if (Input.GetKey(KeyCode.D)){
			ApplyForce(X_AXIS, thrustPower);
			GetSound(soundRight).mute = false;
		}		
		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
			ApplyForce(X_AXIS, 0);
			GetSound(soundLeft).mute = true;
			GetSound(soundRight).mute = true;
		}
		
		// Up and Down
		float up, down, center = 0;
		if (hoverMode){
			up = gravityForce + hoverVerticalThrustDelta;
			down = gravityForce - hoverVerticalThrustDelta;
			center = gravityForce;
		}else{
			up = gravityForce;
			down = -gravityForce;
			center = 0;
		}
		if (Input.GetKey(KeyCode.W)){
			ApplyForce(Y_AXIS, up);	
			GetSound(soundUp).mute = false;
		}		
		if (Input.GetKey(KeyCode.S)){
			ApplyForce(Y_AXIS, down);
			GetSound(soundDown).mute = false;
		}		
		if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)){
			ApplyForce(Y_AXIS, center);
			GetSound(soundUp).mute = true;
			GetSound(soundDown).mute = true;
		}			
		
	}
	
	void HandleMouseInput(){
		xMouseAccumulator *= 0.85f;
		xMouseAccumulator += Input.GetAxis("Mouse X") * (float) Prefs.get("mouseSensivityX");
		transform.Rotate(0, Input.GetAxis("Mouse X") * (float) Prefs.get("mouseSensivityX"), 0);		
		mainCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * (float) Prefs.get("mouseSensivityY"), 0,  0);		
	}
	
	void DrawHud(){
		velocityHUD.text = "VEL: " + (rigidbody.velocity.magnitude * 2).ToString("F0") + "m/s";
		verticalVelocityHUD.text = "vVEL: " + (vVelocity * 2).ToString("F0") + "m/s";
		altitudeHUD.text = "ALT: " + (rigidbody.position.magnitude * 2 - 336).ToString("F0") + "m";
		hoverHUD.text = hoverMode ? "HOVER" : "ORBIT";
		hoverHUD.color = hoverMode ? new Color(0, 1.0f, 0, 1.0f) :  new Color(1.0f, 0, 0, 1.0f);
	}
	
	void ToggleHoverMode(){
		hoverMode = !hoverMode;
		if (hoverMode) GetSound(soundAnnounceHover).Play();
		else GetSound(soundAnnounceOrbit).Play();
	}
	
	void ToggleMap(){
		Renderer[] renderers = mapToggle.GetComponentsInChildren<Renderer>();
		foreach (Renderer childRenderer in renderers){
			childRenderer.enabled = !childRenderer.enabled;
		}	
	}
	
	void CalculateVerticalVelocity(){
		float vPosition = rigidbody.position.magnitude;
		vVelocity = (vPosition - lastVPosition) * (1/ Time.deltaTime);
		lastVPosition = vPosition;
	}
	
	protected AudioSource GetSound(GameObject myGameObject){
		return myGameObject.GetComponent<AudioSource>();		
	}

}
