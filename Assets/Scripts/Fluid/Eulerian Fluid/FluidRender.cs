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
        _size = (n+2)*(n+2);
        _dens = new float[_size];
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
        Ray ray = mainCam.ScreenPointToRay(_mouseScreenPos);
        RaycastHit hit;

        int indiceX = 0;
        int indiceY = 0;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 positionImpact = gameObject.transform.InverseTransformPoint(hit.point);
            positionImpact += new Vector3(5f, 0f, 5f); // On suppose que le plane est toujours carré de côté 10x10 
            indiceX = Mathf.FloorToInt((10f - positionImpact.x) * (n+2)/10);
            indiceY = Mathf.FloorToInt((10f - positionImpact.z) * (n+2)/10);
        }

        return new Vector2Int(indiceX, indiceY);
    }
    public void OnMousePosition(InputAction.CallbackContext cxt)
    {
        _mouseScreenPos = cxt.ReadValue<Vector2>();
    }
    private void DrawDensity() {
        UnityEngine.Color[] tmp = texture.GetPixels(0);
        int tmpLength = tmp.Length;
        for (int i = 0; i < tmpLength; i++) {
            tmp[i] = new UnityEngine.Color(_dens[i], _dens[i], _dens[i], 1f);
        }

        texture.SetPixels(tmp, 0);
        texture.Apply();
    }
    private void GetFromUI()
    {
        Vector2Int coord = GetIdFromPosition(GetMousePos());
        Debug.Log(coord);
        int x = coord.x;
        int y = coord.y;
        if (x < 1 || x > n || y < 1 || y > n) {
            return;
        } 
        _dens[x + y*(n+2)] = source;
        Debug.Log((x,y));

    }
}
