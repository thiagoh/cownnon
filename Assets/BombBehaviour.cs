using UnityEngine;
using System.Collections;

public class BombBehaviour : MonoBehaviour {

    // How long the bomb will live
    public float lifetime = 2.0f;
    // How fast will the bomb move
    public float speed = 5.0f;
    // How much damage will this bomb do if we hit an enemy
    public int damage = 1;

    public int life = 3;

    // Use this for initialization
    void Start() {
        // The game object that contains this component will be
        // destroyed after lifetime seconds have passed
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update() {

        float rotationAngle = transform.localEulerAngles.z;

        float x = Mathf.Cos(rotationAngle * Mathf.Deg2Rad);
        float y = Mathf.Sin(rotationAngle * Mathf.Deg2Rad);

        Vector3 movement = new Vector3(x, y, 0);
        movement.Normalize();

        if (movement.magnitude > 0) {
            transform.Translate(movement * Time.deltaTime * speed, Space.World);
        }

        speed -= 3 * Time.deltaTime;

        if (speed < 0) {
            speed = 0;
        }

        timespan += Time.deltaTime;

        if (timespan > 0.5f) {
            float angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(1, 0, 0));
            Debug.LogWarning("angle: " + angle + "speed:" + speed);
            timespan = 0.0f;
        }

        if (life <= 0) {
            Destroy(gameObject);
        }
    }

    private float timespan = 0;
}
