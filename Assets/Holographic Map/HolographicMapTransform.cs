using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HolographicMapTransform : MonoBehaviour {
	
	public GameObject reference;
	public GameObject parent;
	public DebugOutput debug;
	public GameObject satellite;
	public GameObject shipTemplate;
	public GameObject actors;
	
	protected float counter = 0;
	protected List<GameObject> children = new List<GameObject>();
	protected List<Dictionary<string,GameObject>> localChildren = new List<Dictionary<string,GameObject>>();
	
	protected static HolographicMapTransform instance;
	
	
	void Start () {

		Dictionary<string,GameObject> dict;
		GameObject child;
		foreach(Transform actor in actors.transform){
			dict = new Dictionary<string, GameObject>();
			child = (GameObject) Instantiate(shipTemplate, actor.position * 0.001f, actor.rotation);
			child.transform.parent = transform;
			dict.Add("parent", actor.gameObject);
			dict.Add("child", child);
			localChildren.Add(dict);
			Debug.Log("adding child...");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		counter += 0.1f;
		
		//transform.eulerAngles = new Vector3(0,0,0);
		transform.eulerAngles = new Vector3(
			0,
			0 ,
			0);
		
		//parent.transform.Rotate(-47.0f,0,0);
		parent.transform.Rotate(-90.0f,0,0);
		
		
		foreach (Dictionary<string,GameObject> child in localChildren){
			child["child"].transform.localPosition = child["parent"].transform.position * 0.00085f;
			child["child"].transform.localRotation = child["parent"].transform.rotation;
			child["child"].transform.parent = transform;
			
			debug.queue(child["child"].transform.position.ToString());
		}
				
		
		
		
	}
	
	void FixedUpdate(){
		

		

		
	}
}
