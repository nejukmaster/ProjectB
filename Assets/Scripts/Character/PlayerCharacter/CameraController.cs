using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float followingDelay;
    [SerializeField] Camera camera;

    Vector3 _toCamera;

    // Start is called before the first frame update
    void Start()
    {
        _toCamera = camera.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = transform.position + _toCamera;
    }

    public Camera GetCamera()
    {
        return camera;
    }
}
