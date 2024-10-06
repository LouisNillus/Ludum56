using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    [SerializeField, Range(2, 10)] private int _height = 2;
    [SerializeField, Range(2, 10)] private int _width = 2;
    [SerializeField] private SerializedDictionary<HeadType, int> _headsStackMap = new();

    public SerializedDictionary<HeadType, int> HeadsStackMap => _headsStackMap;
    public int Height => _height;
    public int Width => _width;
}
