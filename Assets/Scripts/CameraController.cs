using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // Karakter nesnesinin Transform bileşeni
    public Transform camera;  // Kamera nesnesinin Transform bileşeni
    public Vector3 cameraOffset = new Vector3(0f, 2f, -5f);  // Kamera offset değeri
    public float cameraSpeed = 2f;  // Kamera hareket hızı
    
    private float cameraRotationX = 0f;  // Kamera yatay açısı
    private float cameraRotationY = 0f;  // Kamera dikey açısı
    public float minYAngle = 0f;  // Kamera minimum dikey açısı
    public float maxYAngle = 150f;  // Kamera maksimum dikey açısı

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            cameraRotationX += mouseX * cameraSpeed;
            cameraRotationY -= mouseY * cameraSpeed;
            
            cameraRotationY = Mathf.Clamp(cameraRotationY, minYAngle, maxYAngle);
        }

        Quaternion cameraRotation = Quaternion.Euler(cameraRotationY, cameraRotationX, 0f);
        Vector3 cameraOffsetPosition = cameraRotation * cameraOffset;
        camera.position = player.position + cameraOffsetPosition + new Vector3(0f, 2f, 0f);
        camera.LookAt(player.position);
    }
}
