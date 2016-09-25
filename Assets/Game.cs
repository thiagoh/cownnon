using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public float timeScale;

    public Transform enemy;

    private int currentNumberOfEnemies = 0;

    public float timeBetweenEnemies;

    public float minTimeBetweenEnemies;

    // Use this for initialization
    void Start() {
        Time.timeScale = timeScale;
        print("Starting " + Time.time);
        StartCoroutine(WaitAndSpawnEnemies(2.0f));
        print("Before WaitAndSpawnEnemies Finishes " + Time.time);
    }

    IEnumerator WaitAndSpawnEnemies(float waitTime) {
        yield return new WaitForSeconds(waitTime);

        while (true) {
            // We want the enemies to be off screen
            float randDistance = Random.Range(-1.8f, 2.0f);

            Vector3 enemyPos = new Vector3();

            // Using the distance and direction we set the position
            enemyPos.x = 8.0f;
            enemyPos.y = randDistance;

            // Spawn the enemy and increment the number of enemies spawned
            Instantiate(enemy, enemyPos, transform.rotation);
            currentNumberOfEnemies++;

            if (currentNumberOfEnemies % 10 == 0) {
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

    }
}
