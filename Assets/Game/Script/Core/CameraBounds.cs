using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBounds : MonoBehaviour
{
    public static CameraBounds instance;

    private Camera cam;
    private float halfWidth;
    private float halfHeight;

    public float MinX { get; private set; }
    public float MaxX { get; private set; }
    public float MinY { get; private set; }
    public float MaxY { get; private set; }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        cam = GetComponent<Camera>();

        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        MinX = pos.x - halfWidth;
        MaxX = pos.x + halfWidth;
        MinY = pos.y - halfHeight;
        MaxY = pos.y + halfHeight;
    }
}