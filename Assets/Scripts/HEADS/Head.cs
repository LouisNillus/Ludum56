using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Head : MonoBehaviour
{
    [SerializeField] private HeadType _headType = HeadType.None;
    [SerializeField, ReadOnly] private EmotionalState _emotionalState = EmotionalState.Neutral;
    [SerializeField] private Wiggle _wiggle = null;
    [SerializeField] private Shake _shake = null;
    [SerializeField] private Shrink _shrink = null;
    [SerializeField] private SpriteRenderer _renderer = null;

    [SerializeField] private SerializedDictionary<EmotionalState, Sprite> _statesSprites = new();

    [SerializeField] private Cell _hoveredCell = null;
    public List<Rule> Rules = new List<Rule>();
    public List<Constraint> Constraints = new List<Constraint>();

    public AudioClip _select = null;
    public AudioClip _drop = null;

    public HeadType HeadType => _headType;
    public Cell LastCell { get; set; } = null;
    public Cell AssignedCell { get; set; } = null;
    public HeadStack ParentStack { get; set; } = null;
    public Wiggle Wiggle => _wiggle;

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

    private void OnMouseDown()
    {

        ApplyAllConstraintsOfType(ConstraintType.Selection, out bool should_constrain);

        if (should_constrain)
        {
            _shake.Play(1);

            return;
        }

        Cursor.visible = false;

        if (ParentStack != null)
        {
            ParentStack.Release();
            ParentStack = null;
        }

        _renderer.sortingOrder = 10;

        if (AssignedCell != null
            && AssignedCell.Head == this
            )
        {
            AssignedCell.Head = null;
        }

        EmotionalState = EmotionalState.Neutral;

        _wiggle.Play();
    }

    private void OnMouseDrag()
    {
        ApplyAllConstraintsOfType(ConstraintType.Selection, out bool should_constrain);

        if (should_constrain)
        {
            return;
        }

        this.transform.position = Vector3.Lerp(this.transform.position, Utilities.MousePosition2D(), 0.25f);

        TargetCell();
    }

    private void OnMouseUp()
    {
        Cursor.visible = true;

        ApplyAllConstraintsOfType(ConstraintType.Selection, out bool should_constrain);

        if (should_constrain)
        {
            return;
        }

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

    public void ApplyAllConstraintsOfType(
        ConstraintType constraint_type,
        out bool should_constrain
        )
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

                cell.Grow.Play();
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

    public void AssignCell(
        Cell cell
        )
    {
        if (cell == null)
        {
            Debug.Log("Target cell is null, head has been removed from board");

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
