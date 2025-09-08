using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int rows = 0;
    public int clos = 0;
    public float squaresGap = 0.1f;
    public GameObject GridSquare;
    public Vector2 startPos = new Vector2(0.0f, 0.0f);  
    public float squareScale = 0.5f;
    public float squareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();


    void Start()
    {
        CreatGrid();
    }

    private void CreatGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPos();

    }

    private void SpawnGridSquares()
    {
        int square_index = 0;
        for (var row = 0; row < rows; ++row)
        {
            for (var col = 0; col < clos; ++col)
            {
                _gridSquares.Add(Instantiate(GridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(square_index % 2 == 0);
                square_index++;

            }
        }
    }

    private void SetGridSquaresPos()
    {
        int col_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_move = false;
        var square_rect = _gridSquares[0].GetComponent<RectTransform>();
        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + squareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + squareOffset;

        foreach (GameObject square in _gridSquares)
        {   
            if (col_number + 1 > clos)
            {
                square_gap_number.x = 0;
                col_number = 0;
                row_number++;
                row_move = false;
            }

            var pos_x_offset = _offset.x * col_number + (square_gap_number.x * squaresGap);
            var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaresGap);

            if (col_number > 0 && col_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;
            }
            if (row_number > 0 && row_number % 3 == 0 && row_move == false)
            {
                row_move = true;
                square_gap_number.y++;
                pos_x_offset += squaresGap;
            }
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + pos_x_offset, startPos.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPos.x + pos_x_offset, startPos.y - pos_y_offset, 0.0f);
            col_number++;
        }

    }
}
