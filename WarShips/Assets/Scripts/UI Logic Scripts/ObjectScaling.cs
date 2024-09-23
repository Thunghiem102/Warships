using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaling : MonoBehaviour
{
    public Camera mainCamera; // Camera chính (Orthographic)
    public MeshFilter backgroundMeshFilter;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
       
        if (backgroundMeshFilter == null)
        {
            backgroundMeshFilter = GetComponent<MeshFilter>();
        }
        AdjustScale();
    }

    void AdjustScale()
    {
        // Kích thước camera
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Kích thước của mesh
        Vector3 meshSize = backgroundMeshFilter.sharedMesh.bounds.size;

        // Kích thước gốc của background (không bị scale)
        Vector3 originalScale = transform.localScale;
        float originalWidth = meshSize.x;
        float originalHeight = meshSize.z;

        // Tính tỷ lệ để scale
        float scaleFactorX = cameraWidth / originalWidth;
        float scaleFactorZ = cameraHeight / originalHeight;

        Debug.Log("Camera Width: " + cameraWidth);
        Debug.Log("Camera Height: " + cameraHeight);
        Debug.Log("Mesh Width: " + originalWidth);
        Debug.Log("Mesh Height: " + originalHeight);
        Debug.Log("Scale Factor X: " + scaleFactorX);
        Debug.Log("Scale Factor Z: " + scaleFactorZ);

        // Áp dụng tỷ lệ scale cho GameObject
        transform.localScale = new Vector3(scaleFactorX, originalScale.y, scaleFactorZ );
        // Cập nhật bounding box của mesh
        backgroundMeshFilter.mesh.RecalculateBounds();
        backgroundMeshFilter.mesh.RecalculateNormals();
    }

}
