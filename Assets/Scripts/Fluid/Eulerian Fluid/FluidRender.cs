using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using Color = System.Drawing.Color;

public class FluidRender : MonoBehaviour
{
    [Header("Obligatoire")]
    public Material material;
    public bool simulating = false;
    
    [Header("Paramètres de Simulation")]
    public int n = 32;
    public float source = 100f;
    public float diff = 0.1f;
    public float visc = 0.1f;
    public float force = 75f;

    [Header("Customisation")] 
    public UnityEngine.Color dieColor;
    
    /* Pour le rendu de la texture et les jolies dessins */
    private Texture2D texture;
    
    /* Infos générales de l'environnement */
    private Vector2 _mouseScreenPos;
    private Vector2 _mouseDelta;
    private Camera mainCam;
    
    /* Tableaux simulation */
    private int _size; // Taille initialisée dans start
    private float[] _dens;
    private float[] _densPrev;
    private float[] _velX;
    private float[] _velY;
    private float[] _velXPrev;
    private float[] _velYPrev;

    //private int time_step = 0;
    
    private FluidCalculs fluidCalculs; 
    private CanvasFluid _canvasFluid; 
    void Start()
    {
        fluidCalculs = gameObject.GetComponent<FluidCalculs>();
        _canvasFluid = gameObject.GetComponent<CanvasFluid>();
        /* Infos générales de l'environnement */
        mainCam = Camera.main;
        
        /* Création des tableaux */ 
        _size = (n+2)*(n+2);
        _dens = new float[_size];
        _densPrev = new float[_size];
        _velX = Enumerable.Repeat(0f, _size).ToArray();
        _velY = Enumerable.Repeat(0f, _size).ToArray();
        _velXPrev = new float[_size];
        _velYPrev = new float[_size];
        
        /* Texture dans le matériau */
        texture = new Texture2D(n+2, n+2, TextureFormat.RGBAHalf, false);
        texture.wrapMode = TextureWrapMode.Clamp; // eviter les debordements de la texture sur l'autre bord
        material.SetTexture("_MainTex", texture);
        if (_canvasFluid != null) _canvasFluid.DisplayTexture(texture);
        
        /* DEBUG */
        DrawGrid();

        _dens[30 + (n + 2) * 30] = 100;
        _velXPrev[30 + (n + 2) * 30] = -200;
        _velYPrev[30 + (n + 2) * 30] = -200;
    }


    void Update()
    {
        if (simulating)
        {
            GetFromUI();
            fluidCalculs.vel_step(n, ref _velX, ref _velY, ref _velXPrev, ref _velYPrev, visc, 0.1f);
            fluidCalculs.dens_step(n, ref _dens, ref _densPrev, ref _velX, ref _velY,diff,0.1f);
            DrawDensity();
        }
        VisualizeVelocity();
    }
    /* Gérée par l'input system */
    public void OnMousePosition(InputAction.CallbackContext cxt) 
    {
        _mouseScreenPos = cxt.ReadValue<Vector2>();
    }
    public void DeltaMouseValue(InputAction.CallbackContext cxt) 
    {
        _mouseDelta = cxt.ReadValue<Vector2>();
    }
    /*************************  Dessins  ************************************/
    private void DrawDensity() 
    {
        for (int i = 0; i < n+2; i++)
        {
            for (int j = 0; j < n+2; j++)
            {
                var col = _dens[IX(i,j,n)] > 10000 ? 10000 : _dens[IX(i,j,n)];
                // col /= 10000;
                texture.SetPixel(i, j, new UnityEngine.Color(col, col, col, 1f));
            }
        }
        texture.Apply();

        }
    /*************************UI Inputs************************************/
    private void GetFromUI()
    {
        for (int i = 0; i < _size; i++)
        {
            _densPrev[i] = 0;
        }
        bool left = Mouse.current.leftButton.isPressed;
        bool right = Mouse.current.rightButton.isPressed;
        if (!left && !right) return;
        
        Vector2Int coord = GetIdFromPosition(GetMousePos());
        int x = coord.x;
        int y = coord.y;
        if (x < 1 || x > n || y < 1 || y > n) {
            return;
        }

        if (simulating)
        {
            if (left)
            {
                _densPrev[x + y * (n + 2)] = source;
            }
            if (right)
            {
                //_velXPrev[x + y * (n + 2)] = force * _mouseDelta.x * (n + 2);
                //_velYPrev[x + y * (n + 2)] = force * _mouseDelta.y * (n + 2);
				double cste = force * Math.Exp((Math.Pow(x-_mouseDelta.x, 2) + Math.Pow(y-_mouseDelta.y, 2))/500);
				_velXPrev[x + y * (n + 2)] = (float) cste;
				_velYPrev[x + y * (n + 2)] = (float) cste;
                //_velXPrev[x + y * (n + 2)] = -200;
                //_velYPrev[x + y * (n + 2)] = -200;
            }
        }
        else
        {
            if (left)
            {
                //_dens[x + y * (n + 2)] = source;
                _densPrev[x + y * (n + 2)] = source;
            } else
            {
                var ycomp = force * _mouseDelta.y * (n + 2);
                var xcomp = force * _mouseDelta.x * (n + 2);
                _velXPrev[x + y * (n + 2)] = xcomp;
                _velX[x + y * (n + 2)] = xcomp;
                _velYPrev[x + y * (n + 2)] = ycomp;
                _velY[x + y * (n + 2)] = ycomp;
                Debug.Log("vel changer in " + x + " , " + y + " for a value of " + xcomp +" , " + ycomp);
                
            }
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
    
    /******************** DEBUG ******************************/
    private void DrawGrid()
    {
        float cellSize = 10f / (n + 2);

        for (int i = 0; i <= n + 2; i++)
        {
            float x = -5f + i * cellSize;

            // Dessiner des lignes verticales
            Vector3 startVertical = new Vector3(x, -5f, 0f);
            Vector3 endVertical = new Vector3(x, 5f, 0f);
            Debug.DrawLine(startVertical, endVertical, UnityEngine.Color.grey);

            // Dessiner des lignes horizontales (permutées)
            float y = -5f + i * cellSize;
            Vector3 startHorizontal = new Vector3(-5f, y, 0f);
            Vector3 endHorizontal = new Vector3(5f, y, 0f);
            Debug.DrawLine(startHorizontal, endHorizontal, UnityEngine.Color.grey);
        }
    }
    
    void VisualizeVelocity()
    {
        float cellSize = 10.0f / n;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Vector3 vel;
                Vector3 left_a;
                Vector3 right_a;
                Vector3 startPos = new Vector3(i * cellSize - 5f, - j * cellSize + 5f, 0.0f);
                UnityEngine.Color c;

                vel = new Vector3(_velX[i + j * (n + 2)], _velY[i + j * (n + 2)], 0.0f);
                c = UnityEngine.Color.Lerp(UnityEngine.Color.green, UnityEngine.Color.red, vel.magnitude / 0.08f);
                vel = vel.normalized * cellSize;

                left_a = Quaternion.Euler(0, 0, 30) * vel.normalized * vel.magnitude * 0.5f;
                right_a = Quaternion.Euler(0, 0, -30) * vel.normalized * vel.magnitude * 0.5f;

                Debug.DrawRay(startPos, vel, c, cellSize);
                Debug.DrawRay(startPos + vel, left_a, c, cellSize);
                Debug.DrawRay(startPos + vel, right_a, c, cellSize);
            }
        }
    }
    /******************** MACROS ******************************/
    private int IX(int i, int j, int N)
    {
        return i + (N + 2) * j;
    }
}
