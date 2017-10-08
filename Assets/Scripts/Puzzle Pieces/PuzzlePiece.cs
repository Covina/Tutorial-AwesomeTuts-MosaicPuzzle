using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece {

    // Current location of puzzle piece
    public int CurrentRow { get; set; }
    public int CurrentColumn { get; set; }


    // Original locations of the puzzle piece
    // this is before it gets split up and shuffled
    public int OriginalRow { get; set; }
    public int OriginalColumn { get; set; }

    // This individual puzzle piece
    public GameObject GameObject { get; set; }


}
