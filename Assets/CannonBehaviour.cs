using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CannonBehaviour : MonoBehaviour {

    // Movement modifier applied to directional movement.
    public float playerSpeed = 4.0f;
    // What the current speed of our player is
    private float currentSpeed = 0.0f;
    // The last movement that we've made
    private Vector3 lastMovement = new Vector3();
    public float factor;
    // The ball cannon will shoot
    public Transform bomb;

    public float maxAngle = 70.0f;
    public float minAngle = 10.0f;

    // Reference to our AudioSource component
    private AudioSource audioSource;

    public Camera mainCamera;

    // The buttons that we can use to shoot lasers
    public List<KeyCode> shootButton;
    // What sound to play when we're shooting
    public AudioClip shootSound;

    private Transform body;

    private Transform wheel;

    // How far from the center of the ship should the bomb be
    public float bombDistance = .8f;

    // How much time (in seconds) we should wait before we can fire again
    public float timeBetweenFires = .3f;
    // If value is less than or equal 0, we can fire
    private float timeTilNextFire = 0.0f;

    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
        body = transform.FindChild("Body");
        wheel = transform.FindChild("Wheel");
    }

    // Update is called once per frame
    void Update() {

        // Move the cannon's body
        movement();

        // Rotate the cannon's body
        rotation();

        checkShoot();
    }

    private void shoot() {

        audioSource.PlayOneShot(shootSound, 1.0f);

        // We want to position the bomb in relation to
        // our player's location
        Vector3 bombPos = transform.position;
        // The angle the bomb will move away from the center
        float rotationAngle = body.localEulerAngles.z;

        // Calculate the position right in front of the ship's position laserDistance units away
        bombPos.x += Mathf.Cos(rotationAngle * Mathf.Deg2Rad) * bombDistance;
        bombPos.y += Mathf.Sin(rotationAngle * Mathf.Deg2Rad) * bombDistance;
        bombPos.z = body.localEulerAngles.z;

        Instantiate(bomb, bombPos, body.rotation);
    }
    private void checkShoot() {

        foreach (KeyCode element in shootButton) {
            if (Input.GetKey(element) && timeTilNextFire < 0) {
                timeTilNextFire = timeBetweenFires;
                shoot();
                break;
            }
        }

        timeTilNextFire -= Time.deltaTime;
    }

    private void rotation() {

        Vector3 angleMovement = new Vector3();

        if (Input.GetKey("w")) {
            angleMovement.z = (Time.deltaTime * factor);
        } else if (Input.GetKey("s")) {
            angleMovement.z = -1 * (Time.deltaTime * factor);
        }

        /*
		* Get the differences from each axis (stands for
		* deltaX and deltaY)
		*/
        Quaternion rotation = body.GetComponent<HingeJoint2D>().transform.rotation;
        float currentAngle = Quaternion.Angle(rotation, Quaternion.Euler(1, 0, 0));
        //Debug.LogWarning(" currentAngle: " + currentAngle + " angleMovement.z: " + angleMovement.z);

        if (Math.Abs(angleMovement.z) > 0.001f) {

            body.GetComponent<HingeJoint2D>().transform.Rotate(0, 0, angleMovement.z);
            currentAngle = Quaternion.Angle(body.GetComponent<HingeJoint2D>().transform.rotation, Quaternion.Euler(1, 0, 0));
        }

        if (currentAngle < minAngle) {
            body.GetComponent<HingeJoint2D>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, minAngle));
        }
        if (currentAngle > maxAngle) {
            body.GetComponent<HingeJoint2D>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, maxAngle));
        }

        currentAngle = Quaternion.Angle(rotation, Quaternion.Euler(1, 0, 0));
        //Debug.LogWarning(" currentAngle: " + currentAngle);
    }

    private void movement() {
        // The movement that needs to occur this frame
        Vector3 groundMovement = new Vector3();
        // Check for input
        groundMovement.x += Input.GetAxis("Horizontal");

        groundMovement.Normalize();

        // Check if we pressed anything
        if (groundMovement.magnitude > 0) {
            // If we did, move in that direction
            currentSpeed = playerSpeed;
            transform.Translate(groundMovement * Time.deltaTime * playerSpeed, Space.World);
            lastMovement = groundMovement;

            //mainCamera.transform.Translate(groundMovement * Time.deltaTime * playerSpeed, Space.World);
        } else {
            // Otherwise, move in the direction we were going
            transform.Translate(lastMovement * Time.deltaTime *
                currentSpeed, Space.World);

            //mainCamera.transform.Translate(lastMovement * Time.deltaTime *
            //    currentSpeed, Space.World);

            // Slow down over time
            currentSpeed *= .9f;
        }
    }
}
