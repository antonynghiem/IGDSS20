﻿using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Vector3 _dragOrigin;
    public Camera mainCamera;
    public float speed = 1.5f;
    public BoxCollider boxCollider;
    
    public float minFov = 15f;
    public float maxFov = 90f;
    public float scrollSensitivity = 10f;

    private void Start()
    {
        // Set initial position and rotation of camera
        mainCamera.transform.position = new Vector3(0, 20,-30);
        mainCamera.transform.rotation = Quaternion.Euler(36.0f, 180.0f, 0.0f);
    }

    private void LateUpdate()
    {
        ChangeFov();
        if (Input.GetMouseButtonDown(1))
        {
            _dragOrigin = Input.mousePosition;
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) GetTile();
        if (Input.GetMouseButton(1)) MoveCameraInBoundary();
    }

    private void GetTile()
    {
        // This would cast rays only against collider in layer 8.
        int layerMask = 1 << 8;
        var cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log($"Clicked on: {hit.collider.name}");
        }
    }

    private void ChangeFov()
    {
        float fov = mainCamera.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        mainCamera.fieldOfView = fov;
    } 

    private void MoveCameraInBoundary()
    {
        Vector3 pos = mainCamera.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
        Vector3 move = new Vector3(
            pos.x * speed,
            0,
            pos.y * speed
        );
    
        mainCamera.transform.Translate(move, Space.World);

        Vector3 newPos = mainCamera.transform.position;
        Bounds areaBounds = boxCollider.bounds;

        mainCamera.transform.position = new Vector3(
            Mathf.Clamp(newPos.x, areaBounds.min.x, areaBounds.max.x),
            newPos.y,
            Mathf.Clamp(newPos.z, areaBounds.min.z, areaBounds.max.z)
        );
    }
}
