using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour{
	//public enum Script{ Konyha, Nappali}
	public int mapRows, mapColumns;
	public GameObject floorTile;
	public GameObject corridorTile;
	public GameObject wallTile;
	public List<RoomTemplate> roomTemplates;

	public GameObject[,] boardPositionsFloor;
	public GameObject[,] boardPositionsWall;
	public GameObject[,] boardPositionsCorridor;
	void Awake(){
		
		boardPositionsFloor = new GameObject[mapRows,mapColumns];
		boardPositionsWall = new GameObject[mapRows,mapColumns];
		boardPositionsCorridor = new GameObject[mapRows,mapColumns];
		Debug.Log ("Mapbuilder awake() run");
	
	}


	// Szoba rajzolása
	public void DrawRooms(SubDungeon subDungeon) {
		
		if (subDungeon == null) {
			return;
		}
		// ha a rész levél, akkor végigmegy rajza és lerak egységenként egy floorTilet
		if (subDungeon.IAmLeaf()) {
			int typeOfRoom;
			typeOfRoom = Random.Range (0, 2);
			FurnishInterface furnisher;
			switch (typeOfRoom) {
			case 0:
				furnisher = new LampOnDesk ();
				break;
			case 1:
				furnisher = new ChairBesideDesk ();
				break;
			default:
				furnisher = new LampOnDesk ();
				break;
			}
			furnisher.placeItems (subDungeon.room);
			GameObject lamp = roomTemplates [Random.Range (0, 2)].lamp;
			Texture floorTexture = roomTemplates[Random.Range(0,3)].floorTexture;
			for (int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++) {
				for (int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++) {
					
					//GameObject instance = Instantiate (floorTile, new Vector3 (i, j, 0f), Quaternion.Identity) as GameObject;
					GameObject instance = Instantiate (floorTile, new Vector3 (i, 0f, j), Quaternion.Euler(90,0,0)) as GameObject;
					instance.GetComponent<MeshRenderer> ().material.mainTexture = floorTexture;
					instance.transform.SetParent (transform);
				    boardPositionsFloor [i, j] = instance;
				}
			}
			DrawWallsAround (subDungeon);
			//Instantiate(lamp,new Vector3((subDungeon.room.x+subDungeon.room.xMax)/2,0f,(subDungeon.room.y+subDungeon.room.yMax)/2),Quaternion.Euler(new Vector3(0,-90,0)));
		} else {		//ha nem levél, akkor halad tovább a gyerekeire
			DrawRooms (subDungeon.left);
			DrawRooms (subDungeon.right);
		}
	}

	public void DrawWallsAround(SubDungeon subDungeon){
		Material wallMaterial = roomTemplates[Random.Range(0,3)].wallTexture;
		for(int i=(int)subDungeon.room.xMin;i<=(int)subDungeon.room.xMax-1;i++){
			//GameObject instance = Instantiate(wallTile,new Vector3(i,subDungeon.room.yMin-1,-0.5f),Quaternion.identity) as GameObject;
			GameObject instance = Instantiate(wallTile,new Vector3(i,0.5f,subDungeon.room.yMin-1),Quaternion.Euler(90,0,0)) as GameObject;
			instance.transform.SetParent (transform);
			instance.GetComponent<MeshRenderer> ().material = wallMaterial;
			boardPositionsWall [i,(int)subDungeon.room.yMin-1] = instance;
		}
		for(int i=(int)subDungeon.room.xMin;i<=(int)subDungeon.room.xMax-1;i++){
			GameObject instance = Instantiate(wallTile,new Vector3(i,0.5f,subDungeon.room.yMax),Quaternion.Euler(90,0,0)) as GameObject;
			instance.transform.SetParent (transform);
			instance.GetComponent<MeshRenderer> ().material = wallMaterial;
			boardPositionsWall [i,(int)subDungeon.room.yMax] = instance;
		}
		for(int i=(int)subDungeon.room.yMin;i<=(int)subDungeon.room.yMax-1;i++){
			GameObject instance = Instantiate(wallTile,new Vector3(subDungeon.room.xMax,0.5f,i),Quaternion.Euler(90,0,0)) as GameObject;//new Vector3((int)subDungeon.room.xMax-0.25f,i-0.25f,-0.25f)
			instance.transform.SetParent (transform);
			instance.GetComponent<MeshRenderer> ().material = wallMaterial;
			boardPositionsWall [(int)subDungeon.room.xMax,i] = instance;
		}
		for(int i=(int)subDungeon.room.yMin;i<=(int)subDungeon.room.yMax-1;i++){
			GameObject instance = Instantiate(wallTile,new Vector3(subDungeon.room.xMin-1,0.5f,i),Quaternion.Euler(90,0,0)) as GameObject;//new Vector3(subDungeon.room.xMin-0.75f,i-0.25f,-0.25f)
			instance.transform.SetParent (transform);
			instance.GetComponent<MeshRenderer> ().material = wallMaterial;
			boardPositionsWall [(int)subDungeon.room.xMin-1,i] = instance;
		}
	}

	//folyosó rajzolása
	public void DrawCorridors(SubDungeon subDungeon) {
		if (subDungeon == null) {
			return;
		}

		DrawCorridors (subDungeon.left);
		DrawCorridors (subDungeon.right);

		foreach (Rect corridor in subDungeon.corridors) {
			for (int i = (int)corridor.x; i < corridor.xMax; i++) {
				for (int j = (int)corridor.y; j < corridor.yMax; j++) {
					if (boardPositionsFloor[i,j] == null) {
						/*GameObject instance = Instantiate (corridorTile, new Vector3 (i, 0f, j), Quaternion.Euler(90,0,0)) as GameObject;
						instance.transform.SetParent (transform);
						boardPositionsCorridor [i, j] = instance;*/
						GameObject instance = Instantiate (floorTile, new Vector3 (i, 0f, j), Quaternion.Euler(90,0,0)) as GameObject;
						//instance.GetComponent<MeshRenderer> ().material.mainTexture = floorTexture;
						instance.transform.SetParent (transform);
						boardPositionsCorridor [i, j] = instance;
					}
				}
			}
		}
	}

	public void DestroyWalls(){
		for(int i=0;i<mapRows;i++){
			for (int j = 0; j < mapColumns; j++) {
				if (boardPositionsCorridor [i, j] != null && boardPositionsWall [i, j] != null) {
					Destroy (boardPositionsWall [i, j]);
				}
			}
		}
	}

	public void PutWallsAroundCorridors(){
		for(int i=0;i<mapRows;i++){
			for (int j = 0; j < mapColumns; j++) {
				if (boardPositionsCorridor[i,j]!=null) {
					for (int n = i - 1; n < i + 2; n++) {
						for (int m = j - 1; m < j + 2; m++) {
							if (Mathf.Abs ((i-n)+(j-m)) == 1 && boardPositionsCorridor [n, m] == null && boardPositionsFloor [n, m] == null && boardPositionsWall [n, m] == null) {
								GameObject instance = Instantiate(wallTile,new Vector3(n,0.5f,m),Quaternion.Euler(90,0,0)) as GameObject;
								instance.transform.SetParent (transform);
								boardPositionsWall [n,m] = instance;
							}
						}
					}
				}
			}
		}

	}
	[System.Serializable]
	public class RoomTemplate{
		public Material wallTexture;
		public Texture floorTexture;
		public GameObject lamp;
	}

}
