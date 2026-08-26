using UnityEngine;

public class GPUBoidView : MonoBehaviour
{
    [SerializeField]
    private Mesh boidMesh;

    [SerializeField]
    private Material boidMaterial;

    public void Init(BoidArrayModel boids, int nbBoids)
    {
        boidMesh = boidPrefab.GetComponent<MeshFilter>().sharedMesh;

        buffer = new ComputeBuffer(nbBoids, boidStride);

        // Initialisation des données GPU
        boidBuffer.SetData(boids);

        // Association au shader de rendu
        material.SetBuffer("boids", buffer);
    }

    public void RefreshBoids(BoidArrayModel arrayModel, BoidSettings settings, int nbBoids, float dt)
    {
        for (int i = 0; i < nbBoids; i++)
        {
            BoidModel boidModel = arrayModel.getBoid(i);

            // Update the position
            boids[i].position = boidModel.Position;

            // Update the direction (rotation)
            if (boidModel.Direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(boidModel.Direction, Vector3.up);
                boids[i].rotation = Quaternion.RotateTowards(boids[i].rotation, targetRotation, settings.RotationSpeed * dt); 
            }
        }
    }

}