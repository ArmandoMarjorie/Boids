using UnityEngine;

public class GameController : MonoBehaviour
{
	private BoidArrayModel boidArrayModel = null;
	private BoidController boidController = null;

	/* ------------- SCENE SETTINGS ------------- */
	[Header("Scene References")]

	[SerializeField]
	private BoidView boidView = null;

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

        /* ------------- BOIDS INITIALIZATION ------------- */

        // Model
        boidArrayModel = new BoidArrayModel(nbBoids,
            boundsSettings.Width,
            boundsSettings.Height,
            boundsSettings.Depth,
            boidHistoryMaxSize);

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
        boidController = new BoidController(boidArrayModel, boidView, boidSettings, boundsSettings);
	}

    public void Update()
	{
		boidController.Update(Time.deltaTime);
	}
}