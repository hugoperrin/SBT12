using UnityEngine;
using System.Collections;
using System.Diagnostics; // to import Stopwatch

public class Bubbles : MonoBehaviour {

	public int numberBubbles;
	public int numberBubblesPopped;

	public Stopwatch timeElapsed;
	public float timeMesured;

	public bool changingSize;

	void Start()
    {
		Transform bubbles = gameObject.GetComponentInChildren<Transform>();
		foreach (Transform bubble in bubbles) {
			numberBubbles += 1;
		}

		numberBubblesPopped = 0;
		changingSize = false;
		timeElapsed = new Stopwatch();
	}

	public void setBubblesActive(){
		Transform bubbles2 = gameObject.GetComponentInChildren<Transform>();
		foreach (Transform bubble in bubbles2) {
			bubble.gameObject.SetActive (true); 
			bubble.GetComponent<Renderer> ().enabled = true;
		}
		Bubble[] bubbles = gameObject.GetComponentsInChildren<Bubble>();
		foreach (Bubble bubble in bubbles) {
			bubble.changingSize = false;
			FallingBubble fallingbubble = bubble as FallingBubble;
			if (fallingbubble != null) {
				Vector3 position = bubble.GetComponent<Transform> ().position;
				position.y = ((FallingBubble) bubble).Yinit;
				bubble.GetComponent<Transform> ().position = position;
			}
		}
		numberBubblesPopped = 0;
		// Begin timing
		timeElapsed.Start();

	}

	public void setBubblesInactive(){
		Transform bubbles = gameObject.GetComponentInChildren<Transform>();
		foreach (Transform bubble in bubbles) {
			bubble.gameObject.SetActive (false);
		}
		numberBubblesPopped = 0;
		// Prepares a new timing to be taken
		timeElapsed = new Stopwatch();
	}

	public void startChangingSize(){
		changingSize = true;

		Bubble[] bubbles = gameObject.GetComponentsInChildren<Bubble>();
		foreach (Bubble bubble in bubbles) {
			bubble.changingSize = true;
		}
	}

}
