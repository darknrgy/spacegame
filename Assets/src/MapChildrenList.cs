using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MapChildrenList {

	private static MapChildrenList instance;
	
	protected List<GameObject> list;

	private MapChildrenList() {
		list = new List<GameObject>();
	}
	
	public static MapChildrenList Instance{
		get{
			if (instance == null){
				instance = new MapChildrenList();
			}
			return instance;
		}
	}
	
	public void RegisterMapObject(GameObject gameObject){
		
		//Mesh mesh = shipTemplate.GetComponent<MeshFilter>().mesh;
		list.Add(gameObject);	
		Debug.Log("Adding Object, count is now: " + list.Count);
	}
	
	public List<GameObject> getAllObjects(){
		return list;
	}
}
