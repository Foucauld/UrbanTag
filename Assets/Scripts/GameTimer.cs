using UnityEngine;
using UnityEngine.UI;
using System;

public class GameTimer : MonoBehaviour
{
    
    public Text timerText;
   

    public GameObject toShowOnEndGame;
    public string SceneToLoadOnTimesUp;

	// Use this for initialization
	void Start ()
	{
        if (GameState.shouldInitTime)
        {
            GameState.remainingTime = GameState.remainingTimeInMinutes * 60;
            if (GameState.remainingTime < 60)
            {
                GameState.remainingTime = 10;
            }

            GameState.shouldInitTime = false;
        }

	}
	
	// Update is called once per frame
	void Update ()
	{
        GameState.remainingTime -= Time.deltaTime;
	    if (GameState.remainingTime > 0)
	    {
	        timerText.text = printTimer(GameState.remainingTime);
	    }
	    else if (GameState.remainingTime < 0 && GameState.remainingTime > -3)
	    {
	        toShowOnEndGame.SetActive(true);
	    }
	    else
	    {
	        toShowOnEndGame.SetActive(false);
	        Application.LoadLevel(SceneToLoadOnTimesUp);
	    }
	}

    string printTimer(float sec)
    {
        float r = Mathf.Round(sec % 60);
        float m = Mathf.Round((sec - r) / 60);
        if (r < 10 && r != 0)
        {
            return "Timer : " + m + ":0" + r;
        }
            return "Timer : " + m + ":" + r;
    }


}
