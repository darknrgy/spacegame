using UnityEngine;
using System.Collections;

public class MapChild : MonoBehaviour {
	
	void Start () {
		
		MapChildrenList mcl = MapChildrenList.Instance;
		mcl.RegisterMapObject(gameObject);
	}
}
