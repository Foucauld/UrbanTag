using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class returnGame : MonoBehaviour {

	public void returninGame()
    {
        SceneManager.LoadScene("GameScene");
        GameState.shouldReload = true;
    }
}
