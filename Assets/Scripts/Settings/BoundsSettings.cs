using UnityEngine;

[CreateAssetMenu(fileName = "BoundsSettings", menuName = "Simulation/Bounds Settings")]

public class BoundsSettings : ScriptableObject
{
    [SerializeField]
    private int width = 100;

    [SerializeField]
    private int height = 100;

    [SerializeField]
    private int depth = 100;

    [SerializeField]
    private float margin = 20f;

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public int Depth { get => depth; set => depth = value; }
    public float Margin { get => margin; set => margin = value; }
}