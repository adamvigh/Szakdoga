using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubDungeon{
	public SubDungeon left, right; //jobb és bal fiú
	public Rect rect; // az egész pályaterület
	public Rect room = new Rect(-1,-1,0,0);
	public List<Rect> corridors = new List<Rect>();

	//Konstruktor
	public SubDungeon(Rect mRect){
		rect=mRect;
	}
	//levélnek nincsenek gyerekei
	public bool IAmLeaf(){
		return left == null && right == null;
	}
	public bool Split(int minRoomSize, int maxRoomSize){
		//ha nem levél, akkor nem kell szétválasztani
		if (!IAmLeaf ()) {
			return false;
		}

		bool splitH, shouldSplit;
		if (rect.width / rect.height >= 1.25) {
			splitH = false; //ha a pálya jóval szélesebb, mint magasabb, akkor függőlegesen szétválasztunk
			//shouldSplit=true;
		} else if (rect.height / rect.width >= 1.25) {
			splitH = true; //ha a pálya jóval  magasabb, mint szélesebb, akkor víszintesen szétválasztunk
			//shouldSplit=true;
		} else {
			splitH = Random.Range (0.0f, 1.0f) > 0.5; //egyébként random
			//shouldSplit=false;
		}

		if (Mathf.Min (rect.height, rect.width) / 2 < minRoomSize) {
			return false; // ha túl alacsony a rész magassága vagy szélessége, mint a minimum size, akkor az levél lesz, nem kell szétválasztani
		}
		//ha vízszintesen választunk
		if(splitH){
			//hol válasszunk szét, figyelve arra, hogy ne legyen túl kicsi a keletkező rész
			int split = Random.Range (minRoomSize, (int)(rect.width - minRoomSize));
			//Létrehozzuk a két új részt
			left = new SubDungeon (new Rect (rect.x, rect.y, rect.width, split));
			right = new SubDungeon (new Rect (rect.x, rect.y + split, rect.width, rect.height - split));
		}else {		//függőleges választás
			int split = Random.Range (minRoomSize, (int)(rect.height - minRoomSize));

			left = new SubDungeon (new Rect (rect.x, rect.y, split, rect.height));
			right = new SubDungeon (new Rect (rect.x + split, rect.y, rect.width - split, rect.height));
		}
		return true;

	}

	public void CreateRoom() {
		//ha vannak gyerekei, akkor halad lefelé a fában, nem hoz létre szobát addig még nem talál levél részt
		if (left != null) {
			left.CreateRoom ();
		}
		if (right != null) {
			right.CreateRoom ();
		}
		if (left != null && right != null) {
			CreateCorridorBetween (left, right);
		}
		if (IAmLeaf()) {
			//a legkisebb szoba fele akkora, mint a rész amiből kilett "vájva", a legnagyobb pedig 2-2 egységgel kisebb
			int roomWidth = (int)Random.Range (rect.width / 2, rect.width - 2);
			int roomHeight = (int)Random.Range (rect.height / 2, rect.height - 2);
			//starting koordináták
			int roomX = (int)Random.Range (1, rect.width - roomWidth - 1);
			int roomY = (int)Random.Range (1, rect.height - roomHeight - 1);
			// a keletkező szoba kezdőkoordinátáit a teljes maphoz vesszük, nem a részhez amiben keletkezik
			room = new Rect (rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);


		}

	}
	//arra van hogyha egy olyan subDungeont kapunk ami nem levél azaz nincs benne szoba, akkor a gyerekeinek a szobájait adja vissza
	public Rect GetRoom() {
		if (IAmLeaf()) {
			return room;
		}
		if (left != null) {
			Rect lroom = left.GetRoom ();
			if (lroom.x != -1) {
				return lroom;
			}
		}
		if (right != null) {
			Rect rroom = right.GetRoom ();
			if (rroom.x != -1) {
				return rroom;
			}
		}

		// workaround non nullable structs
		return new Rect (-1, -1, 0, 0);
	}
	// metódus ami a bal és jobb fiút összekapcsolja egy folyosóval
	public void CreateCorridorBetween(SubDungeon left, SubDungeon right) {
		Rect lroom = left.GetRoom ();
		Rect rroom = right.GetRoom ();

		// kiválasz egy random pontot a bal és jobb szobán is ahol össze lesznek kapcsolva
		Vector2 lpoint = new Vector2 ((int)Random.Range (lroom.x + 1, lroom.xMax - 1), (int)Random.Range (lroom.y + 1, lroom.yMax - 1));
		Vector2 rpoint = new Vector2 ((int)Random.Range (rroom.x + 1, rroom.xMax - 1), (int)Random.Range (rroom.y + 1, rroom.yMax - 1));

		// ha a jobb szoba kapcsolódási ponjának az x koordinátája kisebb, mint a bal szobájé, azaz balrábbb van, akkor az lesz a bal kapcsolódási pont
		if (lpoint.x > rpoint.x) {
			Vector2 temp = lpoint;
			lpoint = rpoint;
			rpoint = temp;
		}

		int w = (int)(lpoint.x - rpoint.x);
		int h = (int)(lpoint.y - rpoint.y);


		// if the points are not aligned horizontally
		if (w != 0) {
			// choose at random to go horizontal then vertical or the opposite
			if (Random.Range (0, 1) > 2) {
				// add a corridor to the right
				corridors.Add (new Rect (lpoint.x, lpoint.y, Mathf.Abs (w) + 1, 1));

				// if left point is below right point go up
				// otherwise go down
				if (h < 0) {
					corridors.Add (new Rect (rpoint.x, lpoint.y, 1, Mathf.Abs (h)));
				} else {
					corridors.Add (new Rect (rpoint.x, lpoint.y, 1, -Mathf.Abs (h)));
				}
			} else {
				// go up or down
				if (h < 0) {
					corridors.Add (new Rect (lpoint.x, lpoint.y, 1, Mathf.Abs (h)));
				} else {
					corridors.Add (new Rect (lpoint.x, rpoint.y, 1, Mathf.Abs (h)));
				}

				// then go right
				corridors.Add (new Rect (lpoint.x, rpoint.y, Mathf.Abs (w) + 1, 1));
			}
		} else {
			// if the points are aligned horizontally
			// go up or down depending on the positions
			if (h < 0) {
				corridors.Add (new Rect ((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs (h)));
			} else {
				corridors.Add (new Rect ((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs (h)));
			}
		}
	}


}