using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public float timeScale;

    // Use this for initialization
    void Start() {
        Time.timeScale = timeScale;
    }

    // Update is called once per frame
    void Update() {

    }
}
