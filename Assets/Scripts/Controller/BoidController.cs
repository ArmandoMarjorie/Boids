using UnityEngine;

public class BoidController
{
    private BoidArrayModel boidArrayModel = null;
    private BoidView boidView = null;
    private BoidSettings boidSettings = null;
    private BoundsSettings boundsSettings = null;

    private Vector3 v;

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
        v = new Vector3(0, 0, 0);
    }

    public void Update(float dt)
    {
        int nbBoids = boidArrayModel.NbBoids;

        // Update of every boids
        for (int i = 0; i < nbBoids; i++)
        {
            BoidModel boidModel = boidArrayModel.getBoid(i);

            // Boids' rules
            Cohesion(boidModel);
            Separation(boidModel);
            Alignment(boidModel);
            //LimitSpeed(boidModel);
            StayInBoundaries(boidModel);

            // Update position
            //boidModel.Direction.Normalize();
            boidModel.Position += boidModel.Direction * boidSettings.Speed * dt;

            // On garde en mémoire le parcours de l'oiseau
            /*oiseau.history.push([oiseau.x, oiseau.y])
    
            oiseau.history = oiseau.history.slice(-50);*/
        }

        boidView.RefreshBoids(boidArrayModel, boidSettings, nbBoids);
    }

    /* Returns the distance between boid 1 and boid 2 */
    private float Distance(BoidModel boid1, BoidModel boid2)
    {
        return Mathf.Sqrt(
            (boid1.PositionX - boid2.PositionX) * (boid1.PositionX - boid2.PositionX) +
            (boid1.PositionY - boid2.PositionY) * (boid1.PositionY - boid2.PositionY) +
            (boid1.PositionZ - boid2.PositionZ) * (boid1.PositionZ - boid2.PositionZ)
        );
    }

    /* Returns true if boid 2 is within the field of vision of boid 1, false otherwise */
    private bool IsNeighbor(BoidModel boid1, BoidModel boid2, int fieldOfVision)
    {
        return Distance(boid1, boid2) < fieldOfVision;
    }

    /* The speed can be excessive, so it is limited to a certain point */
    private void LimitSpeed(BoidModel boid)
    {
        float speed = Mathf.Sqrt(
            boid.DirectionX * boid.DirectionX + 
            boid.DirectionY * boid.DirectionY + 
            boid.DirectionZ * boid.DirectionZ
        );

        if (speed > boidSettings.MaxSpeed)
            boid.Direction = (boid.Direction / speed) * boidSettings.MaxSpeed;
    }

    private void StayInBoundaries(BoidModel boid)
    {
        v = Vector3.zero;

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
            boid.Direction += v * boidSettings.RotationSpeed;
        }
    }

    /* ------------- BOIDS RULES ------------- */

    /* Separation: steer to avoid crowding local flockmates */
    private void Separation(BoidModel boid)
    {
        v = Vector3.zero;

        for (int i = 0; i < boidArrayModel.NbBoids; i++) 
	    {
            BoidModel otherBoid = boidArrayModel.getBoid(i);

            if (otherBoid != boid) // Boid can't be a neighbor of itself 
		    {
                // We want to head in the opposite direction to our neighbors to avoid collisions
                if ( IsNeighbor(boid, otherBoid, boidSettings.MinDistance) )  
                    v += (boid.Position - otherBoid.Position);
		    }
	    }

        if (v != Vector3.zero)
        {
            v.Normalize();
            boid.Direction += v * boidSettings.SeparationWeight;
        }
    }

    /* Alignment: steer towards the average heading of local flockmates */
    private void Alignment(BoidModel boid)
    {
        v = Vector3.zero;

        // We want to get an average, so we will divide by the number of neighbors
        int nbNeighbors = 0;

        for (int i = 0; i < boidArrayModel.NbBoids; i++)
        {
            BoidModel otherBoid = boidArrayModel.getBoid(i);

            if (otherBoid != boid) 
		    {
			    if ( IsNeighbor(boid, otherBoid, boidSettings.FieldOfVision) ) 
			    {
				    v += otherBoid.Direction;
                    nbNeighbors++;
			    }
		    }
	    }

	    if (nbNeighbors > 0 && v != Vector3.zero)
        {
            // calculation of the average direction taken by the neighbors
            v /= (float)nbNeighbors;

            v.Normalize();

            // We subtract the bird's speed to align with the speeds of its neighbors
            boid.Direction += (v - boid.Direction) * boidSettings.AlignmentWeight;
        }
    }

    /* Cohesion: steer to move towards the average position (center of mass) of local flockmates */
    private void Cohesion(BoidModel boid)
    {
        v = Vector3.zero;

        int nbNeighbors = 0;

        for (int i = 0; i < boidArrayModel.NbBoids; i++)
        {
            BoidModel otherBoid = boidArrayModel.getBoid(i);

            if (otherBoid != boid)
            {
                if (IsNeighbor(boid, otherBoid, boidSettings.FieldOfVision))
                {
                    v += otherBoid.Position;
                    nbNeighbors++;
                }
            }
        }

        if (nbNeighbors > 0 && v != Vector3.zero)
        {
            // calculation of the average position of the neighbors (center of mass)
            v /= (float)nbNeighbors;

            v.Normalize();

            // we subtract the bird's position from the center of mass to stay cohesive
            boid.Direction += (v - boid.Position) * boidSettings.CohesionWeight;
        }
    }

}