using System.Collections;
using UnityEngine;

public class CleanUpPowerUp : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform container;

    [Header("Camera Settings")]
    [SerializeField] private float zoomedOutSize = 9f;
    [SerializeField] private float cameraZoomSpeed = 2f;

    [Header("Container Rotation Settings")]
    [SerializeField] private float rotationAngle = -120f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float holdTime = 1f;

    private float originalCamSize;
    private Quaternion originalRotation;

    private void Start()
    {
        originalCamSize = mainCamera.orthographicSize;
        originalRotation = container.rotation;
    }

    public void Activate()
    {
        StopAllCoroutines();
        StartCoroutine(PerformCleanUp());
    }

    private IEnumerator PerformCleanUp()
    {
        // Step 1: Zoom Out Camera
        while (Mathf.Abs(mainCamera.orthographicSize - zoomedOutSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, zoomedOutSize, cameraZoomSpeed * Time.deltaTime);
            yield return null;
        }

        // Step 2: Rotate Container
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotationAngle);
        while (Quaternion.Angle(container.rotation, targetRotation) > 0.5f)
        {
            container.rotation = Quaternion.RotateTowards(container.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Step 3: Hold
        yield return new WaitForSeconds(holdTime);

        // Step 4: Rotate Container Back
        while (Quaternion.Angle(container.rotation, originalRotation) > 0.5f)
        {
            container.rotation = Quaternion.RotateTowards(container.rotation, originalRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Step 5: Zoom In Camera
        while (Mathf.Abs(mainCamera.orthographicSize - originalCamSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, originalCamSize, cameraZoomSpeed * Time.deltaTime);
            yield return null;
        }

        // Final correction
        mainCamera.orthographicSize = originalCamSize;
        container.rotation = originalRotation;

        //  Notify manager cleanup is done
        PowerUpManager.instance.OnPowerUpComplete();
    }
}

