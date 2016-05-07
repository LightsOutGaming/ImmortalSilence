using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour {

    // Called when an object is interacted with.
    public abstract void OnInteract(GameObject interactor);

}
