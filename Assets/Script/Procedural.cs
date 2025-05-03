using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural : MonoBehaviour
{
    public GameObject[] track; // Array de prefabs de secoes de pista
    public GameObject[] obstacles; // Array de prefabs de obstaculos
    public Transform[] lanes; // Lanes da pista onde os obstaculos podem ser gerados (ex: esquerda, meio, direita)
    public float zSpawn = 0f; // Posicao Z onde a proxima secao sera gerada
    public float track_lenght = 354f; // Comprimento de cada secao (offset em Z)
    public float numberOfChuncks = 2; // Quantidade de secoes a manter ativas
    public Transform player; // Transform do jogador, para checar avanco
    private List<GameObject> active_track = new List<GameObject>(); // Lista de secoes atualmente ativas

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Busca o objeto com tag "Player"

        for (int i = 0; i < numberOfChuncks; i++) // Loop para instanciar as secoes iniciais
        {
            if (i == 0)
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
        GameObject sTrack = Instantiate(track[trackIndex], new Vector3(track[trackIndex].transform.position.x, 0.7f, zSpawn), transform.rotation);
        active_track.Add(sTrack);
        zSpawn += track_lenght;

        // Gerar entre 1 e 2 obstáculos aleatórios
        int obstaclesToSpawn = Random.Range(1, 3);

        // Guardar lanes já usadas para não repetir
        List<int> usedLanes = new List<int>();

        for (int i = 0; i < obstaclesToSpawn; i++)
        {
            // Escolhe uma lane ainda não usada
            int laneIndex;
            do
            {
                laneIndex = Random.Range(0, lanes.Length);
            } while (usedLanes.Contains(laneIndex));
            usedLanes.Add(laneIndex);

            // Escolhe um obstáculo aleatório
            GameObject obstacle = obstacles[Random.Range(0, obstacles.Length)];

            // Define a posição do obstáculo no eixo Z aleatoriamente dentro do trecho
            float zOffset = Random.Range(20f, track_lenght - 20f);

            Vector3 spawnPos = new Vector3(lanes[laneIndex].position.x, lanes[laneIndex].position.y, zSpawn - track_lenght + zOffset);

            Instantiate(obstacle, spawnPos, Quaternion.identity);
        }
    }

    // Funcao que remove o trecho mais antigo
    void Delete()
    {
        Destroy(active_track[0]);
        active_track.RemoveAt(0);
    }
}
