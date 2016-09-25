using UnityEngine;
using System.Collections;
using System;

public class CannonBehaviour : MonoBehaviour {

    // Movement modifier applied to directional movement.
    public float playerSpeed = 4.0f;
    // What the current speed of our player is
    private float currentSpeed = 0.0f;
    // The last movement that we've made
    private Vector3 lastMovement = new Vector3();

    // The ball cannon will shoot
    public Transform laser;

    public float maxAngle = 70.0f;
    public float minAngle = 10.0f;

    // How much time (in seconds) we should wait before
    // we can fire again
    //public float timeBetweenFires = .3f;
    // If value is less than or equal 0, we can fire
    private float timeTilNextFire = 0.0f;
    // Reference to our AudioSource component
    private AudioSource audioSource;

    public Camera mainCamera;

    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        // Move the cannon's body
        movement();

    }

    public float factor;

    private void movement() {
        // The movement that needs to occur this frame
        Vector3 groundMovement = new Vector3();
        // Check for input
        groundMovement.x += Input.GetAxis("Horizontal");
        Vector3 angleMovement = new Vector3();

        if (Input.GetKey("w")) {
            angleMovement.z =  (Time.deltaTime * factor);
        } else if (Input.GetKey("s")) {
            angleMovement.z = -1 * (Time.deltaTime * factor);
        }

        groundMovement.Normalize();

        Transform body = transform.FindChild("Body");
        Transform wheel = transform.FindChild("Wheel");

        // Check if we pressed anything
        if (groundMovement.magnitude > 0) {
            // If we did, move in that direction
            currentSpeed = playerSpeed;
            transform.Translate(groundMovement * Time.deltaTime *
                playerSpeed, Space.World);
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

        /*
		* Get the differences from each axis (stands for
		* deltaX and deltaY)
		*/
        Quaternion rotation = body.GetComponent<HingeJoint2D>().transform.rotation;
        float currentAngle = Quaternion.Angle(rotation, Quaternion.Euler(1, 0, 0));
        Debug.LogWarning(" currentAngle: " + currentAngle + " angleMovement.z: " + angleMovement.z);

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
        Debug.LogWarning(" currentAngle: " + currentAngle);

    }
}
