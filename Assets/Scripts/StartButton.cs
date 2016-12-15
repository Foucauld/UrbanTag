using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

 
    [System.Serializable]
    public struct player
    {
        public UnityEngine.UI.Text pseudo;
        public UnityEngine.UI.Text color;
    }
    public UnityEngine.UI.Slider slider;
    public List<PlayerInfo> playerList = new List<PlayerInfo>();
    public UnityEngine.UI.Text gameTime;
    public player[] players;

    public void startButton()
    {
        PlayerInfo tmp;
        tmp.hit = 0;
        for(int i = 0; i < slider.value; i++)
        {
            tmp.playerColor = players[i].color.text.ToLower();
            tmp.name = players[i].pseudo.text;
            playerList.Add(tmp);
        }

        GameState.init(playerList, float.Parse(gameTime.text));
        SceneManager.LoadScene("GameScene");
    }
}
