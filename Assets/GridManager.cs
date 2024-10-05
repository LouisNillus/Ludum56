using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance = null;

    [SerializeField, Range(0, 10)] private int _height = 0;
    [SerializeField, Range(0, 10)] private int _width = 0;
    [SerializeField] private float _cellSpacing = 0;

    [SerializeField] private Cell _cellTemplate = null;

    private Cell[,] _cells = null;

    [Button]
    public void Generate()
    {
        _cells = new Cell[_width, _height];

        for (int y_index = 0; y_index < _height; y_index++)
        {
            for (int x_index = 0; x_index < _width; x_index++)
            {
                Cell cell = Instantiate(_cellTemplate, new Vector2((_cellTemplate.Size + _cellSpacing) * x_index, (_cellTemplate.Size + _cellSpacing) * y_index), Quaternion.identity);
                cell.Coordinates = new(x_index, y_index);
                cell.gameObject.name = $"Cell_{x_index}-{y_index}";

                _cells[x_index, y_index] = cell;
            }
        }
    }

    public bool TryGetCell(
        Vector3 position,
        out Cell found_cell
        )
    {
        found_cell = null;

        foreach (Cell cell in _cells)
        {
            if (cell.Contains(position))
            {
                found_cell = cell;

                return true;
            }
        }

        return false;
    }

    public Cell GetMouseCell()
    {
        foreach (Cell cell in _cells)
        {
            if (cell.Contains(Utilities.MousePosition2D()))
            {
                return cell;
            }
        }

        return null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cell mouse_cell = GetMouseCell();

            if (mouse_cell != null)
            {

                Debug.Log(IsSurrounded(mouse_cell));

            }
        }
    }

    public List<Cell> GetAdjacentCells(
        Cell origin,
        int distance,
        bool include_diagonals
        )
    {
        List<Cell> adjacent_cells = new();

        for (int y_offset = -distance; y_offset <= distance; y_offset++)
        {
            for (int x_offset = -distance; x_offset <= distance; x_offset++)
            {
                int adjacent_x = origin.Coordinates.x + x_offset;
                int adjacent_y = origin.Coordinates.y + y_offset;

                if (adjacent_x >= 0 && adjacent_x < _width && adjacent_y >= 0 && adjacent_y < _height)
                {
                    if (x_offset != 0 || y_offset != 0)
                    {
                        if (include_diagonals || x_offset == 0 || y_offset == 0)
                        {
                            adjacent_cells.Add(_cells[adjacent_x, adjacent_y]);
                        }
                    }
                }
            }
        }

        return adjacent_cells;
    }

    public void RefreshGridEmotionalStates()
    {
        foreach (Cell cell in _cells)
        {
            if (cell.Occupied)
            {
                foreach (Rule rule in cell.Head.Rules)
                {
                    rule.Verify(cell.Head);
                }
            }
        }
    }

    public bool IsSurrounded(
        Cell cell
        )
    {
        foreach (Cell adjacent_cell in GetAdjacentCells(cell, 1, include_diagonals: true))
        {
            if (!adjacent_cell.Occupied)
            {
                return false;
            }
        }

        return true;
    }

    public void ValidateGrid()
    {
        if (!HeadStackManager.Instance.IsEmpty)
        {
            Debug.Log("There are still some heads to place");

            return;
        }

        int mistakes_count = 0;
        mistakes_count++;


    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Generate();
    }
}
