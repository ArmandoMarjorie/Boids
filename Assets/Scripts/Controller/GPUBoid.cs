using UnityEngine;
using System.Collections.Generic;

/* GPU Boid Simulation Class
 * This class is responsible for managing the GPU-based boid simulation.
 * It handles the initialization of the compute shader, updating boid positions and directions,
 * and applying the boid rules (separation, alignment, cohesion) using GPU parallel processing.
 */
public class GPUBoid
{
    /* ------------- GPU SETTINGS ------------- */
    private ComputeShader computeShader = null;
    private ComputeBuffer buffer = null;
    private int indexKernel;

    /* ------------- BOID SETTINGS ------------- */
    private BoidView boidView = null;
    private BoidSettings boidSettings = null;
    private BoundsSettings boundsSettings = null;
    private int nbBoids;

    public GPUBoid(ComputeShader cs,
        int nb,
        BoidView view,
        BoidSettings bSettings,
        BoundsSettings boSettings)
    {
        /* ------------- GPU INITIALIZATION ------------- */
        if (cs != null)
            computeShader = cs;
        else
        {
            Debug.LogError("ComputeShader is not assigned in the inspector.");
            Application.Quit();
        }
        nbBoids = nb;
        indexKernel = computeShader.FindKernel("Update");
        buffer = new ComputeBuffer(nbBoids, sizeof(float) * 6); // à revoir pour size

        /* ------------- BOID SETTINGS INITIALIZATION ------------- */
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
    }

    public void InitGPUData(float dt) // dt = Time.deltaTime
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
        computeShader.SetBuffer(indexKernel, "boids", buffer);
        computeShader.SetFloat("dt", dt);

        computeShader.SetInt("nbBoids", nbBoids);
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
    }

    /* ----------------------------------------------------------------------------- */


    public void Update()
    {
        /*
        int nbBoids = boidArrayModel.NbBoids;

        // Update the desired direction for each boid based on the rules
        for (int i = 0; i < nbBoids; i++)
        {
            BoidModel boid = boidArrayModel.getBoid(i);
            List<BoidModel> visibleNeighbors = GetVisibleNeighbors(boid);
            List<BoidModel> neighborsAround = GetNeighborsAround(boid);
            calculDirection(boid, visibleNeighbors, neighborsAround);
        }
        // Update the position for each boid based on the desired direction
        for (int i = 0; i < nbBoids; i++)
        {
            BoidModel boidModel = boidArrayModel.getBoid(i);
            boidModel.Position += boidModel.Direction * boidSettings.Speed * dt;
            KeepInsideBounds(boidModel);
        }

        boidView.RefreshBoids(boidArrayModel, boidSettings, nbBoids, dt);
        */
    }
}