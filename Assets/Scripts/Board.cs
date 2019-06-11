using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using crass;

public class Board
{
    public const int BoardSideLength = 12;
    public const int MinMatchingBlocks = 4;

    public UnityEvent StateChanged;

    // origin at bottom left
    public Block[,] State = new Block[BoardSideLength, BoardSideLength];

    class BlockBag : BagRandomizer<BlockType> {}

    BlockBag bagOne, bagTwo;

    public Board ()
    {
        List<BlockType> blocks = new List<BlockType>();

        foreach (BlockType blockType in Enum.GetValues(typeof(BlockType)))
        {
            blocks.Add(blockType);
        }

        bagOne = new BlockBag();
        bagOne.Items = blocks;
        bagOne.AvoidRepeats = true;

        bagTwo = new BlockBag();
        bagTwo.Items = blocks;
        bagTwo.AvoidRepeats = true;

        spawnNewPieces();
        StateChanged = new UnityEvent();
    }

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
        spawnNewPieces();
        clearMatches();
        StateChanged.Invoke();
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

    void spawnNewPieces ()
    {
        // assuming board dimensions are even
        int lowerMid = BoardSideLength / 2 - 1, upperMid = lowerMid + 1;

        List<Vector2Int> spawnLocations = new List<Vector2Int>
        {
            new Vector2Int(lowerMid, lowerMid),
            new Vector2Int(lowerMid, upperMid),
            new Vector2Int(upperMid, upperMid),
            new Vector2Int(upperMid, lowerMid)
        };

        int firstSpawnIndex, diagonalIndex;
        Vector2Int spawnOne, diagonal, spawnTwo;

        firstSpawnIndex = UnityEngine.Random.Range(0, spawnLocations.Count);
        diagonalIndex = (firstSpawnIndex + 2) % 4;
        diagonal = spawnLocations[diagonalIndex];

        spawnOne = spawnLocations[firstSpawnIndex];
        spawnLocations.Remove(spawnOne);
        spawnLocations.Remove(diagonal);
        spawnTwo = spawnLocations.PickRandom();

        if (State[spawnOne.x, spawnOne.y] != null || State[spawnTwo.x, spawnTwo.y] != null)
        {
            // TODO: die
            Debug.Log("death");
            return;
        }

        State[spawnOne.x, spawnOne.y] = new Block(bagOne.GetNext());
        State[spawnTwo.x, spawnTwo.y] = new Block(bagTwo.GetNext());
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
            for (int x = 0; x < BoardSideLength; x++)
            {
                Vector2Int pos = new Vector2Int(x, y), newPos = pos;
                
                while (newPos.y < BoardSideLength - 1 && State[newPos.x, newPos.y + 1] == null)
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
        for (int y = 0; y < BoardSideLength; y++)
        {
            for (int x = 0; x < BoardSideLength; x++)
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
        for (int x = 0; x < BoardSideLength; x++)
        {
            for (int y = 0; y < BoardSideLength; y++)
            {
                Vector2Int pos = new Vector2Int(x, y), newPos = pos;
                
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
            for (int y = 0; y < BoardSideLength; y++)
            {
                Vector2Int pos = new Vector2Int(x, y), newPos = pos;
                
                while (newPos.x < BoardSideLength - 1 && State[newPos.x + 1, newPos.y] == null)
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
        
        for (int x = 0; x < BoardSideLength; x++)
        {
            for (int y = 0; y < BoardSideLength; y++)
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

        List<Vector2Int> adjacentPositions = new List<Vector2Int>();

        if (position.x > 0)
        {
            adjacentPositions.Add(new Vector2Int(position.x - 1, position.y));
        }

        if (position.y > 0)
        {
            adjacentPositions.Add(new Vector2Int(position.x, position.y - 1));
        }

        if (position.x < BoardSideLength - 1)
        {
            adjacentPositions.Add(new Vector2Int(position.x + 1, position.y));
        }

        if (position.y < BoardSideLength - 1)
        {
            adjacentPositions.Add(new Vector2Int(position.x, position.y + 1));
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