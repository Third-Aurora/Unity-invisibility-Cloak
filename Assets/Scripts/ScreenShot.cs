using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class ScreenShot : MonoBehaviour {

    public Material cloakMat;

    Camera cam;

    void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            TakePicture();
        }
    }

    void TakePicture() {
        StartCoroutine(PictureRoutine());
    }

    IEnumerator PictureRoutine() {
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
    }
}
