using UnityEngine;
using System.Collections; // to import ArrayList
using Leap;
using System.Diagnostics; // to import Stopwatch



public class PlayerController : MonoBehaviour
{
	public Vector3 position;

	public ArrayList positionRecorded = new ArrayList();

	private AudioSource errorBoundarySound;
	private AudioSource errorControllerNotFoundSound;

	private GameController gameController;

	private Stopwatch stopwatch;
    public MenuManager menuManager;

	// First lines of code that will be read
	void Awake(){
		
		stopwatch = new Stopwatch();

		// Begin timing
		stopwatch.Start();

	}

    void Start (){

		// initialise the sounds
		GameObject errorBoundary = gameObject.transform.GetChild (0).gameObject;
		GameObject errorControllerNotFound = gameObject.transform.GetChild (1).gameObject;
		errorBoundarySound = errorBoundary.GetComponent<AudioSource> ();
		errorControllerNotFoundSound = errorControllerNotFound.GetComponent<AudioSource> ();
    }

    void FixedUpdate()
    {
        if (menuManager.IsOpen() == false)
        {
            float moveHorizontal = 0;
            float moveVertical = 0;
            float speed = 0;
            Vector3 currentPosition = GetComponent<Rigidbody>().position;
            Vector3 mouvement;
            speed = gameController.speed;

            try
            {
                mouvement = gameController.movementInput();
                moveHorizontal = mouvement.x;
                moveVertical = mouvement.y;
            }
            catch (ControllerNotFoundException e)
            {
                position = new Vector3(-0.5f, 0.87f, .49f); // initial position of the player
                GetComponent<Rigidbody>().position = position;
                if (!(errorControllerNotFoundSound.isPlaying)) { errorControllerNotFoundSound.Play(); }
            }

            positionRecorded.Add(new Vector4(currentPosition.x, currentPosition.y,
                currentPosition.z, (float)stopwatch.Elapsed.TotalMilliseconds / 1000));
            // Elapsed.TotalMilliseconds is a double that needs to be casted to a float	
            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
            GetComponent<Rigidbody>().velocity = movement * speed;


            // here, we set the boundaries of our game scene
            currentPosition = GetComponent<Rigidbody>().position;
            float xmin = -2.95f;
            float xmax = +2.0f; // these values have been set empirically
            float ymax = 2.45f;
            float ymin = -0.65f;

            if (currentPosition.x < xmin)
            {
                GetComponent<Rigidbody>().position = new Vector3(xmin, currentPosition.y, currentPosition.z);
                if (!(errorBoundarySound.isPlaying))
                {
                    errorBoundarySound.Play();
                }
            }
            if (currentPosition.x > xmax)
            {
                GetComponent<Rigidbody>().position = new Vector3(xmax, currentPosition.y, currentPosition.z);
                if (!(errorBoundarySound.isPlaying))
                {
                    errorBoundarySound.Play();
                }
            }
            if (currentPosition.y < ymin)
            {
                GetComponent<Rigidbody>().position = new Vector3(currentPosition.x, ymin, currentPosition.z);
                if (!(errorBoundarySound.isPlaying))
                {
                    errorBoundarySound.Play();
                }
            }
            if (currentPosition.y > ymax)
            {
                GetComponent<Rigidbody>().position = new Vector3(currentPosition.x, ymax, currentPosition.z);
                if (!(errorBoundarySound.isPlaying))
                {
                    errorBoundarySound.Play();
                }
            }

        }
        else
        {
            Cursor.visible = true;
            GetComponent<Rigidbody>().velocity = new Vector3(0,0);
        }
    }

	public void setGameController(GameController gameController){
		this.gameController = gameController;
	}

		
}
	