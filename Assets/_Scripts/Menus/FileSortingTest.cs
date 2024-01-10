using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FileSortingTest : MonoBehaviour
{
    [SerializeField] Text FileCounter;
    [SerializeField] int FileNumber;
    void Start()
    {
        FileNumber = 1;
        SetFileNumber();
    }

    // Update is called once per frame
    void Update()
    {
        SetFileNumber();
    }

    void SetFileNumber()
    {
        FileCounter.text = FileNumber.ToString();
    }
}
