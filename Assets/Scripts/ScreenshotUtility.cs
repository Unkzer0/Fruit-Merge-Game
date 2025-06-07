using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenshotUtility
{
 
    public static Sprite CaptureScreenshotAsSprite()
    {
        int width = Screen.width;
        int height = Screen.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
    }
}

