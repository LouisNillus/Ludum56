using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Head : MonoBehaviour
{
    [SerializeField] private HeadData _headData = null;
    [SerializeField, ReadOnly] private EmotionalState _emotionalState = EmotionalState.Neutral;
    [SerializeField] private Wiggle _wiggle = null;
    [SerializeField] private Shake _shake = null;
    [SerializeField] private Shrink _shrink = null;
    [SerializeField] private SpriteRenderer _renderer = null;

    [SerializeField] private SerializedDictionary<EmotionalState, Sprite> _statesSprites = new();

    [SerializeField] private Cell _hoveredCell = null;
    public List<Rule> Rules = new List<Rule>();
    public List<Constraint> Constraints = new List<Constraint>();

    public HeadType HeadType => _headData.Type;
    public Cell LastCell { get; set; } = null;
    public Cell AssignedCell { get; set; } = null;
    public HeadStack ParentStack { get; set; } = null;
    public Wiggle Wiggle => _wiggle;

    private bool isDragging = false;

    public EmotionalState EmotionalState
    {
        get => _emotionalState;
        set
        {
            if (_emotionalState == value)
            {
                return;
            }

            _emotionalState = value;

            _renderer.sprite = _statesSprites[_emotionalState];
            OnEmotionalStateChanged.Invoke(_emotionalState);
        }
    }

    public UnityEvent<EmotionalState> OnEmotionalStateChanged { get; } = new();

    public void Initialize(HeadData data)
    {
        _headData = data;
    }

    private void Update()
    {
        if (isDragging)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, Utilities.MousePosition2D(), 0.25f);

            TargetCell();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            EndDrag();
        }
    }

    private void OnMouseDown()
    {
        if (!isDragging)
        {
            StartDrag();
        }
    }

    public void StartDrag()
    {
        ApplyAllConstraintsOfType(ConstraintType.Selection, out bool should_constrain);

        if (should_constrain)
        {
            _shake.Play(1);

            return;
        }

        isDragging = true;
        Cursor.visible = false;
        _renderer.sortingOrder = 10;

        if (AssignedCell != null && AssignedCell.Head == this)
        {
            AssignedCell.Head = null;
        }

        EmotionalState = EmotionalState.Neutral;
        _wiggle.Play();
    }

    private void EndDrag()
    {
        isDragging = false;
        Cursor.visible = true;
        _renderer.sortingOrder = 1;

        if (_hoveredCell != null)
        {
            AssignToHoveredCell();
        }
        else
        {
            SendBackToStack();
        }

        _wiggle.Stop();
    }

    public void ApplyAllConstraintsOfType(ConstraintType constraint_type, out bool should_constrain)
    {
        should_constrain = false;

        foreach (Constraint constraint in Constraints)
        {
            if (constraint.Perform(constraint_type, this))
            {
                should_constrain = true;
                return;
            }
        }
    }

    public void TargetCell()
    {
        if (GridManager.Instance.TryGetCell(this.transform.position, out Cell cell))
        {
            if (_hoveredCell == null)
            {
                _hoveredCell = cell;
                cell.Grow.Play();  // Animation de survol de cellule
            }
        }
        else if (_hoveredCell != null)
        {
            _hoveredCell.Grow.Stop();
            _hoveredCell = null;
        }
    }

    private void AssignToHoveredCell()
    {
        AssignCell(_hoveredCell);
        _hoveredCell.Grow.Stop();
        _hoveredCell = null;
    }

    public void AssignCell(Cell cell)
    {
        if (cell == null)
        {
            Debug.Log("La cellule cible est nulle, la tête a été retirée du plateau.");
            SendBackToStack();
            return;
        }

        LastCell = AssignedCell;
        AssignedCell = cell;
        AssignedCell.Populate(this);

        GridManager.Instance.RefreshGridEmotionalStates();
    }

    public void SendBackToStack()
    {
        HeadStackManager.Instance.StoreHead(this);
        GridManager.Instance.RefreshGridEmotionalStates();
    }

    public void Destroy()
    {
        _shrink.Play(() => Destroy(gameObject));
    }
}

public enum EmotionalState
{
    Neutral = 0,
    Happy = 1,
    Angry = 2
}
