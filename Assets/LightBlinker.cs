using UnityEngine;
using System.Collections;

public class LightBlinker : MonoBehaviour {

	public DebugOutput debugOutput;
	public GameObject blinkerObject;
	
	protected float lastTime;
	protected bool state = true;
	
	// Update is called once per frame
	void Update () {
		
		float timeNow = Time.fixedTime;
		if (state){
			if (timeNow > lastTime + 0.25f) ToggleLight();
		}else{
			if (timeNow > lastTime + 1.0f) ToggleLight();
		}
	}
	
	void ToggleLight(){
		state = !state;
		blinkerObject.SetActive(state);
		
		/*Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer childRenderer in renderers){
			childRenderer.enabled = state;
		}*/
		lastTime = Time.fixedTime;
	}
}
