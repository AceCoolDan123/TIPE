using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[StructLayout(LayoutKind.Sequential, Size = 44)]
public struct Particle
{
    public float pressure; // 4
    public float density; // 8
    public Vector3 currentForce; // 20
    public Vector3 velocity; // 32 
    public Vector3 position; // 44
}

public class SPH : MonoBehaviour
{
    [Header("General")] public bool showSphere = true;
    public Vector3Int numToSpawn = new Vector3Int(10, 10, 10);
    private int totalParticles
    {
        get { return numToSpawn.x * numToSpawn.y * numToSpawn.z; }
    }
    public Vector3 boxSize= new Vector3(4,10,3);
    public Vector3 spawnCenter;
    public float particleRadius = 0.1f;

    [Header("Particle Rendering")] 
    public Mesh particleMesh;
    public float particleRenderSize = 8f;
    public Material material;

    [Header("Compute")] 
    public ComputeShader shader;
    public Particle[] particles;
    private ComputeBuffer _argsBuffer;
    private ComputeBuffer _particleBuffer;

    private void Awake()
    {
        // SpawnParticlesInBox();
        uint[] args =
        {
            particleMesh.GetIndexCount(0),
            (uint)totalParticles,
            particleMesh.GetIndexStart(0),
            particleMesh.GetBaseVertex(0),
            0
        };

        _argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        _argsBuffer.SetData(args);

        _particleBuffer = new ComputeBuffer(totalParticles, 44);
        _particleBuffer.SetData(particles);

    }

    private static readonly int SizeProperty = Shader.PropertyToID("_size");
    private static readonly int ParticleBufferProperty = Shader.PropertyToID("_particlesBuffer");

    private void Update()
    {
        material.SetFloat(SizeProperty, particleRenderSize);
        material.SetBuffer(ParticleBufferProperty,_particleBuffer);
        
        if(showSphere) 
            Graphics.DrawMeshInstancedIndirect(particleMesh, 
                0,
                material, 
                new Bounds(Vector3.zero, boxSize), 
                _argsBuffer, 
                castShadows: UnityEngine.Rendering.ShadowCastingMode.Off);
        
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        if (!Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(spawnCenter, 2f);
        }
        /*else {
            // Show Particles
            foreach (Particle p in particles) {

                // if (!p.onBoundary)
                // Gizmos.color = Color.white;
                // else
                // Gizmos.color = Color.red;

                Gizmos.color = Color.cyan;

                if (!wireframeSpheres)
                    Gizmos.DrawSphere(p.position, particleRadius);
                else
                    Gizmos.DrawWireSphere(p.position, particleRadius);
            }
        }*/
    }

}
