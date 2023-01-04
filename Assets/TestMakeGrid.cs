using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMakeGrid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int cellsize;
    [SerializeField] private bool show_debug;
    //private Create_Grid<int> grid;
    //private Create_Grid<bool> boolGrid;
    private Create_Grid<CustomGridMapObject> customGrid;

    // Start is called before the first frame update
    void Start()
    {
        //grid = new Create_Grid<int>(width, height, cellsize, transform.position, ()=> new int(), false);
        //boolGrid = new Create_Grid<bool>(width, height, cellsize, transform.position, () => new bool());
        customGrid = new Create_Grid<CustomGridMapObject>(width, height, cellsize, transform.position, (Create_Grid<CustomGridMapObject> g, int x, int y)=> new CustomGridMapObject(g, x, y), show_debug);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //int value = grid.GetValue(Create_Grid<int>.GetMouseWorldPos());
            //grid.SetValue(Create_Grid<int>.GetMouseWorldPos(), value + 1);
            //bool gridbool = boolGrid.GetGridObject(Create_Grid<bool>.GetMouseWorldPos());
            //boolGrid.SetGridObject(Create_Grid<bool>.GetMouseWorldPos(), !gridbool);
            Vector3 pos = Create_Grid<CustomGridMapObject>.GetMouseWorldPos();
            CustomGridMapObject customGridMapObj = customGrid.GetGridObject(pos);
            if (customGridMapObj != null)
            {
                customGridMapObj.AddValue(5);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log(grid.GetValue(Create_Grid<int>.GetMouseWorldPos()));
        }
    }
}

public class CustomGridMapObject
{
    private Create_Grid<CustomGridMapObject> grid;
    private int x;
    private int y;
    private int value;

    public CustomGridMapObject(Create_Grid<CustomGridMapObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value+= addValue;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
