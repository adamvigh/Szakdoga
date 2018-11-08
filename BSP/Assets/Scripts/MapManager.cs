using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
	
	public GameObject player;


	private Vector3 offset;
	void Start () {
		//inicializáljuk a teljes kezdőterületet és megkezdjük a bsp kialakítását
		SubDungeon rootSubDungeon = new SubDungeon (new Rect (0, 0, GetComponent<MapBuilder>().mapRows, GetComponent<MapBuilder>().mapColumns));
		GetComponent<MapGenerator>().CreateBSP(rootSubDungeon);
		//szobák kialakítása
		rootSubDungeon.CreateRoom ();

		//szobák rajzolása
		GetComponent<MapBuilder>().DrawRooms(rootSubDungeon);
		GetComponent<MapBuilder>().DrawCorridors(rootSubDungeon);
		GetComponent<MapBuilder>().DestroyWalls ();
		GetComponent<MapBuilder>().PutWallsAroundCorridors ();
		SpawnPlayer ();
	}
		
	public void SpawnPlayer(){
		for (int i = 0; i < GetComponent<MapBuilder>().mapRows; i++) {
			for (int j = 0; j < GetComponent<MapBuilder>().mapColumns; j++) {
				if (GetComponent<MapBuilder>().boardPositionsFloor [j, i] != null) {
					Instantiate (player, new Vector3 (j, i, -0.5f), Quaternion.identity);
					return;
				}
			}
		}
	}



}
