using UnityEngine;

public class ExplosionRocket : MonoBehaviour
{
    public float delay = 0.5f;

    void Start()
    {
        Destroy(gameObject, delay);
    }
}
