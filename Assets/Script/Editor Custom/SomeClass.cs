using UnityEngine;
using UnityEditor;
public class SomeClass : MonoBehaviour
{
    public ArrayInt[] mArray;

    void Start()
    {
        Debug.Log(mArray[mArray.Length - 1][mArray[mArray.Length - 1].Length - 1]); //debugs the last value
    }
}