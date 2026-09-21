using UnityEngine;

public class Camera_move : MonoBehaviour
{
   public float scrolllspeed;
   public Vector3 scrolldirection = Vector3.up;
   


    // Update is called once per frame
    void Update()
    {
        transform.position += scrolldirection.normalized * scrolllspeed * Time.deltaTime;
    }
}
