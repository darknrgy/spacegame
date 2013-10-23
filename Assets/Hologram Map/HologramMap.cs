using UnityEngine;
using System.Collections;

public class HologramMap : MonoBehaviour {
	
	public GameObject reference;
	public GameObject parent;
	public DebugOutput debug;
	
	protected float counter = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		counter += 0.1f;
		
		//transform.eulerAngles = new Vector3(0,0,0);
		transform.eulerAngles = new Vector3(
			0,
			0 ,
			0);
		
		parent.transform.Rotate(-45.0f,0,0);
		
		/*transform.localEulerAngles = new Vector3(
			transform.localEulerAngles.x -45,
			0 ,
			0);*/
		
		
		/*
		transform.localEulerAngles = new Vector3(
			parent.transform.eulerAngles.x, 
			parent.transform.eulerAngles.y, 
			0);
			
			*/
			
		
		

		
		//Vector3 position = reference.transform.position.normalized;
		
		
		
		
		
		//transform.Rotate(new Vector3(0, 0, 45.0f));
		//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 45.0f);
		//parent.transform.localEulerAngles = new Vector3(45.0f, 0, 0);
		debug.queue("local:" + transform.localEulerAngles.ToString());
		debug.queue("global:" + transform.eulerAngles.ToString());
		debug.queue("parent.local:" + parent.transform.localEulerAngles.ToString());
		debug.queue("parent.global:" + parent.transform.eulerAngles.ToString());
		
	}
	
	void FixedUpdate(){
		

		

		
	}
}
