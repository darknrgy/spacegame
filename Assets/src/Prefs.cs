using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prefs {
	
	protected Dictionary<string,object> userPreferences = new Dictionary<string, object>();
	protected static Prefs instance = null;
	
	public static object get(string key){
		return Instance.userPreferences[key];
	}
	
	public static Prefs Instance{
		get{
			if (instance == null) instance = new Prefs();
			return instance;
		}
	}
	
	// defaults
	public Prefs () {
		userPreferences.Add("mouseSensivityX", 1.0f);
		userPreferences.Add("mouseSensivityY", 1.0f);
		
		
	}
}
