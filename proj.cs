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
using System.Collections.Generic;
public class proj : ObjectClass {
    private double angle;
    public Player player;
    public Ball ball;
    public bool target;
    public bool touchProj;
    public proj elseProj;
	// Use this for initialization
	void Start () {
        angle = 0;
        touchProj = false;
	}
	
    IEnumerator OnTriggerEnter()
    {
        touchProj = true;
        player.SetScore(player.GetScore() + 50);
        angle = ball.GetRandomNumber(-Math.PI/5,Math.PI/5); // we generate a random number
        yield return new WaitForSeconds(1); // we wait a bit of time
        if (target)
        {
            transform.Translate(5 + ball.l * (float)Math.Atan(angle) - transform.position.x, 0, 0); // we move the target with a delay
            elseProj.transform.Rotate(new Vector3(0, (float)(angle * 180 / Math.PI) - elseProj.transform.rotation.eulerAngles[1], 0), Space.World);
        }
        touchProj = false;
    }

}
