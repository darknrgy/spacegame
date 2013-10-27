using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Body : MonoBehaviour {
	
	public DebugOutput debugOutput;
	public GameObject aligner;
	protected Dictionary<int, float> thrust = new Dictionary<int, float>();
	public float thrustAcceleration = 0.1f;
	protected const float GRAVITATIONAL_CONSTANT = 1;
	protected float planetMass;
	
	protected static int X_AXIS = 1;
	protected static int Y_AXIS = 2;
	protected static int Z_AXIS = 3;
	
	protected Dictionary<int, Vector3> vectorMap = new Dictionary<int, Vector3>();
	
	public Body(){
		
		vectorMap.Add(X_AXIS, new Vector3(1,0,0));
		vectorMap.Add(Y_AXIS, new Vector3(0,1,0));
		vectorMap.Add(Z_AXIS, new Vector3(0,0,1));
		thrust[X_AXIS] = 0;
		thrust[Y_AXIS] = 0;
		thrust[Z_AXIS] = 0;		
		planetMass = 1100 / GRAVITATIONAL_CONSTANT;		
	}
	
	protected void Initialize() {
		rigidbody.centerOfMass = new Vector3(0, 0, 0);
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
		return GRAVITATIONAL_CONSTANT * planetMass * rigidbody.mass / transform.position.magnitude;
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
}
