using UnityEngine;

public class GameController : MonoBehaviour
{
    private BoidArrayModel boidArrayModel = null;
    private GPUBoidController GPU_boidController = null;

	/* ------------- SCENE SETTINGS ------------- */
	[Header("Scene References")]

    [SerializeField]
	private GPUBoidView boidView = null;

    [SerializeField]
    private CageView cageView = null;

    [SerializeField]
    private BoidSettings boidSettings = null;

    [SerializeField]
    private BoundsSettings boundsSettings = null;

    [SerializeField]
    private int nbBoids = 100;

    [SerializeField]
    private ComputeShader computeShader = null;

    public void Start()
	{
        /* ------------- SCENE INITIALIZATION ------------- */

        // ScriptableObject
        if (boundsSettings == null)
        {
            Debug.LogError("boundsSettings is not assigned in the inspector.");
            Application.Quit();
        }

        // View (cage)
        if(cageView != null)
            cageView.Init(boundsSettings);
        else
        {
            Debug.LogError("CageView is not assigned in the inspector.");
            Application.Quit();
        }

        /* ------------- BOIDS INITIALIZATION ------------- */

        // Model
        boidArrayModel = new BoidArrayModel(nbBoids,
            boundsSettings.Width,
            boundsSettings.Height,
            boundsSettings.Depth,
            boundsSettings.Center);

        // ScriptableObject
        if (boidSettings == null)
        {
            Debug.LogError("boidSettings is not assigned in the inspector.");
            Application.Quit();
        }

        // Compute Shader
        if (computeShader == null)
        {
            Debug.LogError("computeShader is not assigned in the inspector.");
            Application.Quit();
        }

        // Controller
        GPU_boidController = new GPUBoidController(boidArrayModel, computeShader, boidView, boidSettings, boundsSettings);
        GPU_boidController.InitGPUData();
    }

    public void Update()
	{
        GPU_boidController.Update(Time.deltaTime);
	}

    public void OnDestroy()
    {
        GPU_boidController.OnDestroy();
    }
}