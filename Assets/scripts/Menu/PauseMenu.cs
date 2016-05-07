using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //called when the player hits the return to game button
    public void returnToGame() {
        //close the pause menu
        PlayerController.instance.pauseMenu(false);
    }

    //called when the player hits the return to main menu button
    public void returnToMainMenu() {
        //load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
