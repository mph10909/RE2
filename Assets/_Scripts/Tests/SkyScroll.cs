using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScroll : MonoBehaviour
{

    public float Scrollx = 0.0f;
    public float Scrolly = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Offsetx = Time.time * Scrollx;
        float Offsety = Time.time * Scrolly;

        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Offsetx, Offsety);
    }
}
