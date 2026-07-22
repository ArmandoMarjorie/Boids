using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName = "Boids/Boid Settings")]

public class BoidSettings : ScriptableObject
{
    /*[SerializeField]
    private GameObject boidGO;*/

    [SerializeField]
    private float speed = 2f;

    [SerializeField] 
    private float maxSpeed = 5f;

    [SerializeField] 
    private float rotationSpeed = 0.5f;

    [SerializeField]
    private int fieldOfVision = 75;

    [SerializeField]
    private int minDistance = 20; // La distance où un oiseau reste éloignée d'un autre oiseau

    [SerializeField]
    private float cohesionWeight = 1f;

    [SerializeField]
    private float alignmentWeight = 1f;

    [SerializeField]
    private float separationWeight = 2f;

    public float Speed { get => speed; set => speed = value; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
    public int FieldOfVision { get => fieldOfVision; set => fieldOfVision = value; }
    public int MinDistance { get => minDistance; set => minDistance = value; }

    public float CohesionWeight { get => cohesionWeight; set => cohesionWeight = value; }
    public float AlignmentWeight { get => alignmentWeight; set => alignmentWeight = value; }
    public float SeparationWeight { get => separationWeight; set => separationWeight = value; } 
}