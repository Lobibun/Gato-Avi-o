using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour
{
    private float lenght;
    private float StartP;

    private Transform cam;

    public float ParallaxEffect;

    void Start()
    {
        StartP = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main.transform;
    }

    void Update()
    {
     float ResPos = cam.transform.position.x * (1 - ParallaxEffect);
     float Distance = cam.transform.position.x * ParallaxEffect;  

     transform.position = new Vector3(StartP + Distance, transform.position.y, transform.position.z);
     
     if(ResPos > StartP + lenght)
     {
        StartP += lenght;
     }
     else if(ResPos < StartP - lenght)
     {
        StartP -= lenght;
     }
    }
}
