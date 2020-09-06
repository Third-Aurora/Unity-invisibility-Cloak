using UnityEngine;
using System;

public class CameraView : MonoBehaviour {

    WebCamTexture webcamTexture;
    bool ratioSet;

    //this (incorrectly) assumes the front camera is index 1
    bool isFrontCam;

    void Start() {
        InitWebCam();
    }

    void Update() {

        if (ratioSet || webcamTexture == null) return;

        if (webcamTexture.width > 100) {
            ScalePlane();
        }
    }

    public void SwitchCamera() {
        isFrontCam = !isFrontCam;
        InitWebCam();
    }

    void InitWebCam() {

        int camIndex = Convert.ToInt32(isFrontCam);

        if (camIndex > WebCamTexture.devices.Length - 1) {
            Debug.Log("Camera index: " + camIndex +  " does not exist.");
            return;
        }

        string camName = WebCamTexture.devices[camIndex].name;

        ratioSet = false;

        webcamTexture = new WebCamTexture(camName,Screen.width, Screen.height, 30);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_BaseMap", webcamTexture);
        webcamTexture.Play();
    }

    void ScalePlane() {
        ratioSet = true;

#if UNITY_EDITOR

        float ratio = (float)webcamTexture.width / (float)webcamTexture.height;
        transform.localEulerAngles = new Vector3(90, 180, 0);
        transform.localScale = new Vector3(ratio, 1, 1);

#elif PLATFORM_IPHONE

        float ratio = (float)webcamTexture.height / (float)webcamTexture.width;
        Vector3 camScale = new Vector3(1, 1, -ratio);
        transform.localScale = camScale;

#else
        float ratio = (float)webcamTexture.height / (float)webcamTexture.width;
        Vector3 camScale = new Vector3(1, 1, ratio);
        if (isFrontCam) camScale.x *= -1f;
        transform.localScale = camScale;

#endif
    }
}
