using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    public Text FinalScore;
	// Use this for initialization
	void Start () {
        FinalScore.text = "Score Final: "+ GameState.score;
	}
	
	
}
