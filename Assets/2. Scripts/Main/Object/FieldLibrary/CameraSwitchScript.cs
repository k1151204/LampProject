using UnityEngine;
using System.Collections;

public class CameraSwitchScript : MonoBehaviour {
    CameraController mainCamera;
    public CameraView cameraView;
    public CameraZoom cameraZoom;

    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            mainCamera.setCameraView(cameraView);
            mainCamera.setCameraZoom(cameraZoom);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCamera.setCameraView(CameraView.quaterView);
            mainCamera.setCameraZoom(CameraZoom.bossZoom);
        }
    }
}
