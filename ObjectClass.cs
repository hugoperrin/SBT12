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
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectClass : MonoBehaviour
{
    public List<Vector4> positionList; // list of positions for recording purposes and for equations
    protected Vector3 position; // gives the position

    public Vector3 GetPosition()
    {
        return position;
    }
    public void SetPosition(Vector3 newValue)
    {
        position = newValue;
    }
    public void AddPosition(Vector4 Position)
    {
        if (positionList!= null)
        {
            positionList.Add(Position);
        }
    }
    public Vector4 GetPosition(int index)
    {
        if ((index >= 0) & (index < positionList.Count))
        {
            return positionList[index];
        }
        else
        {
            return positionList[positionList.Count-1];
        }
    }
    public List<Vector4> GetList()
    {
        return positionList;
    }
    public void CutList(int start, int end)
    {
        positionList.RemoveRange(start, end);
    }
    public void InitList()
    {
        positionList = new List<Vector4>();
    }
    // Get the position from k*deltaT before
    public Vector4 LastPosition(int k)
    {
        int j = 0;
        int i = 0;
        int lastIndex = positionList.Count;
        float lastTime = GetPosition(lastIndex).w;
        while (j!=k)
        {
            if (GetPosition(lastIndex-1-i).w==lastTime)
            {
                i = i + 1;
            }
            else
            {
                j = j + 1;
                lastTime = GetPosition(lastIndex - 1 - i).w;
            }
        }
        return GetPosition(lastIndex - 1 - i);
    }
}
