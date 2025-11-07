using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{
    private GameObject cake;
    public ARRaycastManager RayCastManager;

    // Update is called once per frame
    void Update()
    {
        cake = SwitchToAR.GetSavedCakeObject();
        if (Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            List<ARRaycastHit> touches = new List<ARRaycastHit>();
            RayCastManager.Raycast(Input.GetTouch(0).position, touches, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

            if (touches.Count > 0)
            {
                GameObject newCake = Instantiate(cake, touches[0].pose.position, touches[0].pose.rotation);
                newCake.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); // Set scale to (0.3, 0.3, 0.3)
            }
        }
    }
}
