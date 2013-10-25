using UnityEngine;
using System;
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
	protected List<Dictionary<string,object>> localChildren = new List<Dictionary<string,object>>();
	
	protected static HolographicMapTransform instance;
	protected Int32 trailCounter = 0;
	protected Int32 trailPositionCounter = 0;
	
	protected const UInt16 TRAIL_LENGTH = 100;
	protected const UInt16 TRAIL_INTERVAL = 20;
	
	
	
	void Start () {

		Dictionary<string,object> dict;
		GameObject child;
		foreach(Transform actor in actors.transform){
			GameObject trail = GetTrailObject();
			Vector3[] trailPositions = new Vector3[TRAIL_LENGTH];
			dict = new Dictionary<string, object>();
			child = (GameObject) Instantiate(shipTemplate, actor.position * 0.001f, actor.rotation);
			InitializeTrail(trailPositions, child.transform.position);
			child.transform.parent = transform;
			trail.transform.parent = transform;
			dict.Add("parent", actor.gameObject);
			dict.Add("child", child);
			dict.Add("trail", trail);
			dict.Add("trail_positions", trailPositions);
			localChildren.Add(dict);
		}
	}
	
	protected GameObject GetTrailObject(){
		GameObject trail = new GameObject();		
		LineRenderer lineRenderer = trail.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(Color.yellow, Color.yellow);
        lineRenderer.SetWidth(0.002F, 0.002F);
        lineRenderer.SetVertexCount(TRAIL_LENGTH);
		lineRenderer.useWorldSpace = false;
		lineRenderer.transform.parent = trail.transform;
		trail.transform.localPosition = new Vector3(0,0,0);
		trail.transform.parent = transform;
		return trail;
	}
	
	protected void ResetLine(GameObject lineObject, Vector3 position){
		LineRenderer line = GetLineFromObject(lineObject);
		for (UInt16 i = 0; i <= TRAIL_LENGTH-1; i++){
			line.SetPosition(i, position);
		}
	}
	
	protected void InitializeTrail(Vector3[] trail, Vector3 position){
		
		for (UInt16 i = 0; i <= TRAIL_LENGTH - 1; i++){
			trail[i] = position;	
		}
		
	}
	
	protected void Trail(GameObject trail, Vector3 position, ref Vector3[] positions){
		Array.Copy(positions, 1, positions, 0, TRAIL_LENGTH - 1);
		positions[TRAIL_LENGTH-1] = position;
		
		LineRenderer line = GetLineFromObject(trail);
		for (UInt16 i = 0; i <= TRAIL_LENGTH -1; i++){
			line.SetPosition(i, positions[i]);
		}
	}
	
	protected LineRenderer GetLineFromObject(GameObject lineObject){
		return lineObject.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		counter += 0.1f;
		trailCounter ++;
		if (trailCounter >= TRAIL_INTERVAL){
			trailCounter = 0;
			trailPositionCounter ++;
			if (trailPositionCounter > TRAIL_LENGTH) trailPositionCounter = 0;
		}
		
		//transform.eulerAngles = new Vector3(0,0,0);
		transform.eulerAngles = new Vector3(
			0,
			0 ,
			0);
		
		foreach (Dictionary<string,object> child in localChildren){
			GameObject childObject = (GameObject) child["child"];
			GameObject parentObject = (GameObject) child["parent"];
			Vector3[] trailPositions = (Vector3[]) child["trail_positions"];
			childObject.transform.localPosition = parentObject.transform.position * 0.00085f;
			childObject.transform.localRotation = parentObject.transform.rotation;
			
			if (trailCounter == 0){
				GameObject trail = (GameObject) child["trail"];
				trail.transform.localPosition = new Vector3(0,0,0);
				Trail(trail, childObject.transform.localPosition, ref trailPositions);
			}
		}
				
		
		
		
	}
	
	void FixedUpdate(){
		
		
		

		
	}
}
