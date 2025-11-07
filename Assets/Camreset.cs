using UnityEngine;
using Cinemachine;


public class Camreset : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // Reference to the camera's transform
    [SerializeField] private Transform camHolderTransform; // Reference to the camera's transform



    private void Start()
    {
        ResetCameraTransform();

    }
    void ResetCameraTransform()
    {
        var cinemachineBrain = cameraTransform.GetComponent<Cinemachine.CinemachineBrain>();
        if (cinemachineBrain != null)
            cinemachineBrain.enabled = false;  // Temporarily disable

        camHolderTransform.transform.position = new Vector3(0, -1, 0);
        camHolderTransform.eulerAngles = new Vector3(15, 0, 0);

        cameraTransform.transform.position = new Vector3(0, -2, -7);
        cameraTransform.transform.eulerAngles = new Vector3(0, 0, 0);

        if (cinemachineBrain != null)
            cinemachineBrain.enabled = true; // Re-enable
    }


}
