using UnityEngine;
using System.Collections;

public class HologramMap : MonoBehaviour {
	
	public GameObject reference;
	public GameObject parent;
	public DebugOutput debug;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//Vector3 position = reference.transform.position.normalized;
		
		
		transform.eulerAngles = new Vector3(0,0,0);
		
		
		
		//transform.Rotate(new Vector3(0, 0, 45.0f));
		//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 45.0f);
		//parent.transform.localEulerAngles = new Vector3(45.0f, 0, 0);
		
	}
	
	void FixedUpdate(){
		
		debug.queue(transform.localEulerAngles.ToString());
		
	}
}
