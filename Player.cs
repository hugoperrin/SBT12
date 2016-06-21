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




using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap;
using System.Collections.Generic;
using System.Diagnostics;

public class Player : ObjectClass
{
    public string controlType; // which input type will be used
    private Vector3 Long; // local variable for the placement of the crosshair
    private bool rightHanded; //if the player is right handed or left handed
    public Vector3 normal = new Vector3(0, 0, 1);
    public Controller controller; // the leap device
    private Frame frame; // frame object where the leap input will be set
    private Hand handLeft; // correspond to the hand at the leftmost
    private Hand handRight; // correspond to the hand at the rightmost
    public GameObject ballObject; // Player GameObject to interact with the right object Ball
    public Ball ball; // Ball component
    public bool touchP; // boolean which tells the other Gameobject if the ball is grabbable by the player
    public Camera cameraObject; // links to the camera
    public Vector3 direction;
    public Vector3 projectedPosition;
    public GameObject aim; // links to aim GameObject
    public Aim aimComponent; // Aim component
    public bool auto; // aiming mode
    public GameObject npc;
    public NPC npcObject;
    public NPC npcOpponent;
    private int score; 
    public Text error;
    public RawImage stopSign;
    public Stopwatch stopwatch;
    public Profile profile;
    public EventManager eventManager;
    public MenuManager menuManager;

    //Access to variable of interest
    public int GetScore()
    {
        return score;
    }
    public void SetScore(int newScore)
    {
        score = newScore;
    }
    public void SetNewScore(int increment)
    {
        score = score + increment;
    }
        //Change the control type
    public void SetControlType(string type)
    {
        controlType = type;
        profile.SetControlType(type);
    }
    // Access Long
    public Vector3 GetLong()
    {
        return Long;
    }
    // Modify Long
    public void SetLong(Vector3 newLong)
    {
        Long = newLong;
    }
    // Get the left hand 
    public Hand GetHandLeft()
    {
        return handLeft;
    }
    // Get the right hand
    public Hand GetHandRight()
    {
        return handRight;
    }
    public void SetHandPreference (bool right)
    {
        rightHanded = right;
        profile.SetHandPreference(right);
    }
    public void Stop()
    {
        stopwatch.Stop();
        //enabled = false;
    }
    public void Restart()
    {
        //enabled = true;
        stopwatch.Start();
    }
    // Initialization of values
    void Start()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
        profile.LoadProfile();
        InitList();
        controlType = profile.GetControlType();
        auto = false; 
        rightHanded = true; 
        touchP = false;
        Long = transform.position;
        SetPosition(transform.position);
        AddPosition(GetPosition());
        controller = new Controller();
        DeviceList connectedLeaps = controller.Devices;
        frame = controller.Frame(); // controller is a Controller object
        HandList hands = frame.Hands;
        handLeft = hands.Leftmost;
        handRight = hands.Rightmost;
        
    }

    // Determines whether or not the player catches the ball
    public bool OnCollisionCatch()
    {
        // first condition is about whether or not the ball is grabbable
        if (touchP)
        {
            // several cases depending on the controller type
            if (controlType == "leap")
            {
                if ((controller.IsConnected) & (controlType == "leap"))
                {
                    if (rightHanded)
                    {
                        if (handRight.PinchStrength > 0.1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (handLeft.PinchStrength > 0.1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            if (controlType=="Keyboard+Mouse")
            {
                // grab key is left click
                if(Input.GetKey(KeyCode.Mouse1) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
             
        }
        else
        {
            return false;
        }
    }

    // determines the routine to follo when the ball arrives at the hand
    IEnumerator OnTriggerEnter(Collider col)
    {
        if (ball.GetState()[1] == false)
        {
            touchP = true; // boolean to give a condition for the grab mechanics
            Long=new Vector3(0, 0, npcObject.GetPosition().z);
            yield return new WaitForSeconds(6);
            touchP = false;
            Long = new Vector3(0, 0, 0);

        }
    }


    void FixedUpdate()
    {
        if (menuManager.IsOpen() == false) // we don't want it to update if the menu is visible and active
        {
            // Defining the next position
            position = transform.position;
            Vector3 PositionOld = position;
            if (controlType == "leap")
            {
                if ((controller.IsConnected) & (controlType == "leap"))
                {
                    // the player plays with the leap motion
                    // first step is to get the hand objects from the leapmotion device
                    frame = controller.Frame();
                    HandList hands = frame.Hands;
                    handLeft = hands.Leftmost;
                    handRight = hands.Rightmost;
                    // retrieving the position
                    position = new Vector3(((handLeft.StabilizedPalmPosition + handRight.StabilizedPalmPosition).x) / 95 + 5, PositionOld.y, PositionOld.z);
                }
            }
            if (controlType == "Keyboard+Mouse")
            {
                // Mouse and keyboard controls
                position = new Vector3(Input.GetAxis("Mouse X") / 100 + position.x, PositionOld.y, PositionOld.z);
            }


            // determines what to do if the player catches the ball
            eventManager.SetPlayerHasBall(false);
            if (OnCollisionCatch())
            {
                //stop the ball
                ball.SetMove(false);
                eventManager.SetPlayerHasBall(true);
                // initiate the next phase
                ball.SetAway(true);
                ball.timePhase = 0;
                ball.z0 = ball.GetPosition().z;
                ball.y0 = ball.GetPosition().y;
                ball.x0 = ball.GetPosition().x;
                npcObject.onStopPosition = npcObject.GetPosition();

                // determines the value of Ball1.angle, i.e. the aimin process
                if (auto)
                {
                    // auto mode sends the ball to a position randomly close to the one of the NPC
                    ball.angle = (Vector3.Angle(Vector3.ProjectOnPlane(npcObject.GetPosition() - position, new Vector3(0, 1, 0)), new Vector3(0, 0, 1)) + ball.GetRandomNumber(-15, 15)) * Math.PI / 180;
                    if ((npcObject.GetPosition() - position).x < 0)
                    {
                        ball.angle = -ball.angle;
                    }

                }
                else
                {
                    // aiming mode lets the player move the hand so as to move the "crosshair"
                    ball.angle = Math.Atan((positionList[positionList.Count - 1] - positionList[positionList.Count - 15]).x / (10 * Time.fixedDeltaTime * ball.l)) / 10;
                }
                projectedPosition = new Vector3(GetPosition().x + (float)Math.Tan(ball.angle) * ball.l, npcObject.GetPosition().y, npcObject.GetPosition().z);
                npcObject.SetDistance((projectedPosition - npcObject.transform.position).x / 2);
                npcOpponent.SetDistance((projectedPosition - npcOpponent.transform.position).x / 2);
                ball.angle2 = ball.GetRandomNumber(-45, 45) * Math.PI / 180;

            }
            else
            {
                ball.SetMove(true);

            }
            if ((0.5 < position.x) & (position.x < 9))
            {
                stopSign.enabled = false;
                error.text = "";
                transform.Translate((GetPosition() - PositionOld));
                cameraObject.transform.Translate((GetPosition() - PositionOld));
            }
            else
            {
                error.text = "pas possible d'aller par ici";
                stopSign.enabled = true;
            }
            if (stopwatch.IsRunning)
            {
                AddPosition(new Vector4(GetPosition().x, GetPosition().y, GetPosition().y, stopwatch.ElapsedMilliseconds));
            }
        }
    }
}
