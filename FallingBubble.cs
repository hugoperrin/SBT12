using UnityEngine;
using System.Collections;

public class FallingBubble : Bubble {

	public float speed;
	public float Yinit; // initial position of the bubble

	// Update is called once per frame
	void Update () {
		changingSize = false; 
		Vector3 movement = new Vector3 (0.0f, -1, 0.0f);
		GetComponent<Rigidbody> ().velocity = movement * speed;
	}
}
