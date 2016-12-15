using UnityEngine;
using UnityEngine.UI;
using System;

public class GameTimer : MonoBehaviour
{

    private float remainingTime;
    private float remainingAmmoTime;
    public Text timerText;
    private int ammoTimer = Mathf.RoundToInt(GameState.remainingTime * 20);
    public Crosshair crosshair;

    public GameObject toShowOnEndGame;

	// Use this for initialization
	void Start ()
	{
	    remainingTime = GameState.remainingTime * 60;
	    remainingAmmoTime = remainingTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    updateAmmoOnTime();
	    if (remainingTime > 0)
	    {
	        remainingTime -= Time.deltaTime;
	        timerText.text = printTimer(remainingTime);
	    }
	    else if (remainingTime < 0 && remainingTime > -3)
	    {
	        toShowOnEndGame.SetActive(true);
	    }
	    else
	    {
	        toShowOnEndGame.SetActive(false);
	        Application.LoadLevel("todo");
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

    void updateAmmoOnTime()
    {
        if (remainingTime < remainingAmmoTime - 30)
        {
            ammoTimer -= 1;
            if (crosshair.ammo <= 79)
            {
                crosshair.ammo += 20;
                crosshair.UpdateAmmoText();
            }
            remainingAmmoTime = Mathf.CeilToInt(remainingTime);
        }
    }

}
