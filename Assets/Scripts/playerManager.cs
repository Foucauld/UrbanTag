using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerManager : MonoBehaviour {

    [System.Serializable]
    public struct player {
        public Text pseudo;
        public Text color;
    }

    public player[] players;
}
