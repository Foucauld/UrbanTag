using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageDeFin : MonoBehaviour {

    public Text message;
    int i=0;
	void Start ()
    {
        if(GameState.score==0)
        message.text = "Vous etes mauvais.";
        else
        {
            foreach (playerInfo e in GameState.players)
            {
                message.text += "Vous avez touche le joueur " +e.name +" " + e.score+ " fois.\n\n";
                i += e.score;
            }
            i = GameState.score - i;
            if( i!=0)
            {
                message.text += "Vous avez touche le joueur unknown " + i + " fois.";
            }
        }
	}
	
}
