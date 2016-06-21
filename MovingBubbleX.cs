using UnityEngine;
using System.Collections;

public class MovingBubbleX : Bubble {

	public float speed;
	public float Xmin;
	public float Xmax;

	private Vector3 movement;

	private bool moveRight; // true for moving right, false for moving left

	// Use this for initialization
	void Start () { // Awake in the superclass and start in the subclass
		moveRight = true;
	}

	// Update is called once per frame
	void Update () {
		if (moveRight) {
			movement = new Vector3 (1, 0.0f, 0.0f);
		} else {
			movement = new Vector3 (-1, 0.0f, 0.0f);
		}

		GetComponent<Rigidbody> ().velocity = movement * speed;

		if (transform.position.x > Xmax) {
			moveRight = false;
		}
		if (transform.position.x < Xmin) {
			moveRight = true;
		}
	}

}
