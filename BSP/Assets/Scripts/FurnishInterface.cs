using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FurnishInterface {

	void placeItems (Rect room);
	void textureWalls (Rect room);
	void textureFloor (Rect room);
	void textureCeiling (Rect room);

}
