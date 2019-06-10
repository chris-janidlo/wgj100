using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Board
{
    public const int BoardSideLength = 6;
    public const int MinMatchingBlocks = 4;

    // origin at bottom left
    public Block[,] State = new Block[BoardSideLength, BoardSideLength];

    public void Slide (BoardInput input)
    {
        switch (input)
        {
            case BoardInput.SlideUp:
                slideBoardUp();
                break;

            case BoardInput.SlideDown:
                slideBoardDown();
                break;

            case BoardInput.SlideLeft:
                slideBoardLeft();
                break;

            case BoardInput.SlideRight:
                slideBoardRight();
                break;

            default:
                throw new Exception($"unexpected input value {input}");
        }

        clearMatches();
    }

    // TODO: animation
    void clearMatches ()
    {
        foreach (var matchSet in getMatches())
        {
            foreach (var pos in matchSet)
            {
                State[pos.x, pos.y] = null;
            }
        }
    }

    // TODO: animation
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

    List<List<Vector2Int>> getMatches ()
    {
        List<Vector2Int> visited = new List<Vector2Int>();
        List<List<Vector2Int>> matches = new List<List<Vector2Int>>();
        
        for (int x = 0; x <= BoardSideLength; x++)
        {
            for (int y = 0; y <= BoardSideLength; x++)
            {
                if (State[x, y] == null) continue;

                Vector2Int position = new Vector2Int(x, y);
                List<Vector2Int> match = new List<Vector2Int>();

                getAdjacentColors(position, match, visited);

                if (match.Count >= MinMatchingBlocks)
                {
                    matches.Add(match);
                }
            }
        }

        return matches;
    }

    void getAdjacentColors (Vector2Int position, List<Vector2Int> match, List<Vector2Int> visited)
    {
        visited.Add(position);
        match.Add(position);

        BlockType currentType = State[position.x, position.y].Type;

        List<Vector2Int> adjacentPositions = new List<Vector2Int>
        {
            new Vector2Int(position.x, position.y + 1),
            new Vector2Int(position.x, position.y - 1),
            new Vector2Int(position.x - 1, position.y),
            new Vector2Int(position.x + 1, position.y)
        };

        foreach (var pos in adjacentPositions)
        {
            if (visited.Contains(pos)) continue;

            Block toCheck = State[pos.x, pos.y];
            if (toCheck != null && toCheck.Type == currentType)
            {
                getAdjacentColors(pos, match, visited);
            }
        }
    }
}

public enum BoardInput
{
    SlideUp, SlideDown, SlideLeft, SlideRight
}