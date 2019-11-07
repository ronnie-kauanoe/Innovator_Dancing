using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapItems : MonoBehaviour {

    public Wand wand;
    private int childIndex = 0;

	// Use this for initialization
	void Start () {
		for (int i = 1; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (CC_INPUT.GetButtonDown(wand, WandButton.Menu)) {
            transform.GetChild(childIndex).gameObject.SetActive(false);
            childIndex = (childIndex + 1) % transform.childCount;
            transform.GetChild(childIndex).gameObject.SetActive(true);
        }
    }
}
