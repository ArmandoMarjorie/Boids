using UnityEngine;

public class CageView : MonoBehaviour
{
    [SerializeField]
    private BoundsSettings boundsSettings;

    public void Init(BoundsSettings bs)
    {
        boundsSettings = bs;
    }

    private void OnDrawGizmos()
    {
        if (boundsSettings != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundsSettings.Center, new Vector3(boundsSettings.Width, boundsSettings.Height, boundsSettings.Depth));
        }
    }

}