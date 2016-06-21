using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	private AudioSource bubblePoppingSound;

	private ParticleSystem bubbleParticules;

	private GameObject bubblesFlying;

	private Bubbles bubbles;
    public MenuManager menuManager;
    public bool changingSize;

	private int sizeUp = 0;
	private int sizeDown = 0;
	private int waitCounter = 0;
	private int timeToWait;

	// Use this for initialization
	void Awake () {
		bubblePoppingSound = GetComponent<AudioSource>();
		bubblesFlying = gameObject.transform.GetChild (0).gameObject;
		bubbleParticules = bubblesFlying.GetComponent<ParticleSystem> ();
		bubbles = GetComponentInParent<Bubbles> ();
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
		timeToWait = Random.Range (0, 100);

	}

	void FixedUpdate ()
    {
        if (menuManager.IsOpen() == false)
        {
            // part ensuring that all the bubbles don't grow in size at exactely the same time
            if (changingSize && waitCounter < timeToWait)
            {
                waitCounter += 1;
            }
            else
            {

                // the scale changing part
                if (changingSize && sizeUp < 100)
                {
                    transform.localScale += new Vector3(0.001F, 0.001F, 0.001F);
                    sizeUp += 1;
                }
                else
                {
                    if (changingSize)
                    {
                        transform.localScale -= new Vector3(0.001F, 0.001F, 0.001F);
                        sizeDown += 1;
                        if (sizeDown > 100)
                        {
                            sizeUp = 0;
                            sizeDown = 0;
                        }
                    }
                }
            }
        }
	}

	IEnumerator OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			GetComponent<Renderer> ().enabled = false;
			if (!bubbleParticules.isPlaying) { // meaning : if the bubble has already been poped but the animation is still playing
				bubblePoppingSound.Play ();
			}
			bubbleParticules.Play ();
			yield return new WaitForSeconds (1); // if we set the bubble as inactive directly, the animation of bubbles flying won't play
			gameObject.SetActive (false);
			bubbles.numberBubblesPopped += 1;
		}
	}

}
