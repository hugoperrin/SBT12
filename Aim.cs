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

public class Aim : ObjectClass
{
    // link to other GameObjects
    public GameObject BallObject;
    public Ball Ball1;
    public GameObject player;
    public Player player1;
    public MenuManager menuManager;

    // initialization

    void Start()
    {
        position = transform.position;
    }

    // evolution of the crosshair position

    void FixedUpdate()
    {
        if (menuManager.IsOpen() == false) // we don't want it to update if the menu is visible and active
        {
            Vector3 PositionOld = position;
            if (Ball1.GetState()[0] == false)
            {
                if (player1.auto == false)
                {
                    position = new Vector3(((player1.GetPosition()).x), 2, player1.GetLong().z);
                    transform.Translate((position - PositionOld));

                }
            }
            else
            {
                position = new Vector3(((player1.GetPosition()).x), 2, player1.GetLong().z);
                transform.Translate((position - PositionOld));
            }
        }
    }
}

