using UnityEngine;
using UnityEngine.UI;

public class EndMessage : MonoBehaviour {

    public Text finalScore;
    public Text[] playerEndMessages;
    public Text unknownPlayerMessage;
    int scoreDelta;

	void Start ()
    {

        finalScore.text = "Total score : " + GameState.totalScore;
        finalScore.gameObject.SetActive(true);

        for (int i = 0; i < GameState.players.Count; i++)
        {
            PlayerInfo currentPlayer = GameState.players[i];
            playerEndMessages[i].text = "You hit the player " + currentPlayer.name + " " + currentPlayer.hit + " times.";
            playerEndMessages[i].gameObject.SetActive(true);
            scoreDelta += currentPlayer.hit;
        }
        scoreDelta = GameState.totalScore - scoreDelta;
        if ( scoreDelta != 0 )
        {
            unknownPlayerMessage.text = "You hit an unknown player " + scoreDelta + " times.";
            unknownPlayerMessage.gameObject.SetActive(true);
        }
	}
	
}
