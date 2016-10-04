using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
 * Name: Thiago de Andrade Souza
 * Date: 10/03/2016
 * Source file name: CannonBehaviour.cs
 * Last Modified Date: 23:03 10/03/2016
 * Created Date: 23:03 09/29/2016
 */
public class CannonBehaviour : MonoBehaviour {

    public float factor;
    // The ball cannon will shoot
    public Transform bomb;

    public float maxAngle = 70.0f;
    public float minAngle = 10.0f;

    public Camera mainCamera;

    // The buttons that we can use to shoot lasers
    public List<KeyCode> shootButton;
    // What sound to play when we're shooting
    public AudioClip shootSound;

    public float initialLife;
    public float currentLife;

    private float _playerSpeed = 5f;
    private Game _gameController;
    // Reference to our AudioSource component
    private AudioSource audioSource;
    private float _playerInput;
    private Vector2 _currentPosition;
    private Transform _transform;

    private Transform _body;
    private Transform _wheel;

    // How far from the center of the ship should the bomb be
    public float bombDistance = .8f;

    // How much time (in seconds) we should wait before we can fire again
    public float timeBetweenFires = .5f;
    // If value is less than or equal 0, we can fire
    private float timeTilNextFire = 0.0f;

    // Use this for initialization
    void Start() {
        _gameController = GameObject.FindObjectOfType<Game>();
        _transform = GetComponent<Transform>();
        _body = _transform.FindChild("Body");
        _wheel = _transform.FindChild("Wheel");
        initialLife = 100f;
        currentLife = 100f;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        if (_gameController.isGameOver()) {
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
        float rotationAngle = _body.localEulerAngles.z;

        // Calculate the position right in front of the ship's position laserDistance units away
        bombPos.x += Mathf.Cos(rotationAngle * Mathf.Deg2Rad) * bombDistance;
        bombPos.y += Mathf.Sin(rotationAngle * Mathf.Deg2Rad) * bombDistance;
        bombPos.z = _body.localEulerAngles.z;

        Instantiate(bomb, bombPos, _body.rotation);
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
        Quaternion rotation = _body.GetComponent<HingeJoint2D>().transform.rotation;
        float currentAngle = Quaternion.Angle(rotation, Quaternion.Euler(1, 0, 0));
        //Debug.LogWarning(" currentAngle: " + currentAngle + " angleMovement.z: " + angleMovement.z);

        if (Math.Abs(angleMovement.z) > 0.001f) {

            _body.GetComponent<HingeJoint2D>().transform.Rotate(0, 0, angleMovement.z);
            currentAngle = Quaternion.Angle(_body.GetComponent<HingeJoint2D>().transform.rotation, Quaternion.Euler(1, 0, 0));
        }

        if (currentAngle < minAngle) {
            _body.GetComponent<HingeJoint2D>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, minAngle));
        }
        if (currentAngle > maxAngle) {
            _body.GetComponent<HingeJoint2D>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, maxAngle));
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
