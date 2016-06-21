using UnityEngine;
using System.Collections; // to import ArrayList
using Leap;
using System.Diagnostics; // to import Stopwatch

public abstract class GameController {

	public float speed = 0;

	public abstract Vector3 movementInput ();
}

public class ControllerNotFoundException: UnityException{
	
}

public class Mouse : GameController {

	public Mouse() {
		Cursor.visible = false; // the mouse cursor is not displayed
		speed = 3.0f; // enables to set the speed of the movement when the Mouse is used
	}

	public override Vector3 movementInput (){
		float moveHorizontal = Input.GetAxis ("Mouse X");
		float moveVertical = Input.GetAxis ("Mouse Y");
		return new Vector3 (moveHorizontal, moveVertical, 0.0f);
	}
}



public class LeapMotion : GameController {


	public Controller controller; // the leap device
	public Frame frame; // frame object where the leap input will be set
	public HandList hands;
	public Hand handright; // corresponds to the hand at the rightmost

	public LeapMotion() {
		Cursor.visible = false; // the mouse cursor is not displayed
		controller = new Controller(); // this class is related to the leap motion
		frame = controller.Frame(); // controller is a Controller object
		hands = frame.Hands;
		handright = hands.Rightmost;
		speed = 0.02f;  // enables to set the speed of the movement when the LeapMotion is used
	}

	public override Vector3 movementInput (){
		float moveHorizontal;
		float moveVertical;

		frame = controller.Frame(); // controller is a Controller object
		hands = frame.Hands;
		handright = hands.Rightmost;

		if (hands.IsEmpty) {
			throw new ControllerNotFoundException ();
		} else {
			handright = hands.Rightmost;
			// retrieving the position
			moveHorizontal = handright.PalmVelocity.x;
			moveVertical = handright.PalmVelocity.y;
		}

		return new Vector3 (moveHorizontal, moveVertical, 0.0f);
	}
}