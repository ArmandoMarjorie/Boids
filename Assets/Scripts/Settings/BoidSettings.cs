using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName = "Boids/Boid Settings")]

public class BoidSettings : ScriptableObject
{
    [SerializeField]
    private float speed = 15f;

    [SerializeField] 
    private float maxSpeed = 25f;

    [SerializeField] 
    private float rotationSpeed = 180f;

    [SerializeField]
    private int fieldOfVision = 50;

    [SerializeField]
    private float maneuverability = 0.05f;

    [SerializeField]
    private int minDistance = 20; // La distance où un oiseau reste éloignée d'un autre oiseau

    [SerializeField]
    private float cohesionWeight = 1f;

    [SerializeField]
    private float alignmentWeight = 1f;

    [SerializeField]
    private float separationWeight = 1.5f;

    public float Speed { get => speed; set => speed = value; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
    public int FieldOfVision { get => fieldOfVision; set => fieldOfVision = value; }
    public float Maneuverability { get => maneuverability; set => maneuverability = value; }
    public int MinDistance { get => minDistance; set => minDistance = value; }

    public float CohesionWeight { get => cohesionWeight; set => cohesionWeight = value; }
    public float AlignmentWeight { get => alignmentWeight; set => alignmentWeight = value; }
    public float SeparationWeight { get => separationWeight; set => separationWeight = value; } 
}