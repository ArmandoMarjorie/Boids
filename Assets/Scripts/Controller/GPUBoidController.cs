using UnityEngine;
using System.Collections.Generic;

/* GPU Boid Simulation Class
 * This class is responsible for managing the GPU-based boid simulation.
 * It handles the initialization of the compute shader, updating boid positions and directions,
 * and applying the boid rules (separation, alignment, cohesion) using GPU parallel processing.
 */
public class GPUBoidController
{
    /* ------------- BOID SETTINGS ------------- */
    private BoidArrayModel boidArrayModel = null;
    private GPUBoidView boidView = null;
    private BoidSettings boidSettings = null;
    private BoundsSettings boundsSettings = null;

    /* ------------- GPU SETTINGS ------------- */
    private ComputeShader computeShader = null;
    private ComputeBuffer buffer = null;
    private int idKernelDirections = 0;
    private int idKernelPositions = 0;
    private int nbThreadsPerGroups = 256; // /!\ have to match with NB_THREADS_PER_GROUP in Shaders/Boid.compute
    private int nbGroupsThreads = 0;

    public GPUBoidController(BoidArrayModel model,
        ComputeShader cs,
        GPUBoidView view,
        BoidSettings bSettings,
        BoundsSettings boSettings)
    {
        /* ------------- BOID SETTINGS INITIALIZATION ------------- */
        if (model != null)
            boidArrayModel = model;
        else
        {
            Debug.LogError("boidArrayModel is not assigned in the inspector.");
            Application.Quit();
        }
        if (view != null)
            boidView = view;
        else
        {
            Debug.LogError("BoidView is not assigned in the inspector.");
            Application.Quit();
        }
        if (bSettings != null)
            boidSettings = bSettings;
        else
        {
            Debug.LogError("boidSettings is not assigned in the inspector.");
            Application.Quit();
        }
        if (boSettings != null)
            boundsSettings = boSettings;
        else
        {
            Debug.LogError("boundsSettings is not assigned in the inspector.");
            Application.Quit();
        }

        /* ------------- GPU INITIALIZATION ------------- */
        if (cs != null)
            computeShader = cs;
        else
        {
            Debug.LogError("ComputeShader is not assigned in the inspector.");
            Application.Quit();
        }
        idKernelDirections = computeShader.FindKernel("UpdateDirections");
        idKernelPositions = computeShader.FindKernel("UpdatePositions");
        buffer = new ComputeBuffer(boidArrayModel.NbBoids, sizeof(float) * 6); // 3 floats for position + 3 floats for direction
        nbGroupsThreads = Mathf.CeilToInt(boidArrayModel.NbBoids / (float)nbThreadsPerGroups);
    }

    public void InitGPUData()
    {
        if (computeShader == null)
        {
            Debug.LogError("ComputeShader is not assigned in the inspector.");
            Application.Quit();
        }
        if (boidSettings == null)
        {
            Debug.LogError("boidSettings is not assigned in the inspector.");
            Application.Quit();
        }
        if (boundsSettings == null)
        {
            Debug.LogError("boundsSettings is not assigned in the inspector.");
            Application.Quit();
        }
        computeShader.SetBuffer(idKernelDirections, "boids", buffer);
        computeShader.SetBuffer(idKernelPositions, "boids", buffer);
        buffer.SetData(boidArrayModel.getArray());

        computeShader.SetInt("nbBoids", boidArrayModel.NbBoids);
        computeShader.SetFloat("speed", boidSettings.Speed);
        computeShader.SetFloat("rotationSpeed", boidSettings.RotationSpeed);
        computeShader.SetInt("fieldOfVision", boidSettings.FieldOfVision);
        computeShader.SetFloat("maneuverability", boidSettings.Maneuverability);
        computeShader.SetInt("minDistance", boidSettings.MinDistance);
        computeShader.SetInt("rayonAround", boidSettings.RayonAround);
        computeShader.SetInt("maxAngleVertical", boidSettings.MaxAngleVertical);
        computeShader.SetFloat("boundsWeight", boidSettings.BoundsWeight);
        computeShader.SetFloat("cohesionWeight", boidSettings.CohesionWeight);
        computeShader.SetFloat("alignmentWeight", boidSettings.AlignmentWeight);
        computeShader.SetFloat("separationWeight", boidSettings.SeparationWeight);

        computeShader.SetFloat("width", boundsSettings.Width);
        computeShader.SetFloat("height", boundsSettings.Height);
        computeShader.SetFloat("depth", boundsSettings.Depth);
        computeShader.SetFloat("margin", boundsSettings.Margin);
        computeShader.SetVector("center", boundsSettings.Center);
        
        boidView.Init(buffer, boidArrayModel.NbBoids);
    }

    public void Update(float dt)
    {
        // Compute
        computeShader.SetFloat("dt", dt);
        computeShader.Dispatch(idKernelDirections, nbGroupsThreads, 1, 1);
        computeShader.Dispatch(idKernelPositions, nbGroupsThreads, 1, 1);

        // Render
        boidView.RefreshBoids();
    }

    // todo: check if i can do that
    public void OnDestroy()
    {
        if (buffer != null)
        {
            buffer.Release();
            buffer = null;
        }
    }
}