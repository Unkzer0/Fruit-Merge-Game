using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenshotUtility
{
   
        public static RenderTexture CaptureCustomArea(Camera cam, int width, int height, int antiAliasing = 2)
        {
            RenderTexture rt = new RenderTexture(width, height, 24);
            rt.antiAliasing = antiAliasing;

            RenderTexture currentRT = RenderTexture.active;
            cam.targetTexture = rt;
            RenderTexture.active = rt;

            cam.Render(); // Render manually

            cam.targetTexture = null;
            RenderTexture.active = currentRT;

            return rt;
        }
    

    // Optional: Convert to Texture2D
    public static Texture2D ConvertToTexture2D(RenderTexture rt)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;
        return tex;
    }
}


