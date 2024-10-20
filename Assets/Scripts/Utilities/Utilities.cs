using System.Linq;
using UnityEngine;

public static class Utilities
{
    public static Vector3 MousePosition2D()
    {
        Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return new Vector3(mouse_position.x, mouse_position.y, 0);
    }

    public static Vector3 ChangeX(
        this Vector3 vec,
        float x_value
        )
    {
        return new(x_value, vec.y, vec.z);
    }

    public static Vector3 ChangeY(
        this Vector3 vec,
        float y_value
        )
    {
        return new(vec.x, y_value, vec.z);
    }

    public static Vector3 ChangeZ(
        this Vector3 vec,
        float z_value
        )
    {
        return new(vec.x, vec.y, z_value);
    }

    public static Vector3 OffsetX(
        this Vector3 vec,
        float x_value
        )
    {
        return new(vec.x + x_value, vec.y, vec.z);
    }

    public static Vector3 OffsetY(
        this Vector3 vec,
        float y_value
        )
    {
        return new(vec.x, vec.y + y_value, vec.z);
    }

    public static Color ChangeAlpha(
        this Color col,
        float alpha
        )
    {
        return new Color(col.r, col.g, col.b, alpha);
    }

    public static Color FindDominantColor(
        this Sprite sprite,
        float grey_tolerance = 0.1f
        )
    {
        Texture2D readableTexture = sprite.texture.isReadable ? sprite.texture : CreateTemporaryReadableTexture(sprite.texture); ;

        Color[] pixels = readableTexture.GetPixels(
            (int)sprite.textureRect.x, (int)sprite.textureRect.y,
            (int)sprite.textureRect.width, (int)sprite.textureRect.height
        );

        if (!sprite.texture.isReadable)
        {
            Object.DestroyImmediate(readableTexture);
        }

        return pixels
            .Where(c => !IsGrayish(c, grey_tolerance) && c != Color.white)
            .GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();
    }

    private static Texture2D CreateTemporaryReadableTexture(Texture2D texture)
    {
        RenderTexture tempRT = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(texture, tempRT);

        Texture2D readableTexture = new Texture2D(texture.width, texture.height);
        RenderTexture.active = tempRT;
        readableTexture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        readableTexture.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(tempRT);

        return readableTexture;
    }

    private static bool IsGrayish(
        Color color,
        float tolerance
        )
    {
        float maxComponent = Mathf.Max(color.r, color.g, color.b);
        float minComponent = Mathf.Min(color.r, color.g, color.b);

        return (maxComponent - minComponent) <= tolerance;
    }
}
