using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPuzzlerController : MonoBehaviour {

    // Start the game once the player has selected a puzzle
    public void SelectPuzzle()
    {

        // Get the name of which puzzle game object was clicked on
        // Split() breaks the string with a space delimiter, so Puzzle 1 becomes [Puzzle] and [1]
        string[] name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name.Split();

        // Now retrieve the puzzle number from the second element in the new array
        int index = int.Parse(name[1]);

        Debug.Log("Player selected Puzzle number " + index);

        // Selecting the puzzle starts the game
        SceneManager.LoadScene("Gameplay");

    }

    // Navigation button
    public void NavToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }




}
