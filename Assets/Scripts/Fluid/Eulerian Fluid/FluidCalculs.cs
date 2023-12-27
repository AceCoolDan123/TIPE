using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCalculs : MonoBehaviour
{
    public int n = 32;

    private Texture2D texture;
    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(n+2, n+2, TextureFormat.RGBAHalf, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
