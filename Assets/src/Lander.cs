using UnityEngine;
using System.Collections;

public class Lander : MonoBehaviour {
	
	void OnTriggerEnter(Collider other){
		PlayerSpacecraft ship = GetSpacecraft();
		ship.Land();
	}
	
	void OnTriggerExit(Collider other){
		PlayerSpacecraft ship = GetSpacecraft();
		ship.TakeOff();
	}
	
	protected PlayerSpacecraft GetSpacecraft(){
		return  (PlayerSpacecraft) FindObjectOfType(typeof(PlayerSpacecraft));
	}
	
	
}
