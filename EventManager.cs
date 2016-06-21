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
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public Player player;
    public Ball ball;
    public NpcOpponent npcOpponent;
    public NpcCooperative npcCooperative;
    public int status;
    public bool playerHasBall;
    public string sceneName;

    public int GetStatus()
    {
        return status;
    }
    public void SetStatus(int newStatus)
    {
        status = newStatus;
    }
    public void SetPlayerHasBall(bool hasBall)
    {
        playerHasBall = hasBall;
    }
    void Start()
    {
        status = 0;
        playerHasBall = false;
        if (npcOpponent != null)
        {
            npcOpponent.enabled = false;
            npcOpponent.GetComponent<MeshRenderer>().enabled = false;
            npcOpponent.GetComponent<Collider>().enabled = false;
        }
    }
    void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (SceneManager.GetActiveScene().name == "level1")
        {
            if (playerHasBall)
            {
                status = 2;
            }
            if ((player.GetScore() > 50) & (player.GetScore() <= 100) & (playerHasBall == false))
            {
                status = 1;
            }
            if ((player.GetScore() > 100) & (player.GetScore() <= 350) & (playerHasBall == false))
            {
                status = 3;
            }
            if ((player.GetScore() > 350) & (player.GetScore() <= 700) & (playerHasBall == false))
            {
                status = 4;
            }
            if ((player.GetScore() > 700))
            {
                status = 5;
                SceneManager.LoadScene("level2");
            }
        }
        if (SceneManager.GetActiveScene().name == "level2")
        {
            if (playerHasBall)
            {
                status = 2;
            }
            if ((player.GetScore() > 50) & (player.GetScore() <= 100) & (playerHasBall == false))
            {
                status = 1;
            }
            if ((player.GetScore() > 100) & (player.GetScore() <= 350) & (playerHasBall == false))
            {
                status = 3;
            }
            if ((player.GetScore() > 350) & (player.GetScore() <= 700) & (playerHasBall == false))
            {
                status = 4;
            }
            if ((player.GetScore() > 700))
            {
                status = 5;
                npcOpponent.enabled = true;
                npcOpponent.GetComponent<MeshRenderer>().enabled = true;
                npcOpponent.GetComponent<Collider>().enabled = true;
            }
        }
    }
    void OnGUI()
    {
        if (SceneManager.GetActiveScene().name == "level1")
        {
            if (status == 0)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "You'll have to send the ball at the wall like that");
            }
            if (status == 1)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "Catch the ball by clicking on Rightclick");
            }
            if (status == 2)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "Aim at the target by moving laterally and release Rightclick to throw the ball");
            }
            if (status == 3)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "Keep on throwing the ball at the target");
            }
            if (status == 4)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "A few ones and you'll go to the next level");
            }
        }
        if (SceneManager.GetActiveScene().name == "level2")
        {
            if (status == 0)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "You'll have to send the ball to your teammate like that");
            }
            if (status == 1)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "Catch the ball by clicking on Rightclick");
            }
            if (status == 2)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "Aim by moving laterally and release Rightclick to throw the ball to your teammate");
            }
            if (status == 3)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "Keep on exchanging with your partner");
            }
            if (status == 4)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "A few ones and another friend will join you");
            }
            if (status == 5)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "Pass the ball to your teammate, but avoid your blue opponent to get it");
            }
            if (status == 6)
            {
                GUI.TextArea(new Rect(150, 0, 400, 20), "A few ones and another friend will join you");
            }
        }
    }

}
