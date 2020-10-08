using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] float pixelSize = 0.03125f;
    static float _standardFollowStrength = 0.0f;
    static Transform _toFollow;
    static Camera _camera;
    static CameraControls _controller;
    [Range(0.0f, 1.0f)] public float followStrength;

    public static void SetTargetToFollow(Transform toFollow)
    {
        _toFollow = toFollow;
    }

    public static void AssignCamera(){ AssignCamera(Camera.main); }

    public static void AssignCamera(Camera camera) 
    {
        _camera = camera;
        _controller = _camera.GetComponent<CameraControls>();
        if(_controller == null)
        {
            _camera.gameObject.AddComponent<CameraControls>();
            _controller = _camera.GetComponent<CameraControls>();
        }
    }

    void LateUpdate()
    {
        if (_toFollow != null)
        {
            Vector3 vec = Vector3.Lerp(_toFollow.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), _controller.followStrength * 0.4f);
            vec.x = Misc.roundTo(vec.x, pixelSize);
            vec.y = Misc.roundTo(vec.y, pixelSize);
            vec.z = -10f;
            _camera.transform.position = vec;
        }
        else AssignCamera();
    }

    void Start()
    {
        followStrength = _standardFollowStrength;

    }
}
