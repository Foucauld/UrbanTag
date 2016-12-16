using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text FinalScore;

	void Start () {
        FinalScore.text = "Score Final: "+ GameState.totalScore;

	}
	
	
}
