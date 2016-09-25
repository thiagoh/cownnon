using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    //private Game controller;

    // Use this for initialization
    void Start() {
        //controller = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnCollisionEnter2D(Collision2D theCollision) {

        if (theCollision.gameObject.name.StartsWith("bomb")) {

            BombBehaviour bomb = theCollision.gameObject.GetComponent<BombBehaviour>();
            bomb.life -= 1;

            //Destroy(theCollision.gameObject);
            Destroy(gameObject);

            // Plays a sound from this object's AudioSource
            //audio.PlayOneShot(hitSound, 1.0f);
        }

    }
}
