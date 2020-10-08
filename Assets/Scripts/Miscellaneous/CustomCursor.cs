using UnityEngine;
using System.Collections;


public class CustomCursor : MonoBehaviour
{
    [SerializeField] Sprite _cursor;
    [SerializeField] bool _active;
    // Use this for initialization
    void Start()
    {
        if (_active)
        {
            SetNewCursor(_cursor);
            Cursor.visible = false;
        }
        
    }

    void LateUpdate()
    {
        transform.localPosition = InputReader.MousePosition;
    }

    public void SetNewCursor(Sprite cursor) => GetComponent<SpriteRenderer>().sprite = cursor;
}
