
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField]private GameObject hitPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (hitPrefab != null)
        {

            Destroy(gameObject, 0.3f);
        }
    }

}
