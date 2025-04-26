using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural : MonoBehaviour
{
    public GameObject[] track; // Array de prefabs de secoes de pista
    public float zSpawn = 0f; // Posicao Z onde a proxima secao sera gerada
    public float track_lenght = 354f; // Comprimento de cada secao (offset em Z)
    public float numberOfChuncks = 2; // Quantidade de secoes a manter ativas
    public Transform player; // Transform do jogador, para checar avanco
    private List<GameObject> active_track = new List<GameObject>(); // Lista de secoes atualmente ativas

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Busca o objeto com tag "Player"

        for (int i = 0; i<numberOfChuncks; i++) // Loop para instanciar as secoes iniciais
        {
            if(i == 0)
            {
                TrackGenerator(0); // O primeiro pedaco e sempre o index 0
            }
            else
            {
                TrackGenerator(Random.Range(0, track.Length)); // Demais pedacos sao sorteados
            }
        }
    }

    private void Update()
    {
        // Se o jogador estiver a menos de um trecho de distancia de zSpawn
        if (player.position.z + track_lenght > zSpawn)
        {
            TrackGenerator(Random.Range(0, track.Length)); // Gera nova secao aleatoria
            Delete(); // Remove a secao mais antiga
        }
    }

    // Funcao que instancia uma secao de pista
    void TrackGenerator(int trackIndex)
    {
        // Clona o prefab na posicao X original, Y=0.7, Z=zSpawn, com mesma rotacao do pai
        GameObject sTrack = Instantiate(track[trackIndex], new Vector3(track[trackIndex].transform.position.x, 0.7f, zSpawn), transform.rotation);
        active_track.Add(sTrack); // Adiciona o clone a lista de pistas ativas
        zSpawn += track_lenght; // Avanca zSpawn para a proxima instancia
    }

    // Funcao que remove o trecho mais antigo
    void Delete()
    {
        Destroy(active_track[0]); // Destroi o GameObject mais antigo
        active_track.RemoveAt(0);  // Remove a referencia da lista
    }
}
