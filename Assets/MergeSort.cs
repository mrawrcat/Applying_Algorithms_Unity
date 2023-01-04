using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MergeSort : MonoBehaviour
{
    [SerializeField]
    private int[] arr = { 12, 11, 13, 5, 6, 7, 8};
    [SerializeField]
    private List<int> unsortedList = new List<int>();
    [SerializeField]
    private List<int> leftList = new List<int>();
    [SerializeField]
    private List<int> rightList = new List<int>();
    [SerializeField]
    private List<int> sortedList = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        //int[] temp_arr1 = arr.Take(arr.Length/2).ToArray();
        //int[] temp_arr2 = arr.Skip(arr.Length/2).ToArray();
        foreach(int value in arr)
        {
            unsortedList.Add(value);
        }
       
        leftList = unsortedList.Take(unsortedList.Count /2).ToList();
        rightList = unsortedList.Skip(unsortedList.Count/2).ToList();

        sortedList = sort(unsortedList);

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private List<int> merge(List<int> left_list, List<int> right_list)
    {
        List<int> temp_arr = new List<int>();
        int i = 0, j = 0;
        while(i < left_list.Count && j < right_list.Count)
        {
            if (left_list[i] < right_list[j])
            {
                temp_arr.Add(left_list[i]);
                i += 1;
            }
            else
            {
                temp_arr.Add(right_list[j]);
                j += 1;
            }
        }
        while(i < left_list.Count)
        {
            temp_arr.Add(left_list[i]);
            i += 1;
        }
        while(j < right_list.Count)
        {
            temp_arr.Add(right_list[j]);
            j += 1;
        }
        return temp_arr;
    }

    private List<int> sort(List<int> arr)
    {
        if(arr.Count < 2)
        {
            return arr;
        }
        else
        {
            List<int> left_list = sort(arr.Take(arr.Count/ 2).ToList());
            List<int> right_list = sort(arr.Skip(arr.Count/ 2).ToList());
            return merge(left_list, right_list);
        }
    }
   
}