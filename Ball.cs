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
using System.Collections.Generic;

public class Ball : ObjectClass
{
    public double velocity;
    private bool move;
    private bool away; // specifies if the ball moves away the player or to the player
    public double angle; // angle of the trajectory
    public double angle2; // angle random of each ball exchange, which the NPC will try to maintain
    public double timePhase;
    public float z0;
    public float y0;
    public float x0;
    public float x1;
    public float l;
    public float b;
    private float g;
    public GameObject player;
    public Player playerObject;
    public GameObject npc;
    public NPC npcObject;
    public Vector3 normal = new Vector3(0, 0, 1);
    private Ball ballObject;
    public Slider sliderScore;
    public Text textScore;
    public MenuManager menuManager;
    // function to stop the ball when caught or lost
    public void Stop()
    {
        move = false;
        away = true;
        timePhase = 0;
        Vector3 positionInitial = new Vector3(playerObject.GetPosition().x, y0, z0);
        transform.Translate(positionInitial-position);
    }

    // function to release it after the ball has been caught or lost
    public void ResetAndRerun()
    {
        move = true;
    }
    public Ball(Ball ball1)
    {
        ballObject = ball1;
    }
    // change state of movement
    public void SetMove(bool move)
    {
        this.move = move;
    }
    public void SetAway(bool away)
    {
        this.away = away;
    }
    // get state of movement
    public bool[] GetState()
    {
        bool[] a =new bool[2];
        a[0] = move;
        a[1] = away;
        return a;
    }
    // create method for creating random numbers (for angles)

    public double GetRandomNumber(double minimum, double maximum)
    {
        System.Random random = new System.Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }
    // initialization

    IEnumerator Start()
    {
        InitList();
        b = 1;
        timePhase = 0;
        angle = 0;
        angle2 = 0;
        position = transform.position;
        positionList.Add(new Vector4(position.x, position.y, position.y, playerObject.stopwatch.ElapsedMilliseconds / 1000));
        move = true;
        away = true;
        z0 = position.z;
        y0 = position.y;
        x0 = playerObject.GetPosition().x;
        x1 = npcObject.GetPosition().x;
        npcObject.SetPosition(npcObject.transform.position);
        l =Math.Abs(playerObject.GetPosition().z-npcObject.GetPosition().z);
        g = (float)(9.81);
        velocity = Math.Sqrt(g*l/10);
        npcObject.omega =10*(float)velocity/l;
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        angle =Vector3.Angle(Vector3.ProjectOnPlane(npcObject.transform.position-position,new Vector3(0,1,0)),normal);
        if (npcObject.gamemode==0)
        {
            angle = 0;
        }
        angle2 = GetRandomNumber(-45, 45) * Math.PI / 180;
    }
    
    // Function to calculate next position

    private void NextPosition()
    {
        Vector3 Positionold = position;
        if (move == true)
        {
            // these two lines allow to hide the crosshair while the ball is moving
            playerObject.SetLong(new Vector3(0, 0, 0));
            playerObject.touchP = false;
            
            if (away==true)
            {
                // Trajectory when the ball is thrown by the player to the NPC
                position.z = (float)(timePhase * velocity/ (Math.Sqrt(2)) + z0);
                position.x = (float)(timePhase * velocity * Math.Sin(angle) / (Math.Sqrt(2)) + x0);
                position.y = (float)(-(timePhase * timePhase * (g*b /20 ))+ velocity * timePhase / (Math.Sqrt(2)) + y0);
            }
            if (away == false)
            {
                // Trajectory when the ball comes from the NPC to the player
                position.z = (float)(-timePhase * velocity / (Math.Sqrt(2)) + z0);
                position.x = (float)(-timePhase * velocity * Math.Sin(angle) / (Math.Sqrt(2)) + x1);
                if (npcObject.gamemode == 0)
                {
                    position.y = (float)(-(timePhase * timePhase * (g * b / 20)) + velocity * timePhase / (Math.Sqrt(2)) + y0);
                }
                else
                {
                    position.y = (float)(-(timePhase * timePhase * (g * b / 20)) + velocity * timePhase / (Math.Sqrt(2)) + y0);
                }
            }
            // move the ball
            transform.Translate(position - Positionold);
        }
        else
        {
            if (away)
            {
                position.x =playerObject.GetPosition().x;
                transform.Translate((position - Positionold).x,0,0);
            }
        }
    }


    // FixedUpdate loop

    void FixedUpdate()
    {
        if (menuManager.IsOpen()==false)
        {
            // Updating position
            NextPosition();
            // Updating time
            timePhase = timePhase + Time.fixedDeltaTime;
            // Updating position list
            if (playerObject.stopwatch.IsRunning)
            {
                positionList.Add(new Vector4(position.x, position.y, position.y, playerObject.stopwatch.ElapsedMilliseconds));
            }
        }
        
    } 
    void Update()
    {
        sliderScore.value = playerObject.GetScore();
        textScore.text = "Score:  " + playerObject.GetScore().ToString();
    }

}
