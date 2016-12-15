using UnityEngine;
using UnityEngine.UI;
using UrbanTag;

public class Crosshair : MonoBehaviour
{
    public RawImage verticalPart;
    public RawImage horizontalPart;

    public MultiObjectTrackingBasedOnColor colorTracker;

    public float radius;

    Color white = Color.white;
    Color red = Color.red;
    private bool isInsideTarget;
    private string hitColor;

    private float screenHalfWidth = 320;
    private float screenHalfHeight = 240;

    private int score;
    public Text scoreText;
    public Text ammoText;

    public int ammo;

	void Start () {
	    score = 0;
	    scoreText.text = "Score : 0";
	    UpdateAmmoText();
	}

	void Update ()
	{
	    isInsideTarget = false;
	    SetCrosshairColor(white);

	    foreach (ColorObject target in colorTracker.GetTargets())
	    {
	        float targetDistance = (target.getXPos() - screenHalfWidth) * (target.getXPos() - screenHalfWidth) + (target.getYPos() - screenHalfHeight) * (target.getYPos() - screenHalfHeight);
	        if (targetDistance < radius * radius)
	        {
	            SetCrosshairColor(red);
                isInsideTarget = true;
	            hitColor = target.getType();
	        }
	    }
	}

    public void Fire()
    {
        if(ammo > 0){
            if (isInsideTarget)
            {
                score++;
                scoreText.text = "Score : " + score;
                GameState.increaseScore(hitColor);
            }
            ammo--;
            UpdateAmmoText();
        }

    }

    public void UpdateAmmoText()
    {
        ammoText.text = "Ammo : " + ammo;
    }

    private void SetCrosshairColor(Color color)
    {
        verticalPart.color = color;
        horizontalPart.color = color;
    }
}
