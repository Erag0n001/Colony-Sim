using Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pathfinding : MonoBehaviour
{
    public void PlayButtonPressed()
    {
        CreatureManager.instance.SpawnNewCreature();
    }
}
