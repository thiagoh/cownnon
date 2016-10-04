using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Name: Thiago de Andrade Souza
 * Date: 10/03/2016
 * Source file name: Game.cs
 * Last Modified Date: 23:03 10/03/2016
 * Created Date: 23:03 09/29/2016
 */

public class Game : MonoBehaviour {

    public float timeScale;
    public Transform enemy;
    private bool _gameOver = false;
    public float windSpeed;
    public float windDirection;
    public float timeBetweenEnemies;
    public Image _gameOverImage;
    public float minTimeBetweenEnemies;
    public AudioClip gameOverSound;

    public float enemySpeedIncrease;
    private int _currentNumberOfEnemies = 0;
    // Reference to our AudioSource component
    private AudioSource _audioSource;
    private CannonBehaviour _cannonController;

    // Use this for initialization
    void Start() {

        _gameOver = false;
        _gameOverImage.enabled = false;
        _cannonController = GameObject.FindObjectOfType<CannonBehaviour>();
        _audioSource = GetComponent<AudioSource>();

        enemySpeedIncrease = 0f;
        windDirection = 1;
        windSpeed = 0.4f;

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
            _currentNumberOfEnemies++;

            if (_currentNumberOfEnemies % 7 == 0) {
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

        enemySpeedIncrease = Mathf.Clamp(enemySpeedIncrease + (Time.deltaTime / 250f), 0f, 0.4f);
        print("enemySpeedIncrease: " + enemySpeedIncrease);
    }

    private void checkGameOver() {

        if (isGameOver()) {
            return;
        }

        if (_cannonController.currentLife <= 0) {
            print("GAME OVER!!");

            _audioSource.PlayOneShot(gameOverSound, 1.0f);
            _gameOverImage.enabled = true;
            _gameOver = true;
        }
    }

    public bool isGameOver() {
        return _gameOver;
    }
}
