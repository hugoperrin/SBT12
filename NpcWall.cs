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

public class NpcWall : NPC
{
    void Start()
    {
        InitList();
        opponent = false;
        wall = true;
    }
    IEnumerator OnTriggerEnter(Collider col)
    {
        if (ballObject.GetState()[1] == true)
        {
            touch = true;
            hasBall = true;
            exchange = exchange + 1;
            yield return new WaitForSeconds(10 * (10 - difficulty) * Time.fixedDeltaTime);
            ballObject.angle = -ballObject.angle;
            if (proj.touchProj == false)
            {
                if (eventManager.GetStatus() > 0)
                {
                    playerObject.SetScore(playerObject.GetScore() - 10);
                    exchange = exchange-1 ;
                }
                else
                {
                    playerObject.SetScore(playerObject.GetScore() +50);
                }
            }
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
            AddPosition(position);
            ballObject.AddPosition(ballObject.GetPosition());
            playerObject.AddPosition(playerObject.GetPosition());
            playerObject.AddPosition(playerObject.GetPosition());
        }
        if (positionList.Count < 4)
        {

        }
        else
        {

            if (ballObject.GetState()[0])
            {
                if (ballObject.GetState()[1])
                {
                    if (ballObject.timePhase * ballObject.velocity / ballObject.l < Math.PI / 10)
                    {
                        position.x = distance * ((float)Math.Sin(omega * (ballObject.timePhase + Time.fixedDeltaTime) - Math.PI / 2) - (float)Math.Sin(omega * ballObject.timePhase - Math.PI / 2)) + position.x;
                    }
                }
                else
                {
                        if ((position - playerObject.GetPosition()).x < 0)
                        {
                            if (Math.Sign(speed) == -1)
                            {
                                speed = -speed;
                            }
                        }
                        else
                        {
                            if (Math.Sign(speed) == 1)
                            {
                                speed = -speed;
                            }
                        }
                        //Position.x = Position.x + (float)speed * Time.fixedDeltaTime;
                }
            }
            else
            {
                if (Math.Abs((GetPosition() - playerObject.GetPosition()).x) < eps)
                {

                    speed = Math.Min((GetPosition(positionList.Count - 1) - GetPosition(positionList.Count - 2)).x / Time.fixedDeltaTime, 3);
                    speed = Math.Max(speed, 0.5);
                }
                else
                {
                    if ((GetPosition() - playerObject.GetPosition()).x < 0)
                    {
                        if (Math.Sign(speed) == -1)
                        {
                            speed = -speed;
                        }
                    }
                    else
                    {
                        if (Math.Sign(speed) == 1)
                        {
                            speed = -speed;
                        }
                    }
                    //Position.x = Position.x + (float)speed * Time.fixedDeltaTime;
                }
            }
        }
        // command to actually move the NPC
        transform.Translate(GetPosition() - PositionOld);
        position = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        position = transform.position;
        double a = 3 * (1 - Math.Exp(-exchange / 3));
        ballObject.b = (float)Math.Pow(2, a);
        ballObject.velocity = Math.Sqrt(9.81 * ballObject.l / 10) * Math.Sqrt((float)Math.Pow(2, a));
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
    
        AddPosition(new Vector4(position.x, position.y, position.y, playerObject.stopwatch.ElapsedMilliseconds));
        
    }
}
