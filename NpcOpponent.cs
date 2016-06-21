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
using System;
using System.Collections;

public class NpcOpponent : NPC
{
    public NpcCooperative npcCooperative;

    void Start()
    {
        InitList();
        maxVelocity = 0.5f;
        maxDistance = (float)(maxVelocity * Math.Abs(position.z-playerObject.GetPosition().z) / ballObject.velocity);
        print("maxDistance opponent" + maxDistance.ToString());
        opponent = true;
        wall = false;
    }
    IEnumerator OnTriggerEnter(Collider col)
    {
        if (ballObject.GetState()[1]==true)
        {
            touch = true;
            hasBall = true;
            exchange = exchange - 10;
            print("catch it");
            ballObject.Stop();
            yield return new WaitForSeconds(4);
            ballObject.ResetAndRerun();
            touch = false;
            hasBall = false;

        }
    }
    void NextPosition(double angle)
    {
        float eps = 0.5f;
        Vector3 PositionOld = position;
        if (positionList.Count == 0)
        {
            AddPosition(new Vector4(position.x,position.y,position.z,playerObject.stopwatch.ElapsedMilliseconds));
            ballObject.AddPosition(new Vector4(ballObject.GetPosition().x, ballObject.GetPosition().y, ballObject.GetPosition().z, playerObject.stopwatch.ElapsedMilliseconds));
            playerObject.AddPosition(new Vector4(playerObject.GetPosition().x, playerObject.GetPosition().y, playerObject.GetPosition().z, playerObject.stopwatch.ElapsedMilliseconds));
            playerObject.AddPosition(new Vector4(playerObject.GetPosition().x, playerObject.GetPosition().y, playerObject.GetPosition().z, playerObject.stopwatch.ElapsedMilliseconds));
        }
        if (positionList.Count < 4)
        {

        }
        else
        {
            
            if (ballObject.GetState()[0])
            {
                if (ballObject.timePhase * ballObject.velocity / ballObject.l < Math.PI / 10)
                {
                     position.x = distance * ((float)Math.Sin(omega * (ballObject.timePhase + Time.fixedDeltaTime) - Math.PI / 2) - (float)Math.Sin(omega * ballObject.timePhase - Math.PI / 2)) + position.x;
                }
            }
            else
            {
                position.x = position.x + difficulty *0.01f* Math.Sign(playerObject.GetPosition().x - position.x) * Math.Max(maxVelocity, Math.Abs(playerObject.GetPosition().x - position.x));
            }
        }
        // command to actually move the NPC
        if ((0.5 < position.x) &(position.x < 9))
        {
            transform.Translate(GetPosition() - PositionOld);
        }
        position = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (menuManager.IsOpen() == false) // we don't want it to update if the menu is visible and active
        {
            position = transform.position;
            double a = exchange / 10;
            ballObject.b = (float)Math.Pow(2, (Math.Floor(a)));
            ballObject.velocity = Math.Sqrt(9.81 * ballObject.l / 10) * Math.Sqrt((float)Math.Pow(2, (Math.Floor(a))));
            omega = 10 * (float)ballObject.velocity / ballObject.l;
            maxDistance = (float)(maxVelocity * ballObject.l / ballObject.velocity);
            if (touch == false)
            {

                // give the movementof the NPC wen the ball is out of reach
                if (ballObject.GetState()[0] == true)
                {
                    NextPosition(ballObject.angle2);
                }
                else
                {
                    if (hasBall == false)
                    {
                        NextPosition(ballObject.angle2);
                    }
                }
            }
            else
            {

                // initiate grab routine
                ballObject.SetAway(false);
                ballObject.timePhase = 0;
                ballObject.z0 = position.z;
                ballObject.y0 = position.y;
                ballObject.x1 = ballObject.GetPosition().x;

            }
            // stores the position and update frame number
            if (playerObject.stopwatch.IsRunning)
            {
                AddPosition(new Vector4(position.x, position.y, position.y, playerObject.stopwatch.ElapsedMilliseconds));
            }
            if (ballObject.velocity > 100)
            {
                exchange = 0;
                ballObject.b = ballObject.b / (2 ^ 4);
                ballObject.velocity = ballObject.velocity * Math.Sqrt(2 ^ 4);

            }
        }
    }
}
