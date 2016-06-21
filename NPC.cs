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
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public abstract class NPC : ObjectClass
{
    public bool opponent; // give the information about what type of Npc it is, opponent or not
    public bool wall; // allow the possibility to interact with a wall for the first level of difficulty
    public bool hasBall; // tells if te NPC has the ball or not
    public Vector3 onStopPosition; // stores the position of the NPC at the beginniing of the movement
    public int gamemode; // give info about the gamemode, to change the behavior of the AI
    public int difficulty; // give the difficulty to change the AI behavior
    private int behavior; // Last component of the behavior in the HDC model: competitive, collaborative etc.
    public double speed; // gives the speed of the NPC (related to the difficulty)
    private float a; // HDC parameter
    private float b; // HDC parameter
    private float c; // HDC parameter
    private float k;
    private float m;
    public float distance;
    public float maxDistance;
    protected float maxVelocity;
    public float omega;
    private float alpha;
    public float tau; // time constant
    public bool touch; // tells if the ball is grabbable by the NPC
    public int exchange;
    public Collider NPCCol;
    // Link to other GameObjects
    public GameObject ball;
    public Ball ballObject;
    public GameObject player;
    public Player playerObject;
    public proj proj;
    public EventManager eventManager;
    public int retard;
    public MenuManager menuManager;

    // change distance to get to the ball
    public void SetDistance(float dist)
    {
        if (Math.Abs(dist) < maxDistance)
        {
            distance = dist;
        }
        else
        {
            distance = maxDistance* Math.Sign(dist);
        }
    }


    // Initialization of values

    void Start()
    {
        InitList();
        retard = 250;
        k = 1000;
        m = 100;
        alpha = 1000;
        exchange = 0;
        difficulty = 10;
        touch = false;
        a = 0.1f;
        b = 0.2f;
        c = 0.01f;
        tau = 1;
        distance = 0;
        maxVelocity = 2;
        position = transform.position;
        onStopPosition = position;
        playerObject.projectedPosition =position;
        AddPosition(position);
        // Initialization of the gamemode based on the scene components
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Opponent");
        if (gos.Length == 0)
        {
            if (wall)
            {
                gamemode = 0;
                ballObject.angle = 0;
            }
            else
            {
                gamemode = 1;
            }
        }
        else
        {
            gamemode = 2;
        }
    }
    interface NpcMethods
    {
        // Determines whether or not the NPC catches the ball, and the routine that follows
        IEnumerator OnTriggerEnter(Collider col);
        // calculates the next position given the random angle and the gamemode and the player/ball position
        void NextPosition(double angle);
        // set the differential equation coefficients of the behavioral model
        void SetCoeff();
    }
    // FixedUpdate loop to simulate the behavior of the NPC



}

