using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Grow _grow = null;
    [SerializeField] private Shrink _shrink = null;

    public SpriteRenderer _background = null;
    public Collider2D _collider = null; //Raycast target is ignored through its layer

    public Vector2Int Coordinates = default;

    public Grow Grow => _grow;
    public int Size { get; set; } = 1;
    public Head Head { get; set; } = null;
    public bool Occupied => Head != null;
    public Vector3 Position => this.transform.position;

    public void Populate(
        Head new_head
        )
    {
        if (Occupied)
        {
            Head.AssignCell(new_head.LastCell);
        }

        Head = new_head;
        Head.transform.position = this.transform.position;
    }

    public bool Contains(
        Vector3 position
        )
    {
        return _collider.bounds.Contains(new(position.x, position.y, this.transform.position.z));
    }

    public void Destroy()
    {
        _shrink.Play(() => Destroy(gameObject));
    }
}
