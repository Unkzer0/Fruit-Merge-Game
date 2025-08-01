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

    [Header("Sound Effects")]
    [SerializeField] private AudioClip cameraZoomOutSound;
    [SerializeField] private AudioClip cameraZoomInSound;


    private float originalCamSize;
    private Quaternion originalRotation;

    public static bool IsActive { get; private set; } // Add static flag

    private void Start()
    {
        originalCamSize = mainCamera.orthographicSize;
        originalRotation = container.rotation;
    }

    public void Activate()
    {
        StopAllCoroutines();
        PowerUpManager.instance.PowerUpDisableElement();
        IsActive = true;
        StartCoroutine(PerformCleanUp());
    }

    private IEnumerator PerformCleanUp()
    {
        SoundManager.instance.PlayButtonClick(cameraZoomOutSound);
        while (Mathf.Abs(mainCamera.orthographicSize - zoomedOutSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, zoomedOutSize, cameraZoomSpeed * Time.deltaTime);
            yield return null;
        }

        Quaternion targetRotation = Quaternion.Euler(0, 0, rotationAngle);
        while (Quaternion.Angle(container.rotation, targetRotation) > 0.5f)
        {
            container.rotation = Quaternion.RotateTowards(container.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(holdTime);

        while (Quaternion.Angle(container.rotation, originalRotation) > 0.5f)
        {
            container.rotation = Quaternion.RotateTowards(container.rotation, originalRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        SoundManager.instance.PlayButtonClick(cameraZoomInSound);
        while (Mathf.Abs(mainCamera.orthographicSize - originalCamSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, originalCamSize, cameraZoomSpeed * Time.deltaTime);
            yield return null;
        }

        mainCamera.orthographicSize = originalCamSize;
        container.rotation = originalRotation;

        Invoke(nameof(PowerUpManager.instance.PowerUpEnableElement), 0.2f);
        Invoke(nameof(EnablePowerUpElements), 0.2f);
        IsActive = false; // Reset flag
    }

    private void EnablePowerUpElements()
    {
        PowerUpManager.instance.PowerUpEnableElement();
    }
}

