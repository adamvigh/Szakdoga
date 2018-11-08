using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private GameObject player;       


	private Vector3 offset;        


	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		//transform.SetParent (player.transform);
		offset = transform.position - player.transform.position;
	}


	void FixedUpdate () 
	{
		float desiredAngle = player.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler (0, desiredAngle, 0);
		transform.position = player.transform.position - (rotation * offset);
		transform.LookAt (player.transform);
	}
}