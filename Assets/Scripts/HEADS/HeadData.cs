using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HeadData : ScriptableObject
{
    [Header("Object")]
    public Head Object = null;
    public Color MainColor = Color.white;
    [SpritePreview] public Sprite Sprite = null;
    public SerializedDictionary<EmotionalState, Sprite> StatesSprites = new();

    [Header("Informations")]
    public HeadType Type = HeadType.None;
    public HeadFamily Family = HeadFamily.None;

    public List<Rule> Rules = new List<Rule>();
    public List<Constraint> Constraints = new List<Constraint>();

    [Header("Audio")]
    public AudioClip SelectSFX = null;
    public AudioClip DropSFX = null;

    private void OnValidate()
    {
        if (Sprite != null)
        {
            MainColor = Sprite.FindDominantColor();
        }
    }
}
