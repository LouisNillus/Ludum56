using UnityEngine;
using UnityEngine.Events;

public class HeadsManager : MonoBehaviour
{
    public static HeadsManager Instance = null;

    public UnityEvent<Head> OnHeadSelected { get; set; } = new();
    public UnityEvent<Head> OnHeadMoving { get; set; } = new();
    public UnityEvent<Head> OnHeadDropped { get; set; } = new();
    public UnityEvent OnAnyHeadAssigned { get; set; } = new();

    private void Awake()
    {
        Instance = this;
    }
}
