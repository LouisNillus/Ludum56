using AYellowpaper.SerializedCollections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HeadStackManager : MonoBehaviour
{
    public static HeadStackManager Instance = null;

    [SerializeField] private SerializedDictionary<HeadType, int> _headsStackMap = new();
    [SerializeField] private SerializedDictionary<HeadType, Head> _headsTemplateMap = new();

    public bool IsEmpty => !_headsStackMap.Values.Any(count => count > 0);

    public UnityEvent OnHeadsCountChanged { get; set; } = new();
    public UnityEvent OnLevelLoaded { get; set; } = new();

    public void LoadLevel(
        Level level
        )
    {
        _headsStackMap = new(level.HeadsStackMap);

        OnLevelLoaded.Invoke();
    }

    public bool Has(
        HeadType head_type
        )
    {
        return _headsStackMap.ContainsKey(head_type) && _headsStackMap[head_type] > 0;
    }

    public int GetCount(
        HeadType head_type
        )
    {
        return _headsStackMap.ContainsKey(head_type) ? _headsStackMap[head_type] : 0;
    }

    public Head PickHead(
        HeadType head_type
        )
    {
        if (_headsStackMap[head_type] <= 0)
        {
            Debug.Log($"No more {head_type} head in the stack");

            return null;
        }

        _headsStackMap[head_type]--;

        OnHeadsCountChanged.Invoke();

        Head head = Instantiate(_headsTemplateMap[head_type]);

        return head;
    }

    public void StoreHead(
        Head head
        )
    {
        _headsStackMap[head.HeadType]++;

        OnHeadsCountChanged.Invoke();

        Destroy(head.gameObject);
    }

    public void AddHead(
        HeadType head_type
        )
    {
        OnHeadsCountChanged.Invoke();

        _headsStackMap[head_type]++;
    }

    private void Awake()
    {
        Instance = this;
    }
}
