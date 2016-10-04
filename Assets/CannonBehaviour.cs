using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CannonBehaviour : MonoBehaviour {

    private Game gameController;
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

    public float initialLife = 100f;
    public float currentLife = 100f;

    private float _playerSpeed = 5f;

    private float _playerInput;
    private Vector2 _currentPosition;
    private Transform _transform;

    private Transform body;
    private Transform wheel;

    // How far from the center of the ship should the bomb be
    public float bombDistance = .8f;

    // How much time (in seconds) we should wait before we can fire again
    public float timeBetweenFires = .5f;
    // If value is less than or equal 0, we can fire
    private float timeTilNextFire = 0.0f;

    // Use this for initialization
    void Start() {
        gameController = GameObject.FindObjectOfType<Game>();
        audioSource = GetComponent<AudioSource>();
        _transform = GetComponent<Transform>();
        body = _transform.FindChild("Body");
        wheel = _transform.FindChild("Wheel");
    }

    // Update is called once per frame
    void Update() {

        if (gameController.isGameOver()) {
            /*
             * If the game is over, the cannon cannot do anything
             */
            return;
        }

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
            if (Input.GetKeyDown(element) && timeTilNextFire < 0) {
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

        this._currentPosition = this._transform.position;
        this._playerInput = Input.GetAxis("Horizontal");

        if (this._playerInput > 0) {
            this._currentPosition += new Vector2(Time.deltaTime * _playerSpeed, 0);
        }

        if (this._playerInput < 0) {
            this._currentPosition -= new Vector2(Time.deltaTime * _playerSpeed, 0);
        }

        this._transform.position = new Vector2(Mathf.Clamp(this._currentPosition.x, -7.358f, -5.66f), this._currentPosition.y);
    }
}
