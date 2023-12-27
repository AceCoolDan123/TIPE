using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class FluidRender : MonoBehaviour
{
    public Material material;
    private Camera mainCam;
    
    private Texture2D texture;
    public int n = 32;
    public float source = 100f;
    private int _size;
    

    private float[] _dens;
    /* TESTS */
    public Vector2 _mouseScreenPos;


    void Start()
    {
        mainCam = Camera.main;
        texture = new Texture2D(n+2, n+2, TextureFormat.RGBAHalf, false);
        material.SetTexture("_MainTex", texture);
        
    }

    // Update is called once per frame
    void Update()
    {
        GetFromUI();
        DrawDensity();
    }
    
    private Vector3 GetMousePos()
    {
        Vector3 vec3Pos = new Vector3(_mouseScreenPos.x, _mouseScreenPos.y, mainCam.nearClipPlane);
        return mainCam.ScreenToWorldPoint(vec3Pos);
    }
    
    /* Returns the ID of the cell nearest to the input position. */
    private Vector2Int GetIdFromPosition(Vector3 p) {
        p += 0.5f * Vector3.one;
        p *= (n+2);
        return new Vector2Int((int)p.x, (int)p.y);
    }
    public void OnMousePosition(InputAction.CallbackContext cxt)
    {
        _mouseScreenPos = cxt.ReadValue<Vector2>();
    }
    private void DrawDensity() {
        UnityEngine.Color[] tmp = texture.GetPixels(0);
        
        for (int i = 0; i < tmp.Length; i++) {
            Debug.Log(tmp[i]);
            tmp[i] = new UnityEngine.Color(_dens[i], _dens[i], _dens[i], 1f);
            Debug.Log(tmp[i]);
        }

        texture.SetPixels(tmp, 0);
        texture.Apply();
    }
    private void GetFromUI() {
        int x = GetIdFromPosition(GetMousePos()).x;
        int y = GetIdFromPosition(GetMousePos()).y;

        if (x < 1 || x > n || y < 1 || y > n) {
            return;
        } 
        _dens[x + y*(n+2)] = source;

    }
}
