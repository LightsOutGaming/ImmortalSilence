using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //called when the player hits the singleplayer button
    public void singleplayer() {
        //load the test scene
        SceneManager.LoadScene("TestScene");
    }

    //called when the player hits the multiplayer button
    public void multiplayer() {
        // TODO: implement multiplayer
    }

    //called when the player hits the quit button
    public void quit() {
        //close the application
        Application.Quit();
    }
}
