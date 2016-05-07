using UnityEngine;
using System.Collections;
using System;

public class TestInteractable : Interactable {

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // is called when the object is interacted with.
    public override void OnInteract(GameObject interactor) {
        // lets just let the world know we where interacted with
        Debug.Log("Interact");
    }

}
