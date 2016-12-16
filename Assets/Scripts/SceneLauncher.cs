using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLauncher : MonoBehaviour
{
    private Button button;
    public string SceneToLaunchOnButtonClick;

	void Start ()
	{
	    button = GetComponent<Button>();
	    button.onClick.AddListener(Launch);
	}

	private void Launch () {
	    if (button != null && SceneToLaunchOnButtonClick != null && SceneToLaunchOnButtonClick != "")
	    {
	        SceneManager.LoadScene(SceneToLaunchOnButtonClick);
	    }
	}
}
