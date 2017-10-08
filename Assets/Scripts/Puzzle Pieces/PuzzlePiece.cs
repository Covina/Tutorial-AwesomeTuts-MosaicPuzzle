using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece {

    // Current location of puzzle piece
    public int CurrentRow { get; set; }
    public int CurrentColumn { get; set; }


    // Original locations of the puzzle piece
    // this is before it gets split up and shuffled
    public static int OriginalRow { get; set; }
    public static int OriginalColumn { get; set; }

    // This individual puzzle piece
    public static GameObject GameObject { get; set; }


}
