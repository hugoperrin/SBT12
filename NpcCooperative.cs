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

public class NpcCooperative : NPC 
{
    public NpcOpponent npcOpponent;
    void Start()
    {
        InitList();
        maxVelocity = 1;
        opponent = false;
        wall = false;
    }
    IEnumerator OnTriggerEnter(Collider col)
    {
        if (ballObject.GetState()[1] == true)
        {
            touch = true;
            hasBall = true;
            exchange = exchange + 1;
            yield return new WaitForSeconds(10 * (10 - difficulty) * Time.fixedDeltaTime);
            ballObject.angle = Vector3.Angle(Vector3.ProjectOnPlane(GetPosition() - playerObject.GetPosition(), new Vector3(0, 1, 0)), new Vector3(0, 0, 1)) * Math.PI / 180;
            if ((GetPosition() - playerObject.GetPosition()).x < 0)
            {
                ballObject.angle = -ballObject.angle;

            }
            touch = false;
            hasBall = false;
            playerObject.SetNewScore(100);
        }
    }

    void NextPosition(double angle)
    {
        float eps = 0.5f;
        Vector3 PositionOld = position;
        if (GetList().Count == 0)
        {
            AddPosition(position);
            ballObject.AddPosition(ballObject.GetPosition());
            playerObject.AddPosition(playerObject.GetPosition());
            playerObject.AddPosition(playerObject.GetPosition());
        }
        if (GetList().Count < 4)
        {

        }
        else
        {
            // Movement if the ball moves (is thrown)
            if (ballObject.GetState()[0])
            {
                // if it comes from the player to the npc
                if (ballObject.GetState()[1])
                {
                    // the movement to get the ball
                    if (ballObject.timePhase * ballObject.velocity / ballObject.l < Math.PI / 10)
                    {
                        position.x = distance * ((float)Math.Sin(omega * (ballObject.timePhase + Time.fixedDeltaTime) - Math.PI / 2) - (float)Math.Sin(omega * ballObject.timePhase - Math.PI / 2)) + position.x;
                    }
                }
                //if the npc threw it to the player
                else
                {
                    // it places itself in front of the player to be ready afterwards
                    position.x = position.x + 0.003f * Math.Sign(playerObject.GetPosition().x - position.x) * Math.Max(maxVelocity, Math.Abs(playerObject.GetPosition().x - position.x));
                }
            }
            // movement when the ball is caught
            else
            {
                // movement when the opponent isn't there
                if (eventManager.GetStatus()<5)
                {
                    position.x = position.x + 0.003f*Math.Sign(playerObject.GetPosition().x - position.x)*Math.Max(maxVelocity,Math.Abs(playerObject.GetPosition().x - position.x));
                }
                // movement when the opponent is there
                else
                {
                    if ((playerObject.projectedPosition.x-npcOpponent.GetPosition().x)/ballObject.l*(position.z-playerObject.GetPosition().z)<maxDistance)
                    {
                        speed = (position.x - LastPosition(1).x) / (Time.deltaTime);
                        if (npcOpponent.GetPosition().x<5)
                        {
                            if (Math.Abs(speed) < maxVelocity)
                            {
                                position.x = 2 * LastPosition(1).x - LastPosition(2).x + (Time.deltaTime * Time.deltaTime) * 2;
                            }
                            else
                            {
                                position.x = position.x + maxVelocity*Time.fixedDeltaTime;
                            }
                        }
                        else
                        {
                            if (Math.Abs(speed) < maxVelocity)
                            {
                                position.x = 2 * LastPosition(1).x - LastPosition(2).x + -(Time.deltaTime * Time.deltaTime) * 2;
                            }
                            else
                            {
                                position.x = position.x + -maxVelocity * Time.fixedDeltaTime;
                            }
                        }
                    }
                }
            }
        }
        // command to actually move the NPC
        if ((0.5 < position.x) & (position.x < 9))
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
            double a = 3 * (1 - Math.Exp(-exchange / 3));
            ballObject.b = (float)Math.Pow(2, a);
            ballObject.velocity = Math.Sqrt(9.81 * ballObject.l / 10) * Math.Sqrt((float)Math.Pow(2, a));
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
            if (ballObject.velocity > 200)
            {
                exchange = 0;
                ballObject.b = ballObject.b / (2 ^ 4);
                ballObject.velocity = ballObject.velocity * Math.Sqrt(2 ^ 4);

            }
        }
    }
}
