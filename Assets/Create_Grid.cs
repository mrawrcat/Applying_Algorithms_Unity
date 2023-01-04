using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Create_Grid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }



    //visuals for grid using text mesh object
    public static TextMesh CreateGridTextMesh(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    {
        GameObject txtMeshObj = new GameObject("World Text", typeof(TextMesh));
        Transform obj_transform = txtMeshObj.transform;
        obj_transform.SetParent(parent, false);
        obj_transform.localPosition = localPosition;
        TextMesh textMesh = txtMeshObj.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = 1;
        return textMesh;

    }

    public static TextMesh CreateGridLines(Transform parent, string text, Color color, Vector3 localPosition = default(Vector3), int fontSize = 40, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center)
    {
        if (color == null) color = Color.white;
        return CreateGridTextMesh(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment);
    }

    public static Vector3 GetMouseWorldPos_WithZ(Vector3 screenPos, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPos);
        return worldPosition;
    }

    public static Vector3 GetMouseWorldPos_WithZ(Camera worldCamera)
    {
        return GetMouseWorldPos_WithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPos_WithZ()
    {
        return GetMouseWorldPos_WithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPos()
    {
        Vector3 v3 = GetMouseWorldPos_WithZ(Input.mousePosition, Camera.main);
        v3.z= 0f;
        return v3;
    }

    private int width;
    private int height;
    private TGridObject[,] grid_Array;
    private float cellsize;
    private TextMesh[,] debugTextArray;
    private Vector3 originPosition;

    //constructor, accepts two paremeters width and height
    public Create_Grid(int width, int height, float cellsize, Vector3 originPosition, Func<Create_Grid<TGridObject>, int, int, TGridObject> createGridObj, bool showDebug = true)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;
        this.originPosition = originPosition;
        grid_Array = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int w = 0; w < grid_Array.GetLength(0); w++)
        {
            for (int h = 0; h < grid_Array.GetLength(1); h++)
            {
                grid_Array[w, h] = createGridObj(this, w, h);
            }
        }

        if (showDebug)
        {
            for (int w = 0; w < grid_Array.GetLength(0); w++)
            {
                for (int h = 0; h < grid_Array.GetLength(1); h++)
                {
                    debugTextArray[w, h] = CreateGridLines(null, grid_Array[w, h]?.ToString(), Color.white, GetWorldPosition(w, h) + (new Vector3(cellsize, cellsize) * .5f), 40);
                    Debug.DrawLine(GetWorldPosition(w, h), GetWorldPosition(w, h + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(w, h), GetWorldPosition(w + 1, h), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
       

        OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
        {
            debugTextArray[eventArgs.x, eventArgs.y].text = grid_Array[eventArgs.x, eventArgs.y]?.ToString();
        };
    }
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * cellsize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellsize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellsize);
    }

    public void SetGridObject(int w, int h, TGridObject value)
    {
        if(w >= 0 && h >= 0 && w < width && h < height)
        {
            grid_Array[w, h] = value;
            debugTextArray[w,h].text = grid_Array[w,h].ToString();

        }
        
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public void TriggerGridObjectChanged(int w, int h)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = w, y = h });
    }

    public TGridObject GetGridObject(int w, int h)
    {
        if (w >= 0 && h >= 0 && w < width && h < height)
        {
            return grid_Array[w, h];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}
