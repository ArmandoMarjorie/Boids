using UnityEngine;
using System.Collections.Generic;

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

        boidView.RefreshBoids(boidArrayModel, boidSettings.RotationSpeed, dt);
    }

    /* Returns true if boid 2 is within the field of vision of boid 1, false otherwise.
     * Useful for determining which boids are visible to a given boid.
     */
    private bool IsVisibleNeighbor(BoidModel boid1, BoidModel boid2)
    {
        Vector3 towardBoid2 = boid2.Position - boid1.Position;

        // Check if boid2 is around boid1
        if (towardBoid2.sqrMagnitude > boidSettings.RayonAround * boidSettings.RayonAround)
            return false;

        // Check if boid2 is within the field of vision
        float angle = Vector3.Angle(boid1.Direction, towardBoid2);

        return angle < boidSettings.FieldOfVision / 2f;
    }

    /* Returns a list of visible neighbors for the given boid.
     * Useful for calculating cohesion and alignment forces. 
     */
    private List<BoidModel> GetVisibleNeighbors(BoidModel boid)
    {
        List<BoidModel> neighbors = new List<BoidModel>();

        for (int i = 0; i < boidArrayModel.NbBoids; i++)
        {
            BoidModel otherBoid = boidArrayModel.getBoid(i);
            if (otherBoid != boid && IsVisibleNeighbor(boid, otherBoid))
                neighbors.Add(otherBoid);
        }

        return neighbors;
    }

    /* Returns true if boid 2 is around boid 1, false otherwise. */
    private bool IsNeighborAround(BoidModel boid1, BoidModel boid2)
    {
        Vector3 towardBoid2 = boid2.Position - boid1.Position;

        return towardBoid2.sqrMagnitude > 0f && towardBoid2.sqrMagnitude < boidSettings.MinDistance * boidSettings.MinDistance;
    }

    /* Returns a list of neighbors around the given boid.
     * Useful for calculating separation forces.
     */
    private List<BoidModel> GetNeighborsAround(BoidModel boid)
    {
        List<BoidModel> neighbors = new List<BoidModel>();

        for (int i = 0; i < boidArrayModel.NbBoids; i++)
        {
            BoidModel otherBoid = boidArrayModel.getBoid(i);
            if (otherBoid != boid && IsNeighborAround(boid, otherBoid))
                neighbors.Add(otherBoid);
        }

        return neighbors;
    }

    private Vector3 ApplyForceToStayInBoundaries(BoidModel boid)
    {
        Vector3 v = Vector3.zero;

        float minWidth = boundsSettings.Center.x - boundsSettings.Width / 2f;
        float minHeight = boundsSettings.Center.y - boundsSettings.Height / 2f;
        float minDepth = boundsSettings.Center.z - boundsSettings.Depth / 2f;

        float maxWidth = boundsSettings.Center.x + boundsSettings.Width / 2f;
        float maxHeight = boundsSettings.Center.y + boundsSettings.Height / 2f;
        float maxDepth = boundsSettings.Center.z + boundsSettings.Depth / 2f;

        // Width boundaries
        if (boid.PositionX > maxWidth - boundsSettings.Margin)
            v.x = -( (boid.PositionX - (maxWidth - boundsSettings.Margin)) / boundsSettings.Margin );

        else if (boid.PositionX < minWidth + boundsSettings.Margin)
            v.x = ( (minWidth + boundsSettings.Margin) - boid.PositionX ) / boundsSettings.Margin;

        // Height boundaries
        if (boid.PositionY > maxHeight - boundsSettings.Margin)
            v.y = -( (boid.PositionY - (maxHeight - boundsSettings.Margin)) / boundsSettings.Margin );

        else if (boid.PositionY < minHeight + boundsSettings.Margin)
            v.y = ( (minHeight + boundsSettings.Margin) - boid.PositionY ) / boundsSettings.Margin;

        // Depth boundaries
        if (boid.PositionZ > maxDepth - boundsSettings.Margin)
            v.z = -( (boid.PositionZ - (maxDepth - boundsSettings.Margin)) / boundsSettings.Margin );

        else if (boid.PositionZ < minDepth + boundsSettings.Margin)
            v.z = ( (minDepth + boundsSettings.Margin) - boid.PositionZ ) / boundsSettings.Margin;

        if (v != Vector3.zero)
        {
            v.Normalize();
            v *= boidSettings.BoundsWeight;
        }

        return v;
    }

    private void KeepInsideBounds(BoidModel boid)
    {
        float minWidth = boundsSettings.Center.x - boundsSettings.Width / 2f;
        float minHeight = boundsSettings.Center.y - boundsSettings.Height / 2f;
        float minDepth = boundsSettings.Center.z - boundsSettings.Depth / 2f;

        float maxWidth = boundsSettings.Center.x + boundsSettings.Width / 2f;
        float maxHeight = boundsSettings.Center.y + boundsSettings.Height / 2f;
        float maxDepth = boundsSettings.Center.z + boundsSettings.Depth / 2f;

        boid.PositionX = Mathf.Clamp(boid.PositionX, minWidth, maxWidth);
        boid.PositionY = Mathf.Clamp(boid.PositionY, minHeight, maxHeight);
        boid.PositionZ = Mathf.Clamp(boid.PositionZ, minDepth, maxDepth);
    }

    private Vector3 LimitVerticalAngle(Vector3 direction) 
    {
        // sin of the max vertical angle (convert to radian)
        float maxAngle = Mathf.Sin(boidSettings.MaxAngleVertical * Mathf.Deg2Rad);

        direction.y = Mathf.Clamp(direction.y, -maxAngle, maxAngle); 

        return direction.normalized; 
    }

    private void calculDirection(BoidModel boid, List<BoidModel> visibleNeighbors, List<BoidModel> neighborsAround)
    {
        Vector3 directionCorrection = Separation(boid, neighborsAround) + 
            Cohesion(boid, visibleNeighbors) + 
            Alignment(boid, visibleNeighbors) +
            ApplyForceToStayInBoundaries(boid);

        Vector3 newDirection = boid.Direction + directionCorrection;

        if (newDirection.sqrMagnitude > 0f)
        {
            newDirection.Normalize();

            boid.Direction = (1 - boidSettings.Maneuverability) * boid.Direction +
                boidSettings.Maneuverability * newDirection;

            boid.Direction.Normalize();

            boid.Direction = LimitVerticalAngle(boid.Direction);
        }
    }

    /* ------------- BOIDS RULES ------------- */

    /* Separation: steer to avoid crowding local flockmates */
    private Vector3 Separation(BoidModel boid, List<BoidModel> neighbors)
    {
        Vector3 diff = Vector3.zero;
        Vector3 v1 = Vector3.zero;
        float sqrDistance = 0f;

        // We want to get an average, so we will divide by the number of neighbors
        int nbNeighbors = neighbors.Count;

        for (int i = 0; i < nbNeighbors; i++)
        {
            diff = boid.Position - neighbors[i].Position;
            sqrDistance = diff.sqrMagnitude;
            diff.Normalize();
            v1 += diff / sqrDistance;
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
    private Vector3 Alignment(BoidModel boid, List<BoidModel> neighbors)
    {
        Vector3 v2 = Vector3.zero;

        int nbNeighbors = neighbors.Count;

        for (int i = 0; i < nbNeighbors; i++)
            v2 += neighbors[i].Direction;

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
    private Vector3 Cohesion(BoidModel boid, List<BoidModel> neighbors)
    {
        Vector3 centerOfMass = Vector3.zero;
        Vector3 v3 = Vector3.zero;

        int nbNeighbors = neighbors.Count;

        for (int i = 0; i < nbNeighbors; i++)
            centerOfMass += neighbors[i].Position;

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