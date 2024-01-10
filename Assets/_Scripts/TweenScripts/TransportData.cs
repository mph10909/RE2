using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportData : MonoBehaviour
{
    public Action onComplete;

    public virtual void Update()
    {

            enabled = false;
            onComplete?.Invoke();

    }

}
