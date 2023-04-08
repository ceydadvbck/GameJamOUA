using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoSingleton<MainMenuCameraController>
{
    public Vector2 cameraMovementRange = new Vector2(0.5f, 0.5f);
    public Transform cameraTransform;
    private Vector3 cameraStartPosition;
    private Vector2 screenSize;

    void Start()
    {
        cameraStartPosition = cameraTransform.localPosition;
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousePositionFromCenter = new Vector2(mousePosition.x - screenSize.x / 2f, mousePosition.y - screenSize.y / 2f);
        Vector2 mousePositionFromCenterNormalized = new Vector2(mousePositionFromCenter.x / screenSize.x, mousePositionFromCenter.y / screenSize.y);
        Vector2 cameraMovement = new Vector2(mousePositionFromCenterNormalized.x * cameraMovementRange.x, mousePositionFromCenterNormalized.y * cameraMovementRange.y);
        cameraTransform.localPosition = cameraStartPosition + new Vector3(cameraMovement.x, cameraMovement.y, 0f);
    }
}
