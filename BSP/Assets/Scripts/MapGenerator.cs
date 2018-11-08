using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour{
	
	public int minRoomSize, maxRoomSize;

	public void CreateBSP(SubDungeon subDungeon) {
		if (subDungeon.IAmLeaf()) {       // ha a rész levél
			//ha a rész magassága, vagy szélessége nagyobb mint a maximum megengedett
			if (subDungeon.rect.width > maxRoomSize || subDungeon.rect.height > maxRoomSize || Random.Range(0.0f,1.0f) > 0.25) {    
				//szétválasztható
				if (subDungeon.Split (minRoomSize, maxRoomSize)) {
					//szétválasztani és meghívni a gyerekeire is
					CreateBSP(subDungeon.left);
					CreateBSP(subDungeon.right);
				}
			}
		}
	}
}
