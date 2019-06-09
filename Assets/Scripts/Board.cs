using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Board
{
    public const int BoardSideLength = 6;

    // origin at bottom left
    public Block[,] State = new Block[BoardSideLength, BoardSideLength];

    public void Slide (BoardInput input)
    {
        switch (input)
        {
            case BoardInput.Up:
                slideBoardUp();
                break;

            case BoardInput.Down:
                slideBoardDown();
                break;

            case BoardInput.Left:
                slideBoardLeft();
                break;

            case BoardInput.Right:
                slideBoardRight();
                break;

            default:
                throw new Exception($"unexpected input value {input}");
        }
    }

    void move (Vector2Int from, Vector2Int to)
    {
        if (from == to) return;

        Block block = State[from.x, from.y];
        State[from.x, from.y] = null;
        State[to.x, to.y] = block;
    }

    // TODO: generalize
    void slideBoardUp ()
    {
        for (int y = BoardSideLength - 1; y >= 0; y--)
        {
            for (int x = 0; x <= BoardSideLength; x++)
            {
                Vector2Int pos = new Vector2Int(x, y), newPos = pos;
                
                while (newPos.y < BoardSideLength && State[newPos.x, newPos.y + 1] == null)
                {
                    newPos += Vector2Int.up;
                }

                move(pos, newPos);
            }
        }
    }

    // TODO: generalize
    void slideBoardDown ()
    {
        for (int y = 1; y <= BoardSideLength; y++)
        {
            for (int x = 0; x <= BoardSideLength; x++)
            {
                Vector2Int pos = new Vector2Int(x, y), newPos = pos;
                
                while (newPos.y > 0 && State[newPos.x, newPos.y - 1] == null)
                {
                    newPos += Vector2Int.down;
                }

                move(pos, newPos);
            }
        }
    }

    // TODO: generalize
    void slideBoardLeft ()
    {
        for (int x = 1; x <= BoardSideLength; x++)
        {
            for (int y = 0; y <= BoardSideLength; y++)
            {
                Vector2Int pos = new Vector2Int(y, x), newPos = pos;
                
                while (newPos.x > 0 && State[newPos.x - 1, newPos.y] == null)
                {
                    newPos += Vector2Int.left;
                }

                move(pos, newPos);
            }
        }
    }

    // TODO: generalize
    void slideBoardRight ()
    {
        for (int x = BoardSideLength - 1; x >= 0; x--)
        {
            for (int y = 0; y <= BoardSideLength; y++)
            {
                Vector2Int pos = new Vector2Int(y, x), newPos = pos;
                
                while (newPos.x < BoardSideLength && State[newPos.x + 1, newPos.y] == null)
                {
                    newPos += Vector2Int.right;
                }

                move(pos, newPos);
            }
        }
    }
}

public enum BoardInput
{
    Up, Down, Left, Right
}