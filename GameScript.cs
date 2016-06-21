// Copyright 2016 Hugo Perrin, Younes Laaboudi, Maxime Fétiveau, Jules Massin, Olivier Polidori, Seung-Eun Yi 

//This file is part of SBT12-GameProjectForAutism.

//   SBT12-GameProjectForAutism is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    SBT12-GameProjectForAutism is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with SBT12-GameProjectForAutism.  If not, see<http://www.gnu.org/licenses/>.


using UnityEngine;
using System.Collections;
using System; // to import String
using System.IO ; // to import File
using System.Diagnostics; // to import Stopwatch

public class GameScript : MonoBehaviour {

	GameController gameController; // Mouse or LeapMotion

	public Bubbles bubblesPart1 ;
	public Bubbles bubblesPart2 ;
	public Bubbles bubblesPart3 ;
	public Bubbles bubblesPart4 ;
	public Bubbles movingBubblesPart1 ;
	public Bubbles movingBubblesPart2 ;
	public Bubbles movingBubblesPart3 ;
	public Bubbles fallingBubblesPart1 ;
	public Bubbles fallingBubblesPart2 ;

	Bubbles currentBubblesSet;

	public ArrayList bubblesSetsList = new ArrayList ();
	private int numberLevels;

	public PlayerController player; 

	private int score;
	private int bonusPoints;
	private int malusPoints;

    private void NextLevel()
    {
        Application.LoadLevel("level1");
    }
	private bool endFlag;

	void Awake () {

		player.setGameController (new Mouse());

		bubblesSetsList.Add (bubblesPart1);
		bubblesSetsList.Add (bubblesPart2);
		bubblesSetsList.Add (bubblesPart3);
		bubblesSetsList.Add (bubblesPart4);
		bubblesSetsList.Add (movingBubblesPart1);
		bubblesSetsList.Add (movingBubblesPart2);
		bubblesSetsList.Add (movingBubblesPart3);
		bubblesSetsList.Add (fallingBubblesPart1);
		bubblesSetsList.Add (fallingBubblesPart2);

		numberLevels = bubblesSetsList.Count;

		// starting the game
		score = 0;

		this.setAllBubblesInactive();
		this.displayBubbles(bubblesPart1);
		currentBubblesSet = bubblesPart1;

		endFlag = false;
	}
	
	// Update is called once per frame
	void Update () {

		this.checkController ();

		// enables to exit the game smoothly

		// to go back to the first level
		if (Input.GetKey ("r")) {
			resetGame ();
		}
		//to manually operate how levels change
		//if (Input.GetKeyDown ("n")) { // without "Down" several levels are simultaniousily passed
			//nextLevel ();
		//}
		//if (Input.GetKeyDown ("b")) {
			//previousLevel ();
		//}
        // get the menu


		// updating score
		score = 0;
		foreach (Bubbles bubbles in bubblesSetsList) {
			score += bubbles.numberBubblesPopped;
		}

		int i = bubblesSetsList.IndexOf (currentBubblesSet);

		//handles the changes of levels inside level 0
		if (i< numberLevels - 1 && currentBubblesSet.numberBubbles == currentBubblesSet.numberBubblesPopped){
			currentBubblesSet.timeMesured = currentBubblesSet.timeElapsed.ElapsedMilliseconds*0.001f; // timeElapsed.stop() didn't work
			nextLevel();
		}
		if (!endFlag && i == numberLevels - 1 && currentBubblesSet.numberBubbles == currentBubblesSet.numberBubblesPopped){
			AudioSource sound = GetComponent<AudioSource> ();
			sound.Play ();
			currentBubblesSet.timeMesured = currentBubblesSet.timeElapsed.ElapsedMilliseconds*0.001f;
			print("Fin du niveau 0");
            //this.writeRecordedPosition ();
            NextLevel();
            endFlag = true;
		}

		//handles the difficulty of the level
		if (currentBubblesSet.timeElapsed.Elapsed.TotalMilliseconds / 1000 > 3) {
			if (!currentBubblesSet.changingSize) {
				currentBubblesSet.startChangingSize ();
			}
		} 
		if (currentBubblesSet.timeElapsed.Elapsed.TotalMilliseconds / 1000 > 15
		    && currentBubblesSet.numberBubblesPopped < currentBubblesSet.numberBubbles / 2) {
			previousLevel ();
		} else {

			if (currentBubblesSet.timeElapsed.Elapsed.TotalMilliseconds / 1000 > 15
			    && currentBubblesSet.numberBubblesPopped > currentBubblesSet.numberBubbles - 2) {
				malusPoints += currentBubblesSet.numberBubbles - currentBubblesSet.numberBubblesPopped;
				currentBubblesSet.numberBubblesPopped = currentBubblesSet.numberBubbles; // the next time Update is called, it will go to the next level
			} else {
				if (currentBubblesSet.timeElapsed.Elapsed.TotalMilliseconds / 1000 > 20
				   && currentBubblesSet.numberBubblesPopped < currentBubblesSet.numberBubbles) {
					previousLevel ();
				}
			}
		}


	}

	void resetGame(){
		setAllBubblesInactive ();
		displayBubbles (bubblesPart1);
		endFlag = false;
	}

	void previousLevel(){
		setAllBubblesInactive ();
		int i = bubblesSetsList.IndexOf(currentBubblesSet);
		if (i > 0) { // if we are in the first level, there is no way of going down a level
			displayBubbles ((Bubbles) bubblesSetsList [i - 1]);
			currentBubblesSet = (Bubbles) bubblesSetsList [i - 1];
			endFlag = false;
		}
	}

	void nextLevel(){
		int i = bubblesSetsList.IndexOf(currentBubblesSet);
		if (i < numberLevels - 1) {
			displayBubbles ((Bubbles) bubblesSetsList [i + 1]);
			currentBubblesSet = (Bubbles) bubblesSetsList [i + 1];
		}
	}

	void setAllBubblesInactive(){
		foreach (Bubbles bubbles in bubblesSetsList){
			bubbles.setBubblesInactive();
		}

	}

	void displayBubbles(Bubbles bubbles){
		foreach (Bubbles otherBubbles in bubblesSetsList){
			otherBubbles.setBubblesInactive ();
		}
		bubbles.setBubblesActive ();
	}

	void writeRecordedPosition(){

		String stringRecorded = "";

		foreach(Vector4 vector in player.positionRecorded ){
			stringRecorded += vector.x;
			stringRecorded += " "; // the separator is a space
			stringRecorded += vector.y;
			stringRecorded += " ";
			stringRecorded += vector.z;
			stringRecorded += " ";
			stringRecorded += vector.w;
			stringRecorded += "\n";
		}

		File.WriteAllText("RecordedPosition.txt", stringRecorded);

		String stringTimes = "";
		foreach(Bubbles bubblesSet in bubblesSetsList ){
			stringTimes += bubblesSet.ToString();
			stringTimes += " "; // the separator is a space
			stringTimes += bubblesSet.timeMesured.ToString();
			stringTimes += "\n";
		}

		File.WriteAllText("TimesMesured.txt", stringTimes);


	}
    public void SetGameController(string type)
    {
        if (type=="leap")
        {
            player.setGameController(new LeapMotion());
        }
        else
        {
            player.setGameController(new Mouse());
        }
    }
	void checkController(){
		// Part related to the controller input
		if (Input.GetKeyDown("l"))
        { // equivalent to get key L
			player.setGameController (new LeapMotion());
			print ("LeapMotion is the controller");
		}

		if (Input.GetKeyDown("m"))
        { // equivalent to get key M
			player.setGameController (new Mouse());
			print ("Mouse is the controller");
		}
		
	}


}
