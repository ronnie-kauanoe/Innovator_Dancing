using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour {

    public float sizeScale = 1f;
    public float expandTime = 2f;
    public float expireTime = 5f;
    private float spawnTime;
    private float launchTime;
    private Vector3 originalScale;

    [HideInInspector]
    public bool startExpiring = false;
    [HideInInspector]
    public bool expiring = false;

	// Use this for initialization
	void Start () {
        spawnTime = Time.time;
        originalScale = Vector3.one;
    }
	
	// Update is called once per frame
	void Update () {
		if ((Time.time - spawnTime) <= expandTime) {
            float scale = sizeScale * ((Time.time - spawnTime) / expandTime);
            gameObject.transform.localScale = new Vector3(originalScale.x * scale,
                originalScale.y * scale,
                originalScale.z * scale);
        }

        if (startExpiring) {
            launchTime = Time.time;
            expiring = true;
            startExpiring = false;
        }

        if (expiring) {
            if ((Time.time - launchTime) >= expireTime) {
                Destroy(gameObject);
            }
        }
	}
}
