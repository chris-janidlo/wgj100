using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempBlockDrawer : MonoBehaviour
{
    public CreationPhaseManager MatchManager;
    public List<RectTransform> Cells;

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

                if (cell.childCount > 0)
                {
                    Destroy(cell.GetChild(0).gameObject);
                }

                if (block != null)
                {
                    Image prefab = BlockPrefabRepo.Instance.GetPrefab(block.Type);

                    Instantiate(prefab).transform.SetParent(cell, false);
                }
            }
        }
    }
}
