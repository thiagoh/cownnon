using UnityEngine;
using System.Collections;

public class WindBehaviour : MonoBehaviour {

    private Game gameController;
    private float lastChange;
    private Transform _transform;
    private AudioSource audioSource;
    private SpriteRenderer _spriteRenderer;
    public AudioClip windDirectionChange;

    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindObjectOfType<Game>();
        lastChange = 0f;
    }

    // Update is called once per frame
    void Update() {

        if (gameController.isGameOver()) {
            return;
        }

        lastChange += Time.deltaTime;

        if (lastChange > 4 && Random.Range(0, 10) >= 9) {
            gameController.windDirection = gameController.windDirection * -1;
            gameController.windSpeed = Random.Range(0f, 0.2f);

            // Plays a sound when wind changes direction
            audioSource.PlayOneShot(windDirectionChange, 1.0f);

            lastChange = 0f;
        }

        _spriteRenderer.flipX = gameController.windDirection == -1;
    }
}
