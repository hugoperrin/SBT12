using UnityEngine;
using System.Collections;

public class MovingBubbleY : Bubble {

	public float speed;
	public float Ymin;
	public float Ymax;

	private Vector3 movement;

	private bool moveUp; // true for moving up, false for moving down

	// Use this for initialization
	void Start () {
		moveUp = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (moveUp) {
			movement = new Vector3 (0.0f, 1, 0.0f);
		} else {
			movement = new Vector3 (0.0f, -1, 0.0f);
		}

		GetComponent<Rigidbody> ().velocity = movement * speed;

		if (transform.position.y > Ymax) {
			moveUp = false;
		}
		if (transform.position.y < Ymin) {
			moveUp = true;
		}
	}
}
