using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class InputReader{

    private InputReader() { }

    public static bool GetAction1Down()
    {
        return Input.GetMouseButtonDown((int)MouseButton.LeftMouse);
    }
    public static bool GetAction2Down()
    {
        return Input.GetMouseButtonDown((int)MouseButton.RightMouse);
    }

    public static bool GetSwitchWeaponUp()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public static bool GetSwitchWeaponDown()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    public static Vector2 FindDirections()
    {
        Vector2 vec = Vector2.zero;
        if (Input.GetKey(KeyCode.A))vec += new Vector2(-1, 0);
        if (Input.GetKey(KeyCode.D)) vec += new Vector2(1, 0);
        if (Input.GetKey(KeyCode.W)) vec += new Vector2(0, 1);
        if (Input.GetKey(KeyCode.S)) vec += new Vector2(0, -1);
        return vec.normalized;
    }

    public static Vector2 MousePosition => new Vector2(
        Input.mousePosition.x / Screen.width* 640 - 320,
        Input.mousePosition.y / Screen.height* 360 - 180
    );
}