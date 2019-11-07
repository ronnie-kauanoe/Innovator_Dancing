using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrail : MonoBehaviour {

    public Wand wand;
    public GameObject projectile;
    public float projectileForce = 1f;
    private Transform wandTransform;
    private GameObject permanentTrail;

    // Use this for initialization
    void Start () {
        wandTransform = CC_CANOE.WandTransform(wand);
        permanentTrail = Instantiate(projectile, projectile.transform.position, projectile.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
        projectile.transform.position = new Vector3(wandTransform.position.x,
            wandTransform.position.y,
            wandTransform.position.z);
        permanentTrail.transform.position = projectile.transform.position;

        if (CC_INPUT.GetButtonDown(wand, WandButton.Trigger)) {
            GameObject tempProjectile = Instantiate(projectile);
            tempProjectile.AddComponent<DestroyProjectile>();
            tempProjectile.GetComponent<Rigidbody>().isKinematic = false;
            tempProjectile.GetComponent<Rigidbody>().AddForce(CC_CANOE.WandGameObject(wand).transform.forward * projectileForce);
        }
    }
}
