using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToFollowByCamera : MonoBehaviour
{
    static List<ToFollowByCamera> ListOfObjectsToFollow = new List<ToFollowByCamera>();
    static ToFollowByCamera currFollowed;

    public void SetAsFollowed()
    {
        currFollowed = this;
        CameraControls.SetTargetToFollow(transform);
    }
    protected void Start()
    {
        ListOfObjectsToFollow.Add(this);
        SetAsFollowed();
    }
}
