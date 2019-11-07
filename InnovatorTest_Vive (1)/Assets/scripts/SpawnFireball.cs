using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireball : MonoBehaviour {

    public Wand wandToFollow;
    public WandButton spawnButton;
    public GameObject fireball;
    public Vector3 positionOffset;
    public float launchForce;

    private Transform wandTransform;
    private GameObject fireballInstance;    

    // Use this for initialization
    void Start () {
        wandTransform = CC_CANOE.WandGameObject(wandToFollow).transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (CC_INPUT.GetButtonDown(wandToFollow, spawnButton)) {
            fireballInstance = Instantiate(fireball, null);
        }
        if (CC_INPUT.GetButtonPress(wandToFollow, spawnButton)) {
            fireballInstance.transform.position = new Vector3(wandTransform.position.x + positionOffset.x,
                wandTransform.position.y + positionOffset.y,
                wandTransform.position.z + positionOffset.z);
            Debug.Log(fireballInstance.transform.position);
        } 
        if (CC_INPUT.GetButtonUp(wandToFollow, spawnButton)) {
            fireballInstance.GetComponent<Rigidbody>().AddForce(wandTransform.forward * launchForce);
            //fireballInstance.GetComponent<DestroyFireball>().enabled = true;     
        }
	}
}
