using UnityEngine;
using UnityEngine.Events;

public class HeadsManager : MonoBehaviour
{
    public static HeadsManager Instance = null;

    public UnityEvent<Head> OnHeadSelected { get; set; } = new();
    public UnityEvent<Head> OnHeadMoved { get; set; } = new();
    public UnityEvent<Head> OnHeadDropped { get; set; } = new();


    private void Awake()
    {
        Instance = this;
    }
}
