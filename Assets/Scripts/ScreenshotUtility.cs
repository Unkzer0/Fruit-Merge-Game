using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenshotUtility
{
    public static RenderTexture CaptureCustomArea(Camera cam, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;
        cam.Render();
        cam.targetTexture = null;

        return rt;
    }
}
