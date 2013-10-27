using UnityEngine;
using System.Collections;

public class Lander : MonoBehaviour {
	
	void OnTriggerEnter(Collider other){
		Debug.Log("I collided? ??? wtf");
		PlayerSpacecraft ship = (PlayerSpacecraft) FindObjectOfType(typeof(PlayerSpacecraft));
		ship.Land();
	}
	
	void OnTriggerExit(Collider other){
		Debug.Log("I Exited the collider");
		PlayerSpacecraft ship = (PlayerSpacecraft) FindObjectOfType(typeof(PlayerSpacecraft));
		ship.landed = false;
	}
	
	
}
