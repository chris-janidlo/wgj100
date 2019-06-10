using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public Board Board { get; private set; }

    void Awake ()
    {
        Board = new Board();
    }

    void Update ()
    {
        foreach (BoardInput inputDir in Enum.GetValues(typeof(BoardInput)))
        {
            if (Input.GetButtonDown(inputDir.ToString()))
            {
                Board.Slide(inputDir);
            }
        }
    }
}
