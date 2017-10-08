using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    // Load the puzzle selection scene
    public void NavToSelectPuzzle()
    {
        SceneManager.LoadScene("SelectPuzzleMenu");
    }


}
