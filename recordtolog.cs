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
using System.IO;
using System.Collections;
using UnityEngine;

public class recordtolog : MonoBehaviour
{
    int session;
    public Ball ballObject;
    public Player playerObject;
    public NPC npcObject;
    public NPC npcOpponentObject;
    public EventManager eventManager;
    public MenuManager menuManager;
    public string directory;
    string strNPC;
    string strPlayer;
    string strBall;
    string strNPCOpponent;

    public string GetTimestamp(DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssffff");
    }
  
    void Start()
    {
        directory =Application.dataPath; // we initialize the .exe path
        session = 1;
    }
    string f(Vector4 vec)
    {
        return vec.x.ToString()+" "+vec.w.ToString();
    }
    IEnumerator OnTriggerEnter(Collider col) // we activate it when it enters the trigger
    {
        yield return new WaitForFixedUpdate();
        if (eventManager.GetStatus()>0)
        {
            playerObject.SetScore(playerObject.GetScore()-50);
        }
        if (enabled == true)
        {
            npcObject.retard = 10;
            if (npcObject.gamemode > 0)
            {
                npcOpponentObject.retard = 10;
            }
            int a = 0;
            string timeStamp = GetTimestamp(DateTime.Now);
            ballObject.SetMove(false);
            ballObject.SetAway(true);
            strNPC = "Position of NPC (end session at" + timeStamp + " session has lasted: " + (playerObject.stopwatch.ElapsedMilliseconds / 1000).ToString() + ") : /r/n"; // wwe create the header string
            strPlayer = "Position of Player (end session at" + timeStamp + " session has lasted: " + (playerObject.stopwatch.ElapsedMilliseconds / 1000).ToString() + ") : /r/n";
            strBall = "Position of Ball (end session at" + timeStamp + " session has lasted: " + (playerObject.stopwatch.ElapsedMilliseconds / 1000).ToString() + ") : /r/n";
            strNPCOpponent = "Position of NPC opponent (end session at" + timeStamp + " session has lasted: " + (playerObject.stopwatch.ElapsedMilliseconds / 1000).ToString() + ") : /r/n";
            playerObject.stopwatch.Reset(); // we stop the time and reset it
            playerObject.stopwatch.Start();
            var posplayer1 = (playerObject.GetList().ConvertAll<string>(f)); // we convert the vector4 list to a list of string having the format f
            posplayer1.Insert(0, strPlayer);
            var posplayer = posplayer1.ToArray(); // we convert it into an array for faster write output
            File.WriteAllLines(directory + @"\log_pos_player_" + session.ToString() + GetTimestamp(DateTime.Now) + ".txt", posplayer); // we write it in the .txt log
            if (npcObject.gamemode > 0)
            {
                var posnpc1 = (npcObject.GetList().ConvertAll<string>(f));
                posnpc1.Insert(0, strNPC);
                var posnpc = posnpc1.ToArray();
                File.WriteAllLines(directory + @"\log_pos_npc_" + session.ToString() + GetTimestamp(DateTime.Now) + ".txt", posnpc);
                if (npcObject.gamemode > 1)
                {
                    var posnpcopp1 = (npcOpponentObject.GetList().ConvertAll<string>(f));
                    posnpcopp1.Insert(0, strNPCOpponent);
                    var posnpcopp = posnpcopp1.ToArray();
                    File.WriteAllLines(directory + @"\log_pos_npcopp_" + session.ToString() + GetTimestamp(DateTime.Now) + ".txt", posnpcopp);
                    npcObject.CutList(0, npcObject.GetList().Count - 16);
                }
                npcOpponentObject.CutList(0, npcOpponentObject.GetList().Count - 16);
            }
            var posball1 = (ballObject.GetList().ConvertAll<string>(f));
            posball1.Insert(0, strBall);
            var posball = posball1.ToArray();
            File.WriteAllLines(directory + @"\log_pos_ball_" + session.ToString() + GetTimestamp(DateTime.Now) + ".txt", posball);
            session = session + 1;
            ballObject.CutList(0, ballObject.GetList().Count - 16);
            playerObject.CutList(0, playerObject.GetList().Count - 16);
            ballObject.z0 = playerObject.transform.position.z;
            ballObject.x0 = playerObject.transform.position.x;
            ballObject.timePhase = 0;
            ballObject.angle = 0;
            playerObject.projectedPosition.x = playerObject.GetPosition().x;
            npcObject.onStopPosition = npcObject.GetPosition();
            npcObject.SetDistance((playerObject.projectedPosition - npcObject.GetPosition()).x / 2);

            ballObject.SetPosition(new Vector3(playerObject.transform.position.x, ballObject.y0 + 0.5f, ballObject.z0 + 0.5f));
            ballObject.transform.Translate(ballObject.GetPosition() - ballObject.transform.position);
            ballObject.AddPosition(ballObject.GetPosition());
            yield return new WaitForSeconds(5);
            ballObject.SetPosition(new Vector3(playerObject.GetPosition().x, ballObject.y0, ballObject.z0));
            ballObject.transform.Translate(ballObject.GetPosition() - ballObject.transform.position);
            ballObject.AddPosition(ballObject.GetPosition());
            ballObject.SetMove(false);
            yield return new WaitForSeconds(3);
            npcObject.retard = 250;
            if (npcObject.gamemode > 0)
            {
                npcOpponentObject.retard = 250;
            }
        }
    }


}
