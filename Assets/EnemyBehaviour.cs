using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    // What sound to play when hit
    public AudioClip hitSound;

    // how left speed is going to increase
    private float _speedIncrease;
    // How fast will the enemy move
    private float _initialSpeed;
    private float _currentSpeed;
    private float _lifeLostOnHit;
    private float _lastFlip;
    // create an AudioSource variable
    private AudioSource audioSource;
    private SpriteRenderer _spriteRenderer;
    private Vector2 angularDirection;
    private Transform _transform;

    private Game _gameController;
    private CannonBehaviour _cannonController;

    // Use this for initialization
    void Start() {
        _transform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        _gameController = GameObject.FindObjectOfType<Game>();
        _cannonController = GameObject.FindObjectOfType<CannonBehaviour>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _resetSpeed();
        _lifeLostOnHit = 6.0f;
        _initialSpeed = 5.5f;
        _lastFlip = 0f;
        
    }

    // Update is called once per frame
    void Update() {

        if (_gameController.isGameOver()) {

            _lastFlip += Time.deltaTime;

            // Flip the enemies to make some fun!
            if (_lastFlip > 0.5f) {
                _spriteRenderer.flipY = !_spriteRenderer.flipY;
                _lastFlip = 0;
            }
        }

        if (angularDirection.magnitude > 0) {

            float _speed = Mathf.Clamp(_currentSpeed + (_gameController.windSpeed * _gameController.windDirection), 0, _initialSpeed);
            _transform.Translate(angularDirection * Time.deltaTime * _speed, Space.World);
        }

        _currentSpeed -= 3 * Time.deltaTime;
    }

    private void _resetSpeed() {

        _currentSpeed = _initialSpeed;
        float speedIncrease = _gameController.enemySpeedIncrease;
        angularDirection = (Vector2.left * (Random.Range(0.8f, 1.2f) + speedIncrease)) + (Vector2.up * (Random.Range(0.8f, 1.2f) + speedIncrease));

        if (_gameController.isGameOver()) {
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

            float initialLife = _cannonController.initialLife;
            float currentLife = _cannonController.currentLife;
            float lostLifeSize = (initialLife - currentLife) * 360;

            _cannonController.currentLife = Mathf.Clamp(_cannonController.currentLife - _lifeLostOnHit, 0, initialLife);
            //Debug.Log("life lost: " + currentLife);
        }

        if (theCollision.gameObject.name.StartsWith("Ground")) {
            _resetSpeed();
        }
    }
}
