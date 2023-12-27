using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class FluidRender : MonoBehaviour
{
    public Material material;
    public int n = 32;
    public Camera mainCam;
    
    private Texture2D texture;
    private int _size;
    
    /* TESTS */
    public Vector3 _mousepositon;

    void Start()
    {
        mainCam = Camera.main;
        texture = new Texture2D(n+2, n+2, TextureFormat.RGBAHalf, false);
        material.SetTexture("_MainTex", texture);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Mouse.current.position.ReadValue();
        _mousepositon = mainCam.ScreenToWorldPoint(camPos);
    }
    Vector3 GetMousePos()
    {
        Vector3 camPos = Mouse.current.position.ReadValue();
        Vector3 p = mainCam.ScreenToWorldPoint(camPos);
        return p;
        /* return new Vector3(p.x, 0f, p.z); */
    }
    
}
