using UnityEngine;

[CreateAssetMenu(fileName = "BoundsSettings", menuName = "Simulation/Bounds Settings")]

public class BoundsSettings : ScriptableObject
{
    [SerializeField]
    private int width = 60;

    [SerializeField]
    private int height = 60;

    [SerializeField]
    private int depth = 60;

    [SerializeField]
    private float margin = 20f;

    [SerializeField]
    private Vector3 center = Vector3.zero;

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public int Depth { get => depth; set => depth = value; }
    public float Margin { get => margin; set => margin = value; }
    public Vector3 Center { get => center; set => center = value; }
}