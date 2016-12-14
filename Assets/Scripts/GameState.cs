using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {

    
    static public float remainingTime;
    static public List<playerInfo> players;

    static public void init(List<playerInfo> players0, float remainingTime0)
    {
        remainingTime = remainingTime0;
        players = players0;
    }

    public void appendName()
    {

    }

}
public struct playerInfo
{
    public int score;
    public string name;
    public string playerColor;
}
