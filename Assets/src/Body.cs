using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Body : MonoBehaviour {
	
	public DebugOutput debugOutput;
	public GameObject aligner;
	public float thrustAcceleration = 0.1f;
	public Vector3 initialVelocity = new Vector3(0,0,0);
	public bool stationKeeping = false;
	
	protected Dictionary<int, float> thrust = new Dictionary<int, float>();
	protected float stationKeepingAlt;
	protected const float GRAVITATIONAL_CONSTANT = 1100;
	protected float planetMass;
	protected static int X_AXIS = 1;
	protected static int Y_AXIS = 2;
	protected static int Z_AXIS = 3;	
	
	protected float stationKeepingThrust;
	protected float currentAlt;
	
	protected Dictionary<int, Vector3> vectorMap = new Dictionary<int, Vector3>();
	
	public Body(){
		
		vectorMap.Add(X_AXIS, new Vector3(1,0,0));
		vectorMap.Add(Y_AXIS, new Vector3(0,1,0));
		vectorMap.Add(Z_AXIS, new Vector3(0,0,1));
		thrust[X_AXIS] = 0;
		thrust[Y_AXIS] = 0;
		thrust[Z_AXIS] = 0;		
	}
	
	public void Start() {
		rigidbody.centerOfMass = new Vector3(0, 0, 0);
		rigidbody.velocity = initialVelocity;
		currentAlt = stationKeepingAlt = transform.position.magnitude;
	}
	
	void Update(){
		debugOutput.queue("target  : " + stationKeepingAlt);
		debugOutput.queue("altitude: " + transform.position.magnitude);
		debugOutput.queue("thrust:   " + stationKeepingThrust);
		debugOutput.queue("velocity: " + rigidbody.velocity.magnitude);
		
		
	}
	
	public void FixedUpdate(){
		ApplyGravity();
		if (stationKeeping) StationKeeping();		
	}
	
	
	protected void ApplyForce(int axis, float v){
		ApplyForce(axis, v, false);
	}
	protected void ApplyForce(int axis, float v, bool ignoreAcceleration){
		if (ignoreAcceleration) thrust[axis] = v;
		else thrust[axis] = GoTowards(thrust[axis], v, thrustAcceleration);
		rigidbody.AddRelativeForce(vectorMap[axis] * thrust[axis]);		
	}
	
	protected float ApplyGravity(){
		float gravityForce = GravityForce();
		Vector3 gravity = (new Vector3(0,0,0) - aligner.rigidbody.position.normalized) * gravityForce;
		aligner.rigidbody.AddForce(gravity);
		return gravityForce;
	}
	
	protected float GravityForce(){
		return GRAVITATIONAL_CONSTANT * rigidbody.mass / transform.position.magnitude;
	}
	
	private float GoTowards(float previous, float next, float rate){
		
		if (next == previous) return next;
		
		if (next > previous){
			previous += rate;
			if (previous > next) previous = next;
		}else{
			previous -= rate;
			if (previous < next) previous = next;
		}
		return previous;
	}
	protected float yRotation = 0;
	
	protected void StationKeeping2(){
		
		float newAlt = transform.position.magnitude;
		float altDifference = stationKeepingAlt - newAlt;
		
		float thrust = 0.01f * rigidbody.mass;
		
		if (newAlt < currentAlt && newAlt < stationKeepingAlt){
			ApplyForce(X_AXIS, thrust, true);
			Debug.Log("Increasing...");
		}
		
		if (newAlt > currentAlt && newAlt > stationKeepingAlt){
			ApplyForce(X_AXIS, -thrust/2, true);			
			Debug.Log("Decreasing...");
		}
		
		currentAlt = newAlt;
		
	}
	
	
	
	protected void StationKeeping(){
		
		float newAlt = transform.position.magnitude;
		float altDifference = stationKeepingAlt - newAlt;
		
		stationKeepingThrust = altDifference * 0.1f;
		float actualThrust = stationKeepingThrust * rigidbody.mass;
		
		
		ApplyForce(Y_AXIS, actualThrust, true);
		if (newAlt < currentAlt && newAlt < stationKeepingAlt){
			ApplyForce(X_AXIS, actualThrust, true);
			Debug.Log("applying z force: " + -actualThrust);
		}
		
		currentAlt = newAlt;
		
	}
}
