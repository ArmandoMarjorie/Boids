using UnityEngine;

public class BoidModel
{
    /* ------------- ATTRIBUTES ------------- */
    // Boid's current position
    private Vector3 position;
    // Boid's current direction
    private Vector3 direction;

    /* ------------- GETTERS AND SETTERS ------------- */
    public Vector3 Position { get => position; set => position = value; }
    public float PositionX { get => position.x; set => position.x = value; }
    public float PositionY { get => position.y; set => position.y = value; }
    public float PositionZ { get => position.z; set => position.z = value; }

    public Vector3 Direction { get => direction; set => direction = value; }
    public float DirectionX { get => direction.x; set => direction.x = value; }
    public float DirectionY { get => direction.y; set => direction.y = value; }
    public float DirectionZ { get => direction.z; set => direction.z = value; }

    /* ------------- CONSTRUCTOR ------------- */
    public BoidModel(Vector3 startPosition,
        Vector3 startDirection)
    {
        // Boid's state
        this.position = startPosition;
        this.direction = startDirection;
    }

}