using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    // What sound to play when hit
    public AudioClip hitSound;

    // How fast will the enemy move
    public float startSpeed = 5.0f;

    public float speed;

    // create an AudioSource variable
    private AudioSource audioSource;

    private Vector3 movement;

    //private Game controller;

    // Use this for initialization
    void Start() {

        audioSource = GetComponent<AudioSource>();
        //controller = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();

        resetSpeed();
    }

    // Update is called once per frame
    void Update() {

        if (movement.magnitude > 0) {
            transform.Translate(movement * Time.deltaTime * speed, Space.World);
        }

        speed -= 3 * Time.deltaTime;
    }

    private void resetSpeed() {
        speed = startSpeed;
        movement = (Vector3.left * Random.Range(0.2f, 0.6f)) + (Vector3.up * Random.Range(0.3f, 0.9f));
        movement.Normalize();
    }

    void OnCollisionEnter2D(Collision2D theCollision) {

        if (theCollision.gameObject.name.StartsWith("bomb")) {

            BombBehaviour bomb = theCollision.gameObject.GetComponent<BombBehaviour>();
            bomb.life -= 1;

            // Plays a sound from this object's AudioSource
            audioSource.PlayOneShot(hitSound, 1.0f);

            //Destroy(theCollision.gameObject);
            Destroy(gameObject, hitSound.length / 4.0f);
        }

        if (theCollision.gameObject.name.StartsWith("Ground")) {
            resetSpeed();
        }
    }
}
