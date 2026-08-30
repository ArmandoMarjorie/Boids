using UnityEngine;

public class GPUBoidView : MonoBehaviour
{
    [SerializeField]
    private GameObject boidPrefab;

    [SerializeField]
    private Material boidMaterial = null; // create a material with the shader "BoidShader" and assign it in the inspector

    private Mesh boidMesh = null;
    private ComputeBuffer graphicsBuffer = null;
    private uint[] graphics = null;

    public void Init(ComputeBuffer buffer, int nbBoids)
    {
        SkinnedMeshRenderer skinnedMeshRenderer = boidPrefab.GetComponentInChildren<SkinnedMeshRenderer>();

        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer not found.");
            Application.Quit();
        }

        boidMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(boidMesh);

        graphics = new uint[5] 
        {
            boidMesh.GetIndexCount(0), 
            (uint)nbBoids, 
            boidMesh.GetIndexStart(0), 
            boidMesh.GetBaseVertex(0), 
            0 
        };

        graphicsBuffer = new ComputeBuffer(1,
            graphics.Length * sizeof(uint),
            ComputeBufferType.IndirectArguments);

        graphicsBuffer.SetData(graphics);

        boidMaterial.SetBuffer("boids", buffer);
    }

    public void RefreshBoids()
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
        Graphics.DrawMeshInstancedIndirect(boidMesh, 0, boidMaterial, bounds, graphicsBuffer);
    }

    public void OnDestroy()
    {
        if (graphicsBuffer != null)
        {
            graphicsBuffer.Release();
            graphicsBuffer = null;
        }
    }

}