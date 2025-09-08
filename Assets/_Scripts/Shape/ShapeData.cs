using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]

public class ShapeData : ScriptableObject
{
    [System.Serializable]

    public class Row
    {
        public bool[] col;
        private int _size = 0;

        public Row() { }

        public Row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            _size = size;
            col = new bool[_size];
            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < _size; ++i)
            {
                col[i] = false;
            }
        }

    }

    public int cols = 0;
    public int rows = 0;
    public Row[] board;

    public void clear()
    {
        if (board == null) return;

        for (var i = 0; i < rows; i++)
        {
            if (board[i] != null)
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new Row[rows];

        for (var i = 0; i < rows; i++)
        {
            board[i] = new Row(cols);
        }
    }
}
