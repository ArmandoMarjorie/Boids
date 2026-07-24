using UnityEngine;

public class BoidController
{
    private BoidArrayModel boidArrayModel = null;
    private BoidView boidView = null;
    private BoidSettings boidSettings = null;
    private BoundsSettings boundsSettings = null;

    public BoidController(BoidArrayModel model,
        BoidView view,
        BoidSettings bSettings,
        BoundsSettings boSettings)
    {
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
    }

    public void Update(float dt)
    {
        int nbBoids = boidArrayModel.NbBoids;

        // Update the desired direction for each boid based on the rules
        for (int i = 0; i < nbBoids; i++)
        {
            calculDirection(boidArrayModel.getBoid(i));
        }
        // Update the position for each boid based on the desired direction
        for (int i = 0; i < nbBoids; i++)
        {
            BoidModel boidModel = boidArrayModel.getBoid(i);
            boidModel.Position += boidModel.Direction * boidSettings.Speed * dt;
        }

        boidView.RefreshBoids(boidArrayModel, boidSettings, nbBoids, dt);
    }

    /* Returns true if boid 2 is within the field of vision of boid 1, false otherwise */
    private bool IsNeighbor(BoidModel boid1, BoidModel boid2)
    {
        Vector3 towardBoid2 = boid2.Position - boid1.Position;

        // Check if boid2 is around boid1
        if (towardBoid2.sqrMagnitude > boidSettings.RayonAround * boidSettings.RayonAround)
            return false;

        // Check if boid2 is within the field of vision
        float angle = Vector3.Angle(boid1.Direction, towardBoid2);

        return angle < boidSettings.FieldOfVision / 2f;
    }

    private Vector3 StayInBoundaries(BoidModel boid)
    {
        Vector3 v = Vector3.zero;

        float halfWidth = boundsSettings.Width / 2f;
        float halfHeight = boundsSettings.Height / 2f;
        float halfDepth = boundsSettings.Depth / 2f;

        // Width boundaries
        if (boid.PositionX > halfWidth - boundsSettings.Margin)
            v.x = -((boid.PositionX - (halfWidth - boundsSettings.Margin)) / boundsSettings.Margin);
        else if (boid.PositionX < -halfWidth + boundsSettings.Margin)
            v.x = ((-halfWidth + boundsSettings.Margin) - boid.PositionX) / boundsSettings.Margin;

        // Height boundaries
        if (boid.PositionY > halfHeight - boundsSettings.Margin)
            v.y = -((boid.PositionY - (halfHeight - boundsSettings.Margin)) / boundsSettings.Margin);
        else if (boid.PositionY < -halfHeight + boundsSettings.Margin)
            v.y = ((-halfHeight + boundsSettings.Margin) - boid.PositionY) / boundsSettings.Margin;

        // Depth boundaries
        if (boid.PositionZ > halfDepth - boundsSettings.Margin)
            v.z = -((boid.PositionZ - (halfDepth - boundsSettings.Margin)) / boundsSettings.Margin);
        else if (boid.PositionZ < -halfDepth + boundsSettings.Margin)
            v.z = ((-halfDepth + boundsSettings.Margin) - boid.PositionZ) / boundsSettings.Margin;

        if (v != Vector3.zero)
        {
            v.Normalize();
            v *= boidSettings.Maneuverability;
        }

        return v;
    }

    private void calculDirection(BoidModel boid)
    {
        Vector3 towardDirection = Separation(boid) + 
            Cohesion(boid) + 
            Alignment(boid) + 
            StayInBoundaries(boid);

        boid.Direction = (1 - boidSettings.Maneuverability) * boid.Direction +
            boidSettings.Maneuverability * towardDirection;

        boid.Direction.Normalize();
    }

    /* ------------- BOIDS RULES ------------- */

    /* Separation: steer to avoid crowding local flockmates */
    private Vector3 Separation(BoidModel boid)
    {
        Vector3 diff = Vector3.zero;
        Vector3 v1 = Vector3.zero;
        float sqrDistance = 0f;

        // We want to get an average, so we will divide by the number of neighbors
        int nbNeighbors = 0;

        for (int i = 0; i < boidArrayModel.NbBoids; i++) 
	    {
            BoidModel otherBoid = boidArrayModel.getBoid(i);

            if (otherBoid != boid) // Boid can't be a neighbor of itself 
		    {

                diff = boid.Position - otherBoid.Position;
                sqrDistance = diff.sqrMagnitude;

                if (sqrDistance > 0f && sqrDistance < boidSettings.MinDistance * boidSettings.MinDistance)
                {
                    diff.Normalize();
                    v1 += diff / sqrDistance;
                    nbNeighbors++;
                }
                   
            }
	    }

        if (nbNeighbors > 0)
        {
            v1 /= (float)nbNeighbors;
            v1.Normalize();
            v1 = (v1 - boid.Direction) * boidSettings.SeparationWeight; 
        }

        return v1;
    }

    /* Alignment: steer towards the average heading of local flockmates */
    private Vector3 Alignment(BoidModel boid)
    {
        Vector3 v2 = Vector3.zero;

        // We want to get an average, so we will divide by the number of neighbors
        int nbNeighbors = 0;

        for (int i = 0; i < boidArrayModel.NbBoids; i++)
        {
            BoidModel otherBoid = boidArrayModel.getBoid(i);

            if (otherBoid != boid) 
		    {
			    if ( IsNeighbor(boid, otherBoid) ) 
			    {
				    v2 += otherBoid.Direction;
                    nbNeighbors++;
			    }
		    }
	    }

	    if (nbNeighbors > 0)
        {
            // calculation of the average direction taken by the neighbors
            v2 /= (float)nbNeighbors;
            v2.Normalize();
            v2 = (v2 - boid.Direction) * boidSettings.AlignmentWeight;
        }

        return v2;
    }

    /* Cohesion: steer to move towards the average position (center of mass) of local flockmates */
    private Vector3 Cohesion(BoidModel boid)
    {
        Vector3 centerOfMass = Vector3.zero;
        Vector3 v3 = Vector3.zero;

        int nbNeighbors = 0;

        for (int i = 0; i < boidArrayModel.NbBoids; i++)
        {
            BoidModel otherBoid = boidArrayModel.getBoid(i);

            if (otherBoid != boid)
            {
                if (IsNeighbor(boid, otherBoid))
                {
                    centerOfMass += otherBoid.Position;
                    nbNeighbors++;
                }
            }
        }

        if (nbNeighbors > 0)
        {
            // calculation of the average position of the neighbors (center of mass)
            centerOfMass /= (float)nbNeighbors;

            // direction toward the center
            v3 = (centerOfMass - boid.Position);
            v3.Normalize();
            v3 = (v3 - boid.Direction) * boidSettings.CohesionWeight;
        }

        return v3;
    }

}