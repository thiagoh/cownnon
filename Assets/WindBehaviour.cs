using UnityEngine;
using System.Collections;
/*
 * Name: Thiago de Andrade Souza
 * Date: 10/03/2016
 * Source file name: WindBehaviour.cs
 * Last Modified Date: 23:03 10/03/2016
 * Created Date: 23:03 09/29/2016
 */
public class WindBehaviour : MonoBehaviour {

    private Game _gameController;
    private float _lastChange;
    private float _lastCheck;
    private Transform _transform;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    public AudioClip windDirectionChange;

    // Use this for initialization
    void Start() {
        _audioSource = GetComponent<AudioSource>();
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameController = GameObject.FindObjectOfType<Game>();
        _lastChange = 0f;
        _lastCheck = 0f;
        _spriteRenderer.flipX = _gameController.windDirection == -1;
    }

    // Update is called once per frame
    void Update() {

        if (_gameController.isGameOver()) {
            return;
        }

        _lastChange += Time.deltaTime;
        _lastCheck += Time.deltaTime;

        // each 3s check if the wind should change the direction
        if (_lastCheck > 3) {

            // there must be at least 4s from last change
            if (_lastChange > 4 && Random.Range(0f, 1f) >= 0.5) {

                _gameController.windDirection = _gameController.windDirection * -1;
                // the wind has speed
                _gameController.windSpeed = Random.Range(0f, 0.4f);

                // Plays a sound when wind changes direction
                _audioSource.PlayOneShot(windDirectionChange, 1.0f);

                _lastChange = 0f;
                _spriteRenderer.flipX = _gameController.windDirection == -1;
            }

            _lastCheck = 0;
        }
    }
}
