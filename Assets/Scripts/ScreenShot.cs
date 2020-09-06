﻿using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour {

    public Material cloakMat;

    Camera cam;

    void Start() {
        cam = Camera.main;
    }

    public void TakePicture() {
        Debug.Log("Taking Picture...");
        StartCoroutine(PictureRoutine());
    }

    IEnumerator PictureRoutine() {

        //turn off mask while taking picture if you want to tint cloak color
        cloakMat.SetFloat("_TakingPicture", 1);

        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        Rect rect = new Rect(0, 0, width, height);
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGBA32, false);

        cam.targetTexture = renderTexture;
        cam.Render();

        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        cloakMat.SetTexture("_ScreenShot", screenShot);

        cam.targetTexture = null;
        RenderTexture.active = null;

        Destroy(renderTexture);

        cloakMat.SetFloat("_TakingPicture", 0);
    }
}
