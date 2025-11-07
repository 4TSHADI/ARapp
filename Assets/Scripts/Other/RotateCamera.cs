using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateCamera : MonoBehaviour
{
    //[SerializeField] Transform _target;
    //[SerializeField] float _distanceFromTarget = 10f;

    //private float sensitivity = 10f;
    //private bool _isDragging;
    //private float _currentScale;
    //public float minScale, maxScale;
    //private float _temp;
    //private float _scalingRate = 2;
    //private float _yaw = 0f;
    //private float _pitch = 0f;
    public float rotationSpeed = 0.1f;
    public float zoomSpeed = 0.1f; 
    public float minZoom = 0.5f; 
    public float maxZoom = 2.0f;
    //private void Start()
    //{
    //    _currentScale = transform.localScale.x;
    //}
    // Update is called once per frame
    void Update()
    {
        ZoomCam();
        RotateScreen();
        //Control();
        //Quaternion yawRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        //Rotate(yawRotation);
    }

    public void ZoomCam()
    {
        if (Input.touchCount == 2)
        {
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition; 
                float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude; 
                float touchDeltaMag = (touch1.position - touch2.position).magnitude; 
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag; 
                float newZoom = Mathf.Clamp(transform.localScale.x - deltaMagnitudeDiff * zoomSpeed, minZoom, maxZoom);
                transform.localScale = new Vector3(newZoom, newZoom, newZoom);
            }
        }
    }
        public void RotateScreen()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    float rotationX = touch.deltaPosition.y * rotationSpeed;
                    float rotationY = -touch.deltaPosition.x * rotationSpeed;
                    transform.Rotate(rotationX, rotationY, 0, Space.World);
                }
            } 
        }
                //public void OnPointerDown(PointerEventData eventData)
                //{
                //    if (Input.touchCount == 1)
                //    {
                //        _isDragging = true;

                //    }
                //}


                //public void OnPointerUp(PointerEventData eventData)
                //{
                //    _isDragging = false;
                //}


                //public void Control()
                //{
                //    Vector2 inputDelta = Vector2.zero;

                //    if (Input.touchCount > 0)
                //    {
                //        Touch touch = Input.GetTouch(0);
                //        inputDelta = touch.deltaPosition;
                //    }
                //    else if (Input.GetMouseButton(0))
                //    {
                //        inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                //    }

                //    _yaw += inputDelta.x * sensitivity * Time.deltaTime;
                //    _pitch += inputDelta.x * sensitivity * Time.deltaTime;
                //}

                //public void Rotate(Quaternion rotation)
                //{
                //    Vector3 positionOffset = rotation * new Vector3(0, 0, -_distanceFromTarget);
                //    transform.position = _target.position + positionOffset;
                //    transform.rotation = rotation;
                //}
            }
