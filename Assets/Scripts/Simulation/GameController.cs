using UnityEngine;

public class GameController : MonoBehaviour
{
	private GPUBoid boidGPU = null;

	/* ------------- SCENE SETTINGS ------------- */
	[Header("Scene References")]

	[SerializeField]
	private BoidView boidView = null;

    [SerializeField]
    private CageView cageView = null;

    [SerializeField]
    private BoidSettings boidSettings = null;

    [SerializeField]
    private BoundsSettings boundsSettings = null;

	/* ------------- BOIDS SETTINGS ------------- */
	[Header("Boids Settings")]

	[SerializeField]
	private int boidHistoryMaxSize = 50;

	[SerializeField]
	private int nbBoids = 100;

	[SerializeField]
	private bool dessin_parcours = true;


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

        // View
        if (boidView != null)
			boidView.Init(boidArrayModel, nbBoids);
		else
		{
            Debug.LogError("BoidView is not assigned in the inspector.");
            Application.Quit();
        }

        // ScriptableObject
        if (boidSettings == null)
        {
            Debug.LogError("boidSettings is not assigned in the inspector.");
            Application.Quit();
        }

        // Controller
        boidGPU = new GPUBoid(cs, nb, boidView, boidSettings, boundsSettings);
	}

    public void Update()
	{
		boidGPU.Update(Time.deltaTime);
	}
}