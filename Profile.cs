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
using System.Collections.Generic;

public class Profile : MonoBehaviour
{
    public recordtolog recorder;
    private string userName;
    private int lastLevel;
    public string controlType;
    private bool handPreferenceIsRight;
    private int preferedTexture;
    private int preferedColor;
    public int[] customColor;
    // retrieve the control type
    public string GetControlType()
    {
        return controlType;
    }
    // change the control type
    public void SetControlType(string control)
    {
        controlType = control;
    }
    // retrieve the hand preference
    public bool GetHand()
    {
        return handPreferenceIsRight;
    }
    // change the hand preference
    public void SetHandPreference(bool right)
    {
        handPreferenceIsRight=right;
    }
    // retrieve the texture
    public int GetTexture()
    {
        return preferedTexture;
    }
    // change the prefered texture
    public void SetPreferedTexture(int index)
    {
        preferedTexture=index;
    }
    // change the prefered color parameter in the profile
    public void SetPreferedColor(int index)
    {
        preferedColor = index;
    }
    // define the function to change the custom color
    public void SetCustomColor(Color32 color)
    {
        customColor[0] = color.r;
        customColor[1] = color.g;
        customColor[2] = color.b;
    }
    // Get the custom color vector
    public int[] GetCustomColor()
    {
        return customColor;
    }
    // Get the color preference
    public int GetPreferedColor()
    {
        return preferedColor;
    }
    // Function to load the profile
    public void LoadProfile()
    {
        if (System.IO.File.Exists(recorder.directory + @"\profile_default.txt"))
        {
            string[] profile = System.IO.File.ReadAllLines(recorder.directory + @"\profile_default.txt");
            userName = profile[0];
            lastLevel = Convert.ToInt32(profile[1]);
            controlType = profile[2];
            handPreferenceIsRight = Convert.ToBoolean(profile[3]);
            preferedTexture = Convert.ToInt32(profile[4]);
            preferedColor = Convert.ToInt32(profile[5]);
            customColor = new int[] { Convert.ToInt32(profile[6]), Convert.ToInt32(profile[7]), Convert.ToInt32(profile[8]) };
        }
        else
        {
            userName = "default";
            lastLevel = 0;
            controlType = "Keyboard+Mouse";
            handPreferenceIsRight = true;
            preferedTexture = 0;
            preferedColor = 0;
            customColor = new int[] { 150, 150,150 };
            WriteProfile();
        }


    }
    public void WriteProfile()
    {
        string[] text = new string[] { userName, lastLevel.ToString(), controlType,
                handPreferenceIsRight.ToString(), preferedTexture.ToString() , preferedColor.ToString(),
                customColor[0].ToString(),customColor[1].ToString(), customColor[2].ToString() };
        System.IO.File.WriteAllLines(recorder.directory+ @"\profile_default.txt", text);
    }
    void Start()
    {
        
        LoadProfile();
    }
}
