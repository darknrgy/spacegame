using UnityEngine;
using System.Collections;

public class DebugOutput : MonoBehaviour {
	
	public string echo;
	protected string queueString;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void queue(string s){
		queueString += s + "\n";		
	}
	
	public void flush(){
		guiText.text = queueString;
		queueString = "";
	}
	
	void FixedUpdate(){
		flush();	
	}
}
