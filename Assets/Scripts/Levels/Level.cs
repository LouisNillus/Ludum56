using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    [SerializeField, Range(2, 10)] private int _height = 2;
    [SerializeField, Range(2, 10)] private int _width = 2;
    [SerializeField] private SerializedDictionary<HeadData, int> _headsMap = new();

    public SerializedDictionary<HeadData, int> HeadsMap => _headsMap;

    public int Height => _height;
    public int Width => _width;

    [ReadOnly] public Color _correctSetup = Color.red;

    private void OnValidate()
    {
        _correctSetup = (_height * _width) <= _headsMap.Sum(kvp => kvp.Value) ? Color.green : Color.red;
    }
}
