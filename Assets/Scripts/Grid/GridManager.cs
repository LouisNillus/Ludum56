using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance = null;

    [Header("Debug")]
    public bool _generateOnStart = false;

    [Header("Levels")]
    public List<Level> _levels = new List<Level>();
    public int _currentLevelIndex = 0;

    [Header("Grid")]
    [SerializeField] private float _cellSpacing = 0;
    [SerializeField] private Cell _cellTemplate = null;
    private Cell[,] _cells = null;
    private bool _gridIsBusy = false;


    public Level CurrentLevel => _currentLevelIndex < _levels.Count ? _levels[_currentLevelIndex] : _levels.First();
    public int LevelsCount => _levels.Count;


    public UnityEvent<int> OnLevelGenerated { get; private set; } = new();

    public IEnumerator GenerateGridWithDelay()
    {
        _gridIsBusy = true;

        _cells = new Cell[CurrentLevel.Width, CurrentLevel.Height];

        for (int y_index = 0; y_index < CurrentLevel.Height; y_index++)
        {
            for (int x_index = 0; x_index < CurrentLevel.Width; x_index++)
            {
                Cell cell = Instantiate(_cellTemplate, new Vector2((_cellTemplate.Size + _cellSpacing) * x_index, (_cellTemplate.Size + _cellSpacing) * y_index), Quaternion.identity);

                cell.transform.localScale = Vector3.zero;
                cell.Grow.Play(1f);

                yield return new WaitForSeconds(0.15f);

                cell.Coordinates = new(x_index, y_index);
                cell.gameObject.name = $"Cell_{x_index}-{y_index}";

                _cells[x_index, y_index] = cell;
            }
        }

        _gridIsBusy = false;
    }

    public void GenerateGrid()
    {
        OnLevelGenerated.Invoke(_currentLevelIndex);

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

    public List<Cell> GetDiagonalCells(
        Cell origin,
        int distance
        )
    {
        List<Cell> diagonal_cells = new();

        int[,] diagonalOffsets = new int[,]
        {
        { distance, distance },
        { distance, -distance },
        { -distance, distance },
        { -distance, -distance }
        };

        for (int i = 0; i < 4; i++)
        {
            int adjacent_x = origin.Coordinates.x + diagonalOffsets[i, 0];
            int adjacent_y = origin.Coordinates.y + diagonalOffsets[i, 1];

            if (adjacent_x >= 0 && adjacent_x < CurrentLevel.Width && adjacent_y >= 0 && adjacent_y < CurrentLevel.Height)
            {
                diagonal_cells.Add(_cells[adjacent_x, adjacent_y]);
            }
        }

        return diagonal_cells;
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

                    if (cell.Head.EmotionalState == EmotionalState.Angry) //If one condition leads to Angry state, no need to check the other ones.
                    {
                        break;
                    }
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
        if (_gridIsBusy)
        {
            return;
        }

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
        if (_gridIsBusy)
        {
            yield break;
        }

        _gridIsBusy = true;

        _currentLevelIndex++;

        if (_currentLevelIndex >= _levels.Count)
        {
            _currentLevelIndex = 0;
        }


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

            _cells = new Cell[0, 0];
        }

        PlayerPrefs.SetInt("LevelIndex", _currentLevelIndex);

        yield return new WaitForSeconds(0.5f);

        GenerateGrid();

        _gridIsBusy = false;
    }

    private void Awake()
    {
        Instance = this;
        _currentLevelIndex = PlayerPrefs.HasKey("LevelIndex") ? PlayerPrefs.GetInt("LevelIndex") : 0;
    }

    private void Start()
    {
        if (_generateOnStart)
        {
            GenerateGrid();
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(CompleteLevel());
        }
    }
}
