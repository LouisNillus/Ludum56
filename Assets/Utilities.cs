using UnityEngine;

public static class Utilities
{
    public static Vector3 MousePosition2D()
    {
        Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return new Vector3(mouse_position.x, mouse_position.y, 0);
    }

    public static Vector3 ChangeZ(
        this Vector3 vec,
        float z_value
        )
    {
        return new(vec.x, vec.y, z_value);
    }

    public static Color ChangeAlpha(
        this Color col,
        float alpha
        )
    {
        return new Color(col.r, col.g, col.b, alpha);
    }
}
