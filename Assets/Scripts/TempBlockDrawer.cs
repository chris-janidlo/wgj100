using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempBlockDrawer : MonoBehaviour
{
    public MatchManager MatchManager;
    public List<RectTransform> Cells;
    public Image ArtBlockPrefab, GameplayBlockPrefab, MusicBlockPrefab, StoryBlockPrefab;

    Board board => MatchManager.Board;

    void Start ()
    {
        board.StateChanged.AddListener(drawBoard);
        drawBoard();
    }

    void drawBoard ()
    {
        for (int x = 0; x < Board.BoardSideLength; x++)
        {
            for (int y = 0; y < Board.BoardSideLength; y++)
            {
                Block block = board.State[x, y];
                RectTransform cell = Cells[x + y * Board.BoardSideLength];

                if (block != null)
                {
                    Image prefab = null;

                    switch (block.Type)
                    {
                        case BlockType.Art:
                            prefab = ArtBlockPrefab;
                            break;
                        case BlockType.Gameplay:
                            prefab = GameplayBlockPrefab;
                            break;
                        case BlockType.Music:
                            prefab = MusicBlockPrefab;
                            break;
                        case BlockType.Story:
                            prefab = StoryBlockPrefab;
                            break;
                    }

                    Instantiate(prefab).transform.SetParent(cell, false);
                }
                else if (cell.childCount > 0)
                {
                    Destroy(cell.GetChild(0).gameObject);
                }
            }
        }
    }
}
