using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampOnDesk : FurnishInterface {
	public GameObject Desk, Chair;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void placeItems(Rect room){
		Debug.Log ("LampOnDesk lett ide");
	}
	public void textureWalls(Rect room){
		Debug.Log ("asd");
	}
	public void textureFloor(Rect room){
		Debug.Log ("asd");
	}
	public void textureCeiling(Rect room){
		Debug.Log ("asd");
	}

}
