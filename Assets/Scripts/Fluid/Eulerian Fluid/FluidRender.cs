using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class FluidRender : MonoBehaviour
{
    [Header("Obligatoire")]
    public Material material;

    public bool simulating = false;
    [Header("Paramètres de Simulation")]
    public int n = 32;
    public float source = 100f;
    public float diff = 0.1f;
    
    /* Pour le rendu de la texture et les jolies dessins */
    private Texture2D texture;
    private Vector3 objectSize;
    
    /* Infos générales de l'environnement */
    private Vector2 _mouseScreenPos;
    private Camera mainCam;
    
    /* Tableaux simulation */
    private int _size; // Taille initialisée dans start
    private float[] _dens;
    private float[] _densPrev;
    private float[] _velX;
    private float[] _velY;
    private float[] _velXPrev;
    private float[] _velYPrev;

    private FluidCalculs fluidCalculs; 
    void Start()
    {
        fluidCalculs = gameObject.GetComponent<FluidCalculs>();
        /* Infos générales de l'environnement */
        mainCam = Camera.main;
        Renderer objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null) objectSize = objectRenderer.bounds.size;
        else Debug.LogError("Le composant Renderer est manquant sur cet objet.");
        
        /* Création des tableaux */ 
        _size = (n+2)*(n+2);
        _dens = new float[_size];
        _densPrev = new float[_size];
        _velX = Enumerable.Repeat(0.5f, _size).ToArray();
        _velY = new float[_size];
        _velXPrev = new float[_size];
        _velYPrev = new float[_size];
        
        /* Texture dans le matériau */
        texture = new Texture2D(n+2, n+2, TextureFormat.RGBAHalf, false);
        material.SetTexture("_MainTex", texture);
    }


    void Update()
    {
        GetFromUI();
        if (simulating) fluidCalculs.dens_step(n, ref _dens, ref _densPrev, ref _velX, ref _velY,diff,Time.deltaTime);
        DrawDensity();
    }
    /* Gérée par l'input system */
    public void OnMousePosition(InputAction.CallbackContext cxt) 
    {
        _mouseScreenPos = cxt.ReadValue<Vector2>();
    }
    /*************************  Dessins  ************************************/
    private void DrawDensity() {
        UnityEngine.Color[] tmp = texture.GetPixels(0);
        int tmpLength = tmp.Length;
        for (int i = 0; i < tmpLength; i++)
        {
            tmp[i] = new UnityEngine.Color(_dens[i], _dens[i], _dens[i], 1f);
        }
        texture.SetPixels(tmp, 0);
        texture.Apply();
    }
    /*************************UI Inputs************************************/
    private void GetFromUI()
    {
        if (!Mouse.current.leftButton.isPressed) return;
        Vector2Int coord = GetIdFromPosition(GetMousePos());
        int x = coord.x;
        int y = coord.y;
        if (x < 1 || x > n || y < 1 || y > n) {
            return;
        }

        if (simulating) _densPrev[x + y*(n+2)] = source;
        else
        {
            _dens[x + y*(n+2)] = source;
            _densPrev[x + y*(n+2)] = source;
        }
        

    }
    private Vector3 GetMousePos()
    {
        Vector3 vec3Pos = new Vector3(_mouseScreenPos.x, _mouseScreenPos.y, mainCam.nearClipPlane);
        return mainCam.ScreenToWorldPoint(vec3Pos);
    }
    /* Retourne l'ID de la cellule la plus proche de la souris */
    private Vector2Int GetIdFromPosition(Vector3 p) {
        Ray ray = mainCam.ScreenPointToRay(_mouseScreenPos);
        RaycastHit hit;

        int indiceX = 0;
        int indiceY = 0;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 positionImpact = gameObject.transform.InverseTransformPoint(hit.point);
            positionImpact += new Vector3(5f, 0f, 5f); // On suppose que le plane est toujours carré 
            indiceX = Mathf.FloorToInt((10f - positionImpact.x) * (n+2)/10f);
            indiceY = Mathf.FloorToInt((10f - positionImpact.z) * (n+2)/10f);
        }

        return new Vector2Int(indiceX, indiceY);
    }
}
