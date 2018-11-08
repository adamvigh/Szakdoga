using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairBesideDesk : FurnishInterface {
	GameObject lamp=Resources.Load<GameObject>("Prefabs/Old_USSR_Lamp_01");
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public void placeItems(Rect room){
		GameObject insncedObj=GameObject.Instantiate(lamp,new Vector3((room.x+room.xMax)/2,0.5f,(room.y+room.yMax)/2),Quaternion.Euler(new Vector3(0,-90,0)));
		Debug.Log ("ChairBesideDesk lett ide");
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
