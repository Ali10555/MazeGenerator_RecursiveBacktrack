using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int RownCol = 10;
    public float CellSize = 10;
    public bool HighlightVisited = true;
    public List<CELL> GirdCells = new List<CELL>();


    Stack<int> GirdCellsStack = new Stack<int>();

    int current;

    
    void Start()
    {
        Setup();
    }

    [ContextMenu ("Create Grid")]
    public void Setup()
    {
        GirdCells = new List<CELL>();

        for (int j = 0; j < RownCol; j++)
        {
            for (int i = 0; i < RownCol; i++)
            {
                CELL cell = new CELL(i, j);
                GirdCells.Add(cell);
            }
        }

        current = 0;

        GirdCells[current].top = false;
        GirdCells[current].right = false;
        GirdCells[current].bottom = false;
        GirdCells[current].left = false;
    }

    private void Update()
    {
        DrawGridDebug();
        RecursiveBacktrack();
    }

    void DrawGridDebug()
    {
        //Showing Grid
        for (int i = 0; i < GirdCells.Count; i++)
        {
            GirdCells[i].ShowLine(CellSize);

            if (HighlightVisited)
                GirdCells[i].Highlight(CellSize);
        }
    }

    void RecursiveBacktrack()
    {
        //Visiting
        GirdCells[current].isVisited = true;

        int next = CheckNeighors(current);

        if (IsCorrectIndex(next))
        {
            GirdCells[next].isVisited = true;

            GirdCellsStack.Push(current);

            RemoveWall(current, next);

            current = next;
        }
        else if (GirdCellsStack.Count > 0)
        {
            current = GirdCellsStack.Pop();
        }
    }

    public int CheckNeighors(int Index)
    {
        List<int> neighbors =new List<int>();

        CELL c = GirdCells[Index];

        int top = GetIndex(c.i, c.j - 1);
        int right = GetIndex(c.i + 1, c.j);
        int bottom = GetIndex(c.i, c.j + 1);
        int left = GetIndex(c.i -1 , c.j);

        //Top
        if (IsCorrectIndex(top))
        {
            if (!GirdCells[top].isVisited)
            {
                neighbors.Add(top);
            }
        }

        //Right
        if (IsCorrectIndex(right))
        {
            if (!GirdCells[right].isVisited)
            {
                neighbors.Add(right);
            }
        }

        //Bottom
        if (IsCorrectIndex(bottom))
        {
            if (!GirdCells[bottom].isVisited)
            {
                neighbors.Add(bottom);
            }
        }

        //Left
        if (IsCorrectIndex(left))
        {
            if (!GirdCells[left].isVisited)
            {
                neighbors.Add(left);
            }
        }

        // Seleting one of the neighbors
        if (neighbors.Count > 0)
        {
            int r = neighbors[Random.Range(0, neighbors.Count)];
            return r;
        }
        else
        {
            return -1;
        }
    }

    void RemoveWall(int _current,int _next)
    {
        int diff_i = GirdCells[_current].i - GirdCells[_next].i;
        int diff_j = GirdCells[_current].j - GirdCells[_next].j;

        if (diff_i == 1)
        {
            GirdCells[_current].left = false;
            GirdCells[_next].right = false;
        }
        else if(diff_i == -1)
        {
            GirdCells[_current].right = false;
            GirdCells[_next].left = false;
        }
        else if(diff_j == 1)
        {
            GirdCells[_current].top = false;
            GirdCells[_next].bottom = false;
        }
        else if (diff_j == -1)
        {
            GirdCells[_current].bottom = false;
            GirdCells[_next].top = false;
        }
    }

    int GetIndex(int i , int j)
    {
        if(i< 0 || j < 0 || i>RownCol-1 || j > RownCol-1)
            return -1;

        return i + j * RownCol;
    }

    bool IsCorrectIndex(int _index)
    {
        if (_index >= 0 && _index < GirdCells.Count)
            return true;
        else
            return false;
    }

}

[System.Serializable]
public class CELL
{
    public int i;
    public int j;

    bool _isVisited = false;
    public bool isVisited { get { return _isVisited; } set { _isVisited = value; } }

    public bool top = true;
    public bool right = true;
    public bool bottom = true;
    public bool left = true;

    
    public CELL(int i, int j)
    {
        this.i = i;
        this.j = j;
    }

    public void ShowLine(float CellSize)
    {
        float x = i * CellSize;
        float z = j * CellSize;

        Vector3 pos1 = new Vector3(x, 0, z); // Left Top 

        Vector3 pos2 = new Vector3(x + CellSize, 0, z); // Right Top 

        Vector3 pos3 = new Vector3(x, 0, z + CellSize); // Left Bottom

        Vector3 pos4 = new Vector3(x + CellSize, 0, z + CellSize); // Right Bottom

        if (left)
            Debug.DrawLine(pos1, pos3, Color.red); //Left line

        if (top)
            Debug.DrawLine(pos1, pos2, Color.red); //Top line

        if (right)
            Debug.DrawLine(pos2, pos4, Color.red); //Right line

        if (bottom)
            Debug.DrawLine(pos3, pos4, Color.red); //Bottom line

    }

    public void Highlight(float CellSize)
    {
        float x = i * CellSize;
        float z = j * CellSize;

        Vector3 pos1 = new Vector3(x, 0, z); // Left Top 

        Vector3 pos2 = new Vector3(x + CellSize, 0, z); // Right Top 

        Vector3 pos3 = new Vector3(x, 0, z + CellSize); // Left Bottom

        Vector3 pos4 = new Vector3(x + CellSize, 0, z + CellSize); // Right Bottom

        if (isVisited)
        {
            Debug.DrawLine(pos1, pos4, Color.green);
            Debug.DrawLine(pos2, pos3, Color.green);
        }
    }
}



