using UnityEngine;

[CreateAssetMenu(fileName = "BoundsSettings", menuName = "Simulation/Bounds Settings")]

public class BoundsSettings : ScriptableObject
{
    [SerializeField]
    private float width = 60;

    [SerializeField]
    private float height = 60;

    [SerializeField]
    private float depth = 60;

    [SerializeField]
    private float margin = 20f;

    [SerializeField]
    private Vector3 center = Vector3.zero;

    public float Width { get => width; set => width = value; }
    public float Height { get => height; set => height = value; }
    public float Depth { get => depth; set => depth = value; }
    public float Margin { get => margin; set => margin = value; }
    public Vector3 Center { get => center; set => center = value; }
}