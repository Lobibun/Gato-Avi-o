using UnityEngine;

public class Helicoptero : RangedEnemy
{
    [Header("Velocidades")]
    public float velocidadeEntrada = 5f;
    public float velocidadeCamera = 4f;
    public float velocidadeVertical = 3f;

    [Header("Controle")]
    public bool chegouNoPosto = false;
    private bool indoParaCima = true;

    protected override void Start()
    {
        base.Start();
        SetDirection(Vector2.left);
    }

    void Update()
    {
        Mover();

        if (chegouNoPosto)
        {
            CheckAndFire();
        }
    }

    void Mover()
    {
        float moveX;
        float moveY = 0f;

        if (!chegouNoPosto)
        {
            moveX = -velocidadeEntrada;
        }
        else
        {
            moveX = velocidadeCamera;
            moveY = indoParaCima ? velocidadeVertical : -velocidadeVertical;
        }

        transform.Translate(new Vector2(moveX, moveY) * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PontoDeParada"))
        {
            chegouNoPosto = true;
            indoParaCima = true;
        }

        if (other.CompareTag("Teto"))
        {
            indoParaCima = false;
        }

        if (other.CompareTag("Chao"))
        {
            indoParaCima = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * visionRange);
    }
}
