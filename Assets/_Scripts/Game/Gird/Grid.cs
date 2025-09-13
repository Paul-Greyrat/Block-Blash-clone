using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int rows = 0;
    public int cols = 0;
    public float squaresGap = 0.1f;
    public GameObject GridSquare;
    public Vector2 startPos = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float squareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();

    private LineIndicator _lineIndicator;

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }

    void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPos();

    }

    private void SpawnGridSquares()
    {
        // 0, 1, 2, 3, 4,
        // 5, 6, 7, 8, 9

        int square_index = 0;
        for (var row = 0; row < rows; ++row)
        {
            for (var col = 0; col < cols; ++col)
            {
                _gridSquares.Add(Instantiate(GridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(_lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);
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
            if (col_number + 1 > cols)
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

    private void CheckIfShapeCanBePlaced()
    {
        var squareIndex = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.Selected && !gridSquare.Squareoccupied)
            {
                squareIndex.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
                //gridSquare.ActivateSquare();
            }
        }

        var CurrentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (CurrentSelectedShape == null) return; // No shape selected
        if (CurrentSelectedShape.TotalSquaresNumber == squareIndex.Count)
        {
            foreach (var square_index in squareIndex)
            {
                _gridSquares[square_index].GetComponent<GridSquare>().PlaceShapeOnBroad();
            }


            var shapeLeft = 0;
            foreach (var shape in shapeStorage.shapeList)
            {
                if (shape.IsOnStrartPosition() && shape.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }

            if (shapeLeft == 0)
            {
                GameEvents.RequestNewAShape();
            }
            else
            {
                GameEvents.SetShapeInActive();
            }

            CheckIfLineIsComplete();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }
    }

    void CheckIfLineIsComplete()
    {
        List<int[]> lines = new List<int[]>();

        // column
        foreach (var col in _lineIndicator.columnIndex)
        {
            lines.Add(_lineIndicator.GetVerticalLine(col));
        }

        // row 
        for (int row = 0; row < rows; row++)
        {
            List<int> data = new List<int>(9);
            for (int index = 0; index < cols; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]);
            }
            lines.Add(data.ToArray());
        }

        //square
        for (var square = 0; square < 9; square++)
        {
            List<int> data = new List<int>(9);
            for (var index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.Square_data[square, index]);
            }
            lines.Add(data.ToArray());
        }

        var completedLines = CheckIfSquareIsComplete(lines);

        if (completedLines > 2)
        {
            //ToDo: Play bonus animation.
        }

        var totalScores = 10 * completedLines;
        GameEvents.AddScores(totalScores);
    }

    private int CheckIfSquareIsComplete(List<int[]> data)
    {
        HashSet<int> squaresToClear = new HashSet<int>();
        var linesCompleted = 0;
        foreach (var square in data)
        {
            var lineCompleted = true;
            foreach (var index in square)
            {
                var comp = _gridSquares[index].GetComponent<GridSquare>();
                if (comp.Squareoccupied == false)
                {
                    lineCompleted = false;
                    break;
                }
            }
            if (lineCompleted)
            {
                foreach (var index in square)
                {
                    squaresToClear.Add(index);
                }
                linesCompleted++;
            }
        }
        foreach (var squareIndex in squaresToClear)
        {
            var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
            comp.DeactivateSquare();
        }
        foreach (var squareIndex in squaresToClear)
        {
            var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
            comp.ClearOccupied();
        }
        return linesCompleted;
    }
}
