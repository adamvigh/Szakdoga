using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room{
	//private int leftX,rightX,bottomY,upperY;
	//private List<Room> neighbours;
	public int presetNumber;
	public Rect room=new Rect(-1,-1,0,0);

	public void setRect (int a, int b, int c, int d){
		room = new Rect (a, b, c, d);

	}
}
