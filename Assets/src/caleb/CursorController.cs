using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private GameObject _cursor;

    // Update is called once per frame
    public void MoveCursor(Vector2 pos)
    {
        Vector3 newPosition = new Vector3(pos.x, pos.y, this.transform.position.z);
        this.transform.position = newPosition;
        Debug.Log(_cursor.transform.position);
    }
}
