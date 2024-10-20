using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu]
public class HeadDataTable : ScriptableObject
{
    public SerializedDictionary<HeadType, HeadData> Table = new SerializedDictionary<HeadType, HeadData>();

    public HeadData GetData(
        HeadType head_type
        )
    {
        Table.TryGetValue(head_type, out HeadData result);

        return result;
    }
}
