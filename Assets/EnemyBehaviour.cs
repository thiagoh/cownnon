using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    private Game gameController;
    // What sound to play when hit
    public AudioClip hitSound;

    // How fast will the enemy move
    public float initialSpeed = 6.0f;
    public float currentSpeed;
    private float lifeLostOnHit = 6.0f;

    // create an AudioSource variable
    private AudioSource audioSource;
    private Vector2 angularDirection;
    private Transform _transform;
    private CannonBehaviour cannonController;

    // Use this for initialization
    void Start() {
        _transform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.FindObjectOfType<Game>();
        cannonController = GameObject.FindObjectOfType<CannonBehaviour>();
        resetSpeed();
    }

    // Update is called once per frame
    void Update() {

        if (angularDirection.magnitude > 0) {

            float windSpeed = gameController.windSpeed * gameController.windDirection;
            _transform.Translate(angularDirection * Time.deltaTime * (currentSpeed + windSpeed), Space.World);
        }

        currentSpeed -= 3 * Time.deltaTime;
    }

    private void resetSpeed() {

        currentSpeed = initialSpeed;
        angularDirection = (Vector2.left * Random.Range(0.8f, 1.2f)) + (Vector2.up * Random.Range(0.8f, 1.2f));

        if (gameController.isGameOver()) {
            /*
             * If the game is over, the enemies are going to keep jumping up, 
             * but not towwards left anymore, to produce a funny scene!
             */
            angularDirection = Vector3.up;
        }
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
