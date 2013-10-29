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
	
	protected GameObject velocityVector;
	protected GameObject forwardVector;
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
		if (stationKeeping){
			velocityVector = new GameObject("Velocity Vector");
			velocityVector.transform.parent = transform;
			velocityVector.transform.position = transform.position;
			forwardVector = new GameObject("Forward Vector");
			forwardVector.transform.parent = transform;
			forwardVector.transform.localPosition = initialVelocity.normalized;			
		}
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
	
	// Station Keeping
	protected float yRotation = 0;
	protected void StationKeeping(){	
		
		float yawAngle = AlignYaw();
		if (yawAngle > 3) return;
		float newAlt = transform.position.magnitude;
		float altDifference = stationKeepingAlt - newAlt;		
		stationKeepingThrust = altDifference * 0.05f;
		float actualThrust = stationKeepingThrust * rigidbody.mass;
		ApplyForce(Y_AXIS, actualThrust, true);
		if (newAlt < currentAlt && newAlt < stationKeepingAlt){
			ApplyForce(X_AXIS, actualThrust, true);			
		}		
		currentAlt = newAlt;
	}
	
	protected float AlignYaw(){
		velocityVector.transform.position = transform.position + rigidbody.velocity.normalized;		
		float angle = Vector3.Angle(new Vector3(1,0,0), velocityVector.transform.localPosition);
		float zDelta = velocityVector.transform.localPosition.z;
		rigidbody.transform.Rotate(0, angle * 0.01f * (zDelta > 0 ? -1 : 1),0);	
		return angle;
	}
}
