using Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewMapp : MonoBehaviour
{
    public void PlayButtonPressed()
    {
        Printer.LogWarning("test");
        GridManager.GenerateGrid();
    }
}
