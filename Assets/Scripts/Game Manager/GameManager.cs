using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    // Array to store the individual puzzle pieces
    private GameObject[] puzzlePieces;

    // Array to store the individual puzzle piece Image sprites
    private Sprite[] puzzleImages;

    // Define the size of the two-dimensional array storing the puzzle layout
    private PuzzlePiece[,] Matrix = new PuzzlePiece[GameVariables.MaxRows, GameVariables.MaxColumns];

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
        // set the index
        this.puzzleIndex = index;
        Debug.Log("SetPuzzleIndex() :: Player selected Puzzle number [" + index + "]");
    }



    // Fires when this gameobject becomes enabled or active
    private void OnEnable()
    {
        // START :: Subscribe the delegate to the sceneLoaded event
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    // Fires when this gameobject becomes disabled or deactive (likely never since it's a singleton)
    private void OnDisable()
    {
        // STOP :: Unsubscribe the delegate
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    // Delegate subscription function to react when the Scene changes
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("OnLevelFinishedLoading() :: Scene loaded [" + scene.name + "]");

        // Check if the scene that was loaded was our Gameplay scene
        if(scene.name == "Gameplay")
        {
            // Verify a puzzle selection was made
            if(puzzleIndex > 0)
            {
                // Debug
                // Debug.Log("OnLevelFinishedLoading() :: puzzleIndex > 0");

                // Load the selected puzzle
                LoadPuzzle();

                //  start game
                GameStarted();
            }
        }

    }
    

    // Load the puzzle sprite image(s)
    private void LoadPuzzle()
    {
        // Populate the array :: Load the image from local Resources/ folder
        puzzleImages = Resources.LoadAll<Sprite>("Sprites/BG " + puzzleIndex);
        

        // Populate the Array :: Get the array of defined sprites for the puzzle from PuzzleHolder class
        puzzlePieces = GameObject.Find("PuzzleHolder").GetComponent<PuzzleHolder>().puzzlePieces;

        // Loop through the GameObjects
        for(int i = 0; i < puzzlePieces.Length; i++)
        {
            // assign the puzzle piece image to their game object
            puzzlePieces[i].GetComponent<SpriteRenderer>().sprite = puzzleImages[i];
        }

    }

    // Start the game by generating the shuffled puzzle
    private void GameStarted ()
    {

        // Generate random number to find which piece to deactivate
        int index = Random.Range(0, GameVariables.MaxSize);

        // Disable a single random puzzle piece to open the space to move puzzle
        puzzlePieces[index].SetActive(false);

        // Loop through rows 
        for (int row = 0; row < GameVariables.MaxRows; row++)
        {

            // loop through columns
            for (int column = 0; column < GameVariables.MaxColumns; column++)
            {
                // Generate array loop Index
                // (0 * 4 + 1) = 1
                // (2 * 3 + 1) = 7
                int loopIndex = row * GameVariables.MaxColumns + column;

                // check to make sure this puzzle piece is active (and not the radnomly deactivated one)
                if (puzzlePieces[loopIndex].activeInHierarchy)
                {
                    // Generate vector 3 coordinates
                    Vector3 point = GetScreenCoordinatesFromViewport(row, column);

                    // Place the puzzle piece
                    puzzlePieces[loopIndex].transform.position = point;

                    // Create a new puzzle piece object in the Matrix array
                    Matrix[row, column] = new PuzzlePiece();

                    // Assign the new puzzle piece the game object
                    Matrix[row, column].GameObject = puzzlePieces[loopIndex];

                    // Store original row and column placement data
                    Matrix[row, column].OriginalRow = row;
                    Matrix[row, column].OriginalColumn = column;


                } else
                {
                    // The missing piece location is set to null
                    Matrix[row, column] = null;
                }

            }

        }

        // Shuffle all the puzzle pieces
        Shuffle();

        // Now we're ready to start playing
        gameState = GameState.Playing;

    }

    // Generate the screen coordinates for the puzzle piece
    private Vector3 GetScreenCoordinatesFromViewport(int row, int column)
    {
        // generate V3 location in world space
        Vector3 point = Camera.main.ViewportToWorldPoint(new Vector3(0.184f * row, 1 - 0.188f * column, 0));

        // force z to zero in world space
        point.z = 0;

        return point;
    }


    // Shuffle the puzzle pieces
    private void Shuffle()
    {
        // Loop through each piece
        for (int row = 0; row < GameVariables.MaxRows; row++)
        {
            for (int column = 0; column < GameVariables.MaxColumns; column++)
            {
                // if its the missing one, skip it
                if (Matrix[row, column] == null)
                {
                    // skip
                    continue;
                }
                else
                {
                    // generate a new row position.
                    int randomRow = Random.Range(0, GameVariables.MaxRows);

                    // generate a new column position.
                    int randomColumn = Random.Range(0, GameVariables.MaxColumns);

                    // Swap this piece with the piece at the random row and column location
                    Swap(row, column, randomRow, randomColumn);

                }


            }
        }
    }

    /// <summary>
    /// Randomly swap the positions of two puzzle pieces
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="randomRow"></param>
    /// <param name="randomColumn"></param>
    private void Swap(int row, int column, int randomRow, int randomColumn)
    {
        
        // Store original piece data
        PuzzlePiece temp = Matrix[row, column];

        // Replace original piece with random piece
        Matrix[row, column] = Matrix[randomRow, randomColumn];

        // Move original piece to random piece location
        Matrix[randomRow, randomColumn] = temp;

        // If the position has a piece in it
        if(Matrix[row, column] != null) {

            // get the pieces position by convering vector space coordinates into a Vector 3.
            Matrix[row, column].GameObject.transform.position = GetScreenCoordinatesFromViewport(row, column);

            // Store the new row and column values since it has already been swapped
            Matrix[row, column].CurrentRow = row;
            Matrix[row, column].CurrentColumn = column;

        }

        // Set the position of by convering vector space coordinates into a Vector 3.
        Matrix[randomRow, randomColumn].GameObject.transform.position = GetScreenCoordinatesFromViewport(randomRow, randomColumn);

        // Store the new row and column values since it has already been swapped
        Matrix[randomRow, randomColumn].CurrentRow = randomRow;
        Matrix[randomRow, randomColumn].CurrentColumn = randomColumn;

    }


    // User tapping screen
    private void CheckInput()
    {
        // Did they tap they screen?
        if(Input.GetMouseButtonDown(0))
        {
            // Cast a ray to see what they hit on their touch
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Fire the ray and get all the hits
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // Did we hit 
            if(hit.collider != null)
            {
                // Name format: piece-0-1;  Split using dash.
                // [0] = piece, [1] = row number, [2] = column number
                string[] parts = hit.collider.gameObject.name.Split('-');

                // get the row and column numbers of what the player tapped
                int rowPart = int.Parse(parts[1]);      // row
                int columnPart = int.Parse(parts[2]);   // column

                // Set row and column to -1 so that it's clear when checking their value 
                // (helper value)
                int rowFound = -1;
                int columnFound = -1;

                // Loop through each row and column
                for(int row = 0; row < GameVariables.MaxRows; row++)
                {
                    // break out of loop if we found it
                    if(rowFound != -1)
                    {
                        break;
                    }

                    // Loop through the columns
                    for (int column = 0; column < GameVariables.MaxColumns; column++)
                    {
                        // if we already found the column, break the loop
                        if(columnFound != -1)
                        {
                            break;
                        }

                        // If we are looping over the blank space, go to next loop iteration.
                        if(Matrix[row, column] == null)
                        {
                            continue;
                        }

                        // If the row and column match what we tapped on, store the values of which one we tapped.
                        // This basically says that we successfully tapped on a valid puzzle piece.
                        if(Matrix[row, column].OriginalRow == rowPart && Matrix[row, column].OriginalRow == columnPart)
                        {
                            rowFound = row;
                            columnFound = column;
                        }

                    }

                }


            }
        }

    }


} // GameManager
