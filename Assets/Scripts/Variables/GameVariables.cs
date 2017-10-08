using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameVariables {


    // Number of puzzle rows
    public static int MaxRows = 4;

    // Number of puzzle columns
    public static int MaxColumns = 4;

    // Total number of individual puzzle pieces
    public static int MaxSize = MaxRows * MaxColumns;



}

/// <summary>
/// Possible game states
/// </summary>
public enum GameState
{
    Playing,
    Animating,
    End
}
