using UnityEngine;

public static class ScreenshotUtility
{
    /// <summary>
    /// Captures a screenshot from a specific camera into a RenderTexture.
    /// </summary>
    public static RenderTexture CaptureCustomArea(Camera cam, int width, int height, int antiAliasing = 2)
    {
        if (cam == null)
        {
            Debug.LogError("ScreenshotUtility: Camera is null.");
            return null;
        }

        RenderTexture rt = new RenderTexture(width, height, 24)
        {
            antiAliasing = antiAliasing
        };

        RenderTexture currentRT = RenderTexture.active;

        cam.targetTexture = rt;
        RenderTexture.active = rt;

        cam.Render();

        cam.targetTexture = null;
        RenderTexture.active = currentRT;

        return rt;
    }

    /// <summary>
    /// Converts a RenderTexture to a Texture2D.
    /// </summary>
    public static Texture2D ConvertToTexture2D(RenderTexture rt)
    {
        if (rt == null)
        {
            Debug.LogError("ScreenshotUtility: RenderTexture is null.");
            return null;
        }

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;
        return tex;
    }
}
