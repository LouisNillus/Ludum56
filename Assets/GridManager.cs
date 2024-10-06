using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<Level> _levels = new List<Level>();
    public int _currentLevelIndex = 0;

    public static GridManager Instance = null;

    [SerializeField] private float _cellSpacing = 0;

    [SerializeField] private Cell _cellTemplate = null;

    private Cell[,] _cells = null;
    private Level CurrentLevel => _currentLevelIndex < _levels.Count ? _levels[_currentLevelIndex] : _levels.First();

    public static bool LockedControls { get; private set; } = false;

    public IEnumerator GenerateGridWithDelay()
    {
        LockedControls = true;

        _cells = new Cell[CurrentLevel.Width, CurrentLevel.Height];

        for (int y_index = 0; y_index < CurrentLevel.Height; y_index++)
        {
            for (int x_index = 0; x_index < CurrentLevel.Width; x_index++)
            {
                Cell cell = Instantiate(_cellTemplate, new Vector2((_cellTemplate.Size + _cellSpacing) * x_index, (_cellTemplate.Size + _cellSpacing) * y_index), Quaternion.identity);

                cell.transform.localScale = Vector3.zero;
                cell.Grow.Play();

                yield return new WaitForSeconds(0.15f);

                cell.Coordinates = new(x_index, y_index);
                cell.gameObject.name = $"Cell_{x_index}-{y_index}";

                _cells[x_index, y_index] = cell;
            }
        }

        LockedControls = false;
    }

    [Button]
    public void GenerateGrid()
    {
        StartCoroutine(GenerateGridWithDelay());

        HeadStackManager.Instance.LoadLevel(CurrentLevel);
    }

    public bool TryGetCell(
        Vector3 position,
        out Cell found_cell
        )
    {
        found_cell = null;

        foreach (Cell cell in _cells)
        {
            if (cell != null && cell.Contains(position))
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

                if (adjacent_x >= 0 && adjacent_x < CurrentLevel.Width && adjacent_y >= 0 && adjacent_y < CurrentLevel.Height)
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
            if (cell != null && cell.Occupied)
            {
                if (cell.Head.Rules.Count == 0)
                {
                    cell.Head.EmotionalState = EmotionalState.Happy;
                }

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

    public void ValidateGrid() // Called from UI button
    {
        int mistakes_count = 0;

        foreach (Cell cell in _cells)
        {
            if (cell.Occupied && cell.Head.EmotionalState == EmotionalState.Angry)
            {
                cell.Head.Wiggle.Play(1);

                mistakes_count++;
            }
        }

        if (!HeadStackManager.Instance.IsEmpty)
        {
            Debug.Log("There are still some heads to place");

            return;
        }

        if (mistakes_count == 0)
        {
            StartCoroutine(CompleteLevel());
        }
    }

    private IEnumerator CompleteLevel()
    {
        if (LockedControls)
        {
            yield break;
        }

        LockedControls = true;

        _currentLevelIndex++;

        if (_cells != null)
        {
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                for (int y = 0; y < _cells.GetLength(1); y++)
                {
                    Cell cell_to_destroy = _cells[x, y];

                    if (cell_to_destroy != null)
                    {
                        if (cell_to_destroy.Head != null)
                        {
                            cell_to_destroy.Head.Destroy();
                        }

                        yield return new WaitForSeconds(0.15f);

                        _cells[x, y].Destroy();
                    }
                }
            }

            _cells = null;
        }

        yield return new WaitForSeconds(0.5f);

        GenerateGrid();

        LockedControls = false;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();
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
}
