using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStart : MonoBehaviour
{
    void Start()
    {
        CameraControls.AssignCamera();
        Misc.AssignInstance(GetComponent<Misc>());
        Destroy(this);
    }

    void Update()
    {
        
    }
}
