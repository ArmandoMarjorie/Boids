using UnityEngine;

public class BoidView : MonoBehaviour
{
    [SerializeField] 
    private GameObject boidPrefab;

    private Transform[] boids;

    public void Init(BoidArrayModel arrayModel, int nbBoids)
    {
        boids = new Transform[nbBoids];

        for (int i = 0; i < nbBoids; i++)
        {
            BoidModel boidModel = arrayModel.getBoid(i);
            GameObject go = Instantiate(boidPrefab, transform);
            go.name = $"Boid {i}";
            boids[i] = go.transform;
            boids[i].position = boidModel.Position;
        }
    }

    public void RefreshBoids(BoidArrayModel arrayModel, BoidSettings settings, int nbBoids)
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
                boids[i].rotation = Quaternion.RotateTowards(boids[i].rotation, targetRotation, settings.RotationSpeed); 
            }
        }
    }

}