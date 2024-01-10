using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrasition : TransportData
{ 
    public GameObject sceneTrasition;


    public override void Update()
    {
        base.Update();
        Instantiate(sceneTrasition, new Vector3(20,20,20),Quaternion.identity);

    }
}
