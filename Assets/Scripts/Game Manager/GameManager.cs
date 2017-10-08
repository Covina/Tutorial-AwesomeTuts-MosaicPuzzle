using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    // in individual game objects
    private GameObject[] puzzlePieces;

    // the picture for the puzzle pieces
    private Sprite[] puzzleImages;

    // Use viewport coordinates (0,1)
    private Vector3 screenPositionToAnimate;
    
    // The piece to animate
    private PuzzlePiece pieceToAnimate;

    // In which row and column is the piece located
    private int rowToAnimate, columnToAnimate;

    // Speed of piece movement
    private float animationSpeed = 10f;

    //
    private int puzzleIndex;

    // Specify game state
    private GameState gameState;

	// init variables
	void Awake () {
        // make singleton of GameManager
        MakeSingleton();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Make the Singleton of GameManager
    private void MakeSingleton()
    {

        if(instance != null)
        {
            // destroy dupe game object
            Destroy(this);

        } else
        {
            // Create
            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }


    // Store which puzzle the player selected
    public void SetPuzzleIndex(int index)
    {
        this.puzzleIndex = index;
        Debug.Log("SetPuzzleIndex() :: Player selected Puzzle number [" + index + "]");
    }


} // GameManager
