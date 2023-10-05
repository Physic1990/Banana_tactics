using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGridManager : MonoBehaviour
{
    public int max_x, max_y;
    public int cursorDelay;
    private int cursorDelayCounter;
    private Grid grid;
    private Cursor cursor;
    public enum MoveDirection
    {
        Left,
        Right,
        Up,
        Down,
        None,
        // Add more cell types as needed
    }
    // Define the Grid class
    public class Grid
    {
        private int width;
        private int height;
        private GameObject[,] gridArray;

        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
            gridArray = new GameObject[width, height];
        }

        // Add methods and properties for your Grid class as needed
    }

    public class Cursor
    {
        private int x, y, max_x, max_y;
        Vector2 position;
        public Cursor(int x, int y, int max_x, int max_y)
        {
            this.x = x;
            this.y = y;
            this.max_x = max_x;
            this.max_y = max_y;
            position = new Vector2(x, y);
        }

        public void MoveCursor(MoveDirection direction)
        {
            switch(direction)
            {
                case MoveDirection.Left:
                    if(x > 0)
                    {
                        x--;
                    }
                    break;
                case MoveDirection.Right:
                    if( x < max_x)
                    {
                        x++;
                    }
                    break;
                case MoveDirection.Up:
                    if(y < max_y)
                    {
                        y++;
                    }
                    break;
                case MoveDirection.Down:
                    if(y > 0)
                    {
                        y--;
                    }
                    break;
                default:
                    break;
            }
            position.x = x;
            position.y = y;
        }

        public Vector2 GetCursorPosition()
        {
            return position;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        max_x = 10;
        max_y = 15;
        grid = new Grid(max_x, max_y);
        cursor = new Cursor(0, 0, max_x, max_y);
    }

    // Update is called once per frame
    void Update()
    {
        cursorDelayCounter++;
        if (cursorDelayCounter > cursorDelay)
        {
            cursorDelayCounter = UpdateCursor(cursorDelayCounter, cursor);
        }

    }

    public int UpdateCursor(int returnValue, Cursor cursor)
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            cursor.MoveCursor(MoveDirection.Right);
            returnValue = 0;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cursor.MoveCursor(MoveDirection.Left);
            returnValue = 0;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            cursor.MoveCursor(MoveDirection.Down);
            returnValue = 0;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cursor.MoveCursor(MoveDirection.Up);
            returnValue = 0;
        }
        Debug.Log(cursor.GetCursorPosition());
        return returnValue;
    }
}
