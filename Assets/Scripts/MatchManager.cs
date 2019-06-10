using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public Board Board { get; private set; }
    public float SlideDelay;

    void Awake ()
    {
        Board = new Board();
        StartCoroutine(loopRoutine());
    }

    IEnumerator loopRoutine ()
    {
        while (true)
        {
            foreach (BoardInput inputDir in Enum.GetValues(typeof(BoardInput)))
            {
                if (Input.GetButtonDown(inputDir.ToString()))
                {
                    Board.Slide(inputDir);
                    yield return new WaitForSeconds(SlideDelay);
                }
            }
            yield return null;
        }
    }
}
