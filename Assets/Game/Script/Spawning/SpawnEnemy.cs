using UnityEngine;
using System.Collections.Generic;

public class AreaSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyConfig
    {
        public string nome;             
        public GameObject prefab;       
        
        [Header("Tempo de Aparição")]
        public float tempoParaComecar;  
        public float tempoParaParar;    

        [Header("Controle de Sorteio")]
        [Range(1, 100)]
        public float pesoDeSpawn;       
        
        [Header("Vagas Dinâmicas (Limite Simultâneo)")]
        [Tooltip("Com quantos inimigos desse tipo a fase começa (Limite Inicial)")]
        public int limiteInicial = 2;

        [Tooltip("Quantos slots novos adiciona a cada aumento?")]
        public int aumentoDeVagas = 1;

        [Tooltip("A cada quantos segundos DEPOIS DE COMEÇAR A SPAWNAR o limite aumenta?")]
        public float tempoParaAumentarVagas = 60f; 
    }

        [System.Serializable]
        public class BoxConfig
    {
        public GameObject prefab;
        [Range(1,100)] public float peso = 10;
    }

    [Header("Spawn de Caixas")]
    public List<BoxConfig> boxes;
    public float boxSpawnRate = 20f;   
    public float boxSpawnChance = 0.4f; 

    private float nextBoxSpawnTime;

    [Header("Lista de Inimigos")]
    public List<EnemyConfig> listaDeInimigos;

    private List<GameObject> inimigosVivos = new List<GameObject>();

    [Header("Área de Spawn")]
    public Vector2 areaSize = new Vector2(5f, 10f);

    [Header("Configuração Geral")]
    public float spawnRate = 2f; 
    public int globalMaxEnemies = 100; 

    private float nextSpawnTime;
    private float startTime;
    private bool stageEnded = false;
    
    [Header("Duração da fase")]
    public float stageDuration = 1800f;     

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (stageEnded) return;

        float elapsedTime = Time.time - startTime;
        if (elapsedTime >= stageDuration) { stageEnded = true; return; }

        inimigosVivos.RemoveAll(item => item == null);

        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;
            
            if (inimigosVivos.Count < globalMaxEnemies)
            {
                SpawnEnemies(1, elapsedTime);
            }
        }

        if (Time.time >= nextBoxSpawnTime)
        {
            nextBoxSpawnTime = Time.time + boxSpawnRate;

        if (Random.value <= boxSpawnChance && boxes.Count > 0)
        {
            SpawnBox();
        }
        }
    }

    void SpawnEnemies(int amount, float currentTime)
    {
        for (int i = 0; i < amount; i++)
        {
            List<EnemyConfig> candidatosValidos = new List<EnemyConfig>();
            float pesoTotal = 0f;

            foreach (EnemyConfig enemy in listaDeInimigos)
            {
                // 1. Checa tempo
                bool tempoOk = currentTime >= enemy.tempoParaComecar && 
                              ((enemy.tempoParaParar <= 0) || (currentTime < enemy.tempoParaParar));
                
                // 2. CALCULA O LIMITE (CORRIGIDO)
                int limiteAtual = enemy.limiteInicial;

                if (enemy.tempoParaAumentarVagas > 0 && tempoOk)
                {
                    // AQUI ESTÁ A MUDANÇA: Subtraímos o tempo de início
                    // O tempo só conta a partir de quando o inimigo foi liberado
                    float tempoAtivoDesteInimigo = currentTime - enemy.tempoParaComecar;
                    
                    int aumentos = (int)(tempoAtivoDesteInimigo / enemy.tempoParaAumentarVagas);
                    limiteAtual += (aumentos * enemy.aumentoDeVagas);
                }

                // 3. Conta vivos
                int quantidadeViva = 0;
                foreach(GameObject vivo in inimigosVivos)
                {
                    if (vivo.name.Contains(enemy.prefab.name))
                        quantidadeViva++;
                }

                // 4. Verifica vaga
                bool temVaga = quantidadeViva < limiteAtual;

                if (tempoOk && temVaga)
                {
                    candidatosValidos.Add(enemy);
                    pesoTotal += enemy.pesoDeSpawn;
                }
            }

            if (candidatosValidos.Count == 0) return;

            Vector3 spawnPos = transform.position + new Vector3(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                Random.Range(-areaSize.y / 2f, areaSize.y / 2f), 0f);
            spawnPos.z = 0;

            float valorSorteado = Random.Range(0, pesoTotal);
            float somaAtual = 0;
            EnemyConfig escolhido = null;

            foreach (EnemyConfig candidato in candidatosValidos)
            {
                somaAtual += candidato.pesoDeSpawn;
                if (valorSorteado <= somaAtual)
                {
                    escolhido = candidato;
                    break;
                }
            }

            if (escolhido != null)
            {
                GameObject novo = Instantiate(escolhido.prefab, spawnPos, Quaternion.identity);
                inimigosVivos.Add(novo);
            }
        }
    }

    void SpawnBox()
    {
        float pesoTotal = 0f;
        foreach (var box in boxes)
        pesoTotal += box.peso;

        float sorteio = Random.Range(0, pesoTotal);
        float soma = 0f;

        BoxConfig escolhida = null;
        foreach (var box in boxes)
        {
            soma += box.peso;
        if (sorteio <= soma)
        {
            escolhida = box;
            break;
        }
    }

    if (escolhida == null) return;

         Vector3 spawnPos = transform.position + new Vector3(
        Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
        Random.Range(-areaSize.y / 2f, areaSize.y / 2f),
        0f);

         GameObject novaCaixa = Instantiate(escolhida.prefab, spawnPos, Quaternion.identity);
         Vector3 pos = novaCaixa.transform.position;
        pos.z = 0f;
        novaCaixa.transform.position = pos;
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, areaSize.y, 1));
    }
}