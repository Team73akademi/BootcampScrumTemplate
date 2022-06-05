using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCubes : MonoBehaviour
{
    public static GenerateCubes intance;
    public GameObject blueCube;
    public Transform blueCubeparent;
    public int minX,maxX,minZ,maxZ;
    public LayerMask layermask;

    private void Awake()
    {
        if(intance == null)
        {
            intance = this;

        }
    }
    
}
