using UnityEngine;

public class BoidModel
{
    /* ------------- ATTRIBUTES ------------- */
    // Boid's current position
    private Vector3 position;
    // Boid's current direction
    private Vector3 direction;

    // History of the boid's positions, used for visualization purposes.
    private Vector3[] history;
    private int historyMaxSize;
    private int historyIndex;
    private int historySize;

    /* ------------- GETTERS AND SETTERS ------------- */
    public Vector3 Position { get => position; set => position = value; }
    public float PositionX { get => position.x; set => position.x = value; }
    public float PositionY { get => position.y; set => position.y = value; }
    public float PositionZ { get => position.z; set => position.z = value; }


    public Vector3 Direction { get => direction; set => direction = value; }
    public float DirectionX { get => direction.x; set => direction.x = value; }
    public float DirectionY { get => direction.y; set => direction.y = value; }
    public float DirectionZ { get => direction.z; set => direction.z = value; }

    public Vector3[] History { get => history; set => history = value; }
    public int HistoryMaxSize { get => historyMaxSize; set => historyMaxSize = value; }
    public int HistoryIndex { get => historyIndex; set => historyIndex = value; }
    public int HistorySize { get => historySize; set => historySize = value; }

    /* ------------- CONSTRUCTOR ------------- */
    public BoidModel(Vector3 startPosition,
        Vector3 startDirection,
        int historyMaxSize)
    {
        // Boid's state
        this.position = startPosition;
        this.direction = startDirection;
        
        // History initialization
        this.historyMaxSize = historyMaxSize;
        this.historyIndex = 0;
        this.historySize = 0;
        this.history = new Vector3[historyMaxSize];
    }

}