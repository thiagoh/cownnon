using UnityEngine;
using System.Collections;

public class LifebarBehaviour : MonoBehaviour {

    public Transform cannon;
    private CannonBehaviour cannonController;

    private Transform _transform;
    private Transform _lifeLostTransform;
    private Transform life;
    private Transform lifeLost;

    private float initialPos = 1.78f;
    private float finalPos = 0f;
    private float maxDistanceWalkedByBar;

    // Use this for initialization
    void Start() {
        cannonController = GameObject.FindObjectOfType<CannonBehaviour>();

        maxDistanceWalkedByBar = Mathf.Abs(finalPos - initialPos);

        _transform = GetComponent<Transform>();

        life = _transform.FindChild("life");
        lifeLost = _transform.FindChild("life-lost");

        _lifeLostTransform = lifeLost.transform;
        _lifeLostTransform.localPosition = new Vector2(initialPos, _lifeLostTransform.localPosition.y);
    }

    // Update is called once per frame
    void Update() {

        float initialLife = cannonController.initialLife;
        float currentLife = cannonController.currentLife;
        float factor = ((initialLife - currentLife) / initialLife);

        if (factor > 0f) {

            float lostLifeSize = factor * 35.55f;

            _lifeLostTransform.localScale = new Vector2(Mathf.Clamp(lostLifeSize, 0, 35.55f), 1);
            _lifeLostTransform.localPosition = new Vector2(Mathf.Clamp(initialPos - (factor * maxDistanceWalkedByBar), finalPos, initialPos), _lifeLostTransform.localPosition.y);

            //Debug.Log("lostLifeSize:" + lostLifeSize + ", factor: " + factor);
            //Debug.LogWarning("new pos:" + _lifeLostTransform.localPosition.x);
        }


    }
}
