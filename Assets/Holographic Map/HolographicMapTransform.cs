using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HolographicMapTransform : MonoBehaviour {
	
	public GameObject reference;
	public GameObject parent;
	public DebugOutput debug;
	public GameObject satellite;
	public GameObject shipTemplate;
	
	protected float counter = 0;
	protected List<GameObject> children = new List<GameObject>();
	
	protected static HolographicMapTransform instance;
	
	
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
		
		parent.transform.Rotate(-47.0f,0,0);
		
		foreach (GameObject child in children){
			child.transform.localPosition = satellite.transform.position * 0.001f;
			child.transform.localEulerAngles = satellite.transform.eulerAngles;
		}
		
		
		
		
	}
	
	void FixedUpdate(){
		

		

		
	}
}
