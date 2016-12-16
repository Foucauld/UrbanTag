using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class reloadGun : MonoBehaviour {

	public void reload()
    {
        SceneManager.LoadScene("WebCamTextureMarkerBasedARSample");
    }
}
