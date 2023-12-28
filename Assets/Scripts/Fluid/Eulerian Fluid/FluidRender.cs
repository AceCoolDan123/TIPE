using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class FluidRender : MonoBehaviour
{
    [Header("Obligatoire")]
    public Material material;
    [Header("Paramètres de Simulation")]
    public int n = 32;
    public float source = 100f;
    
    /* Pour le rendu de la texture et les jolies dessins */
    private Texture2D texture;
    private Vector3 objectSize;
    
    /* Infos générales de l'environnement */
    private Vector2 _mouseScreenPos;
    private Camera mainCam;
    
    /* Tableaux simulation */
    private int _size; // Taille initialisée dans start
    private float[] _dens;


    void Start()
    {
        /* Infos générales de l'environnement */
        mainCam = Camera.main;
        Renderer objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null) objectSize = objectRenderer.bounds.size;
        else Debug.LogError("Le composant Renderer est manquant sur cet objet.");
        /* Création des tableaux */ 
        _size = (n+2)*(n+2);
        _dens = new float[_size];
        /* Texture dans le matériau */
        texture = new Texture2D(n+2, n+2, TextureFormat.RGBAHalf, false);
        material.SetTexture("_MainTex", texture);
    }


    void Update()
    {
        GetFromUI();
        DrawDensity();
    }
    /* Gérée par l'input system */
    public void OnMousePosition(InputAction.CallbackContext cxt) 
    {
        _mouseScreenPos = cxt.ReadValue<Vector2>();
    }
    /*************************  Dessins  ************************************/
    private void DrawDensity() {
        var mip1Data = texture.GetPixelData<Color32>(0);
        Debug.Log(mip1Data.Length);
        // Fill pixels in mipmap level 1 with white
        for (int i = 0; i < mip1Data.Length; i++)
        {
            var newCol = (byte)(_dens[i/2] / 100 * 255);
            mip1Data[i] = new Color32(255, 255, 255, newCol);
        }

        // Copy the texture changes to the GPU
        texture.Apply(false);
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
        _dens[x + y*(n+2)] = source;

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
