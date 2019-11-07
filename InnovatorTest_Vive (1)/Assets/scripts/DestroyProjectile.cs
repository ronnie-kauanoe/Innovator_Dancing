using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectile : MonoBehaviour {

    private float duration;
    private float startTime;

	// Use this for initialization
	void Start () {
        duration = GetComponent<ParticleSystem>().main.duration;
        startTime = Time.time;
        Debug.Log(duration);
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time - startTime) >= duration) {
            Destroy(gameObject);
        }
	}
}
