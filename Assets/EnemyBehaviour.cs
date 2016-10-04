using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    // What sound to play when hit
    public AudioClip hitSound;

    // How fast will the enemy move
    public float startSpeed = 5.0f;

    public float speed;

    private float lifeLostOnHit = 6.0f;

    // create an AudioSource variable
    private AudioSource audioSource;
    private Vector3 movement;

    private CannonBehaviour cannonController;

    // Use this for initialization
    void Start() {

        audioSource = GetComponent<AudioSource>();
        cannonController = GameObject.FindObjectOfType<CannonBehaviour>();

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

        if (cannonController.isGameOver()) {
            /*
             * If the game is over, the enemies are going to keep jumping up, 
             * but not towwards left anymore, to produce a funny scene!
             */
            movement = (Vector3.up * Random.Range(0.3f, 0.9f));
        }

        movement.Normalize();
    }

    void OnCollisionEnter2D(Collision2D theCollision) {

        //Debug.LogWarning("hit with " + theCollision.gameObject.tag);

        if (theCollision.gameObject.CompareTag("Bomb")) {

            BombBehaviour bomb = theCollision.gameObject.GetComponent<BombBehaviour>();
            bomb.life -= 1;


            // Plays a sound from this object's AudioSource
            audioSource.PlayOneShot(hitSound, 1.0f);

            //Destroy(theCollision.gameObject);
            Destroy(gameObject, hitSound.length / 4.0f);

        } else if (theCollision.gameObject.CompareTag("Cannon")) {

            float initialLife = cannonController.initialLife;
            float currentLife = cannonController.currentLife;
            float lostLifeSize = (initialLife - currentLife) * 360;

            cannonController.currentLife = Mathf.Clamp(cannonController.currentLife - lifeLostOnHit, 0, initialLife);
            //Debug.Log("life lost: " + currentLife);
        }

        if (theCollision.gameObject.name.StartsWith("Ground")) {
            resetSpeed();
        }
    }
}
