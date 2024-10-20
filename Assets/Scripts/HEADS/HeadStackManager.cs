using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HeadStackManager : MonoBehaviour
{
    public static HeadStackManager Instance = null;

    public HeadDataTable HeadDataTable = null;

    [SerializeField] private SerializedDictionary<HeadData, int> _headsMap = new();

    [SerializeField] private HeadStack _headsStackTemplate = null;
    [SerializeField] private GameObject _stacksContainer = null;

    public bool IsEmpty => !_headsMap.Values.Any(count => count > 0);

    public UnityEvent OnHeadsCountChanged { get; set; } = new();
    public UnityEvent OnLevelLoaded { get; set; } = new();

    private List<HeadStack> _currentLevelStacks = new List<HeadStack>();

    public void LoadLevel(
        Level level
        )
    {
        ClearStacks();

        _headsMap = new(level.HeadsMap);

        for (int i = 0; i < _headsMap.Count; i++)
        {
            HeadStack stack = Instantiate(_headsStackTemplate, _stacksContainer.transform);
            stack.HeadData = _headsMap.ElementAt(i).Key;

            _currentLevelStacks.Add(stack);
        }

        OnLevelLoaded.Invoke();
    }

    public void ClearStacks()
    {
        for (int stack_index = _currentLevelStacks.Count - 1; stack_index >= 0; stack_index--)
        {
            Destroy(_currentLevelStacks[stack_index].gameObject);
        }

        _currentLevelStacks.Clear();
    }

    public bool Has(
        HeadType head_type
        )
    {
        return _headsMap.TryGetValue(GetHeadData(head_type), out int count) && count > 0;
    }

    public int GetCount(
        HeadType head_type
        )
    {
        _headsMap.TryGetValue(GetHeadData(head_type), out int count);

        return count;
    }

    public HeadData GetHeadData(
        HeadType head_type
        )
    {
        return HeadDataTable.GetData(head_type);
    }

    public Head PickHead(
        HeadType head_type
        )
    {
        HeadData head_data = GetHeadData(head_type);

        if (GetCount(head_type) <= 0)
        {
            Debug.Log($"No more {head_type} head in the stack");

            return null;
        }

        _headsMap[head_data]--;

        OnHeadsCountChanged.Invoke();

        Head head = Instantiate(head_data.Object);
        head.Initialize(head_data);

        return head;
    }

    public void StoreHead(
        Head head
        )
    {
        _headsMap[GetHeadData(head.HeadType)]++;

        OnHeadsCountChanged.Invoke();

        Destroy(head.gameObject);
    }

    public void AddHead(
        HeadType head_type
        )
    {
        OnHeadsCountChanged.Invoke();

        _headsMap[GetHeadData(head_type)]++;
    }

    private void Awake()
    {
        Instance = this;
    }
}
