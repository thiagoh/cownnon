using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public float timeScale;

    public Transform enemy;
    public bool _gameOver = false;
    private int currentNumberOfEnemies = 0;
    public float windSpeed;
    public float windDirection;
    public float timeBetweenEnemies;
    public Image gameOverImage;
    public float minTimeBetweenEnemies;
    // Reference to our AudioSource component
    private AudioSource audioSource;
    public AudioClip gameOverSound;
    private CannonBehaviour cannonController;

    // Use this for initialization
    void Start() {

        _gameOver = false;
        gameOverImage.enabled = false;
        cannonController = GameObject.FindObjectOfType<CannonBehaviour>();
        audioSource = GetComponent<AudioSource>();

        windDirection = 1;
        windSpeed = 0.1f;

        Time.timeScale = timeScale;
        print("Starting " + Time.time);

        StartCoroutine(WaitAndSpawnEnemies(2.0f));
        print("Before WaitAndSpawnEnemies Finishes " + Time.time);
    }

    IEnumerator WaitAndSpawnEnemies(float waitTime) {

        yield return new WaitForSeconds(waitTime);

        while (!isGameOver()) {
            // We want the enemies to be off screen
            float randDistance = Random.Range(-1.8f, 2.0f);

            Vector3 enemyPos = new Vector3();

            // Using the distance and direction we set the position
            enemyPos.x = 8.0f;
            enemyPos.y = randDistance;

            // Spawn the enemy and increment the number of enemies spawned
            Instantiate(enemy, enemyPos, transform.rotation);
            currentNumberOfEnemies++;

            if (currentNumberOfEnemies % 7 == 0) {
                timeBetweenEnemies *= 0.9f;
            }

            if (timeBetweenEnemies < minTimeBetweenEnemies) {
                timeBetweenEnemies = minTimeBetweenEnemies;
            }

            // How much time to wait before checking if we need to spawn another wave
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    // Update is called once per frame
    void Update() {
        checkGameOver();
    }

    private void checkGameOver() {

        if (_gameOver) {
            return;
        }

        if (cannonController.currentLife <= 0) {
            Debug.Log("GAME OVER!!");

            audioSource.PlayOneShot(gameOverSound, 1.0f);
            gameOverImage.enabled = true;
            _gameOver = true;
        }
    }

    public bool isGameOver() {
        return _gameOver;
    }
}
