using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movimento : MonoBehaviour
{
    private CharacterController controller; // Referencia ao componente CharacterController
    private Vector3 moveDirection;

    [Header("Velocidades")]
    [SerializeField] private float forwardSpeed = 15f; // Velocidade do movimento do jogador
    [SerializeField] private float sideSpeed = 5f; // Velocidade do movimento lateral
    [SerializeField] private float jumpPower = 5f; // Velocidade do pulo
    [SerializeField] private float gravity = 5f;

    [Header("Lanes")]
    private int targetLane = 1; // 0 = esquerda, 1 = meio, 2 = direita
    [SerializeField] private float laneDistance = 3f; // Distancia entre as lanes

    // Funcao chamada no início do jogo
    void Start()
    {
        // Obtem o componente CharacterController anexado ao objeto
        controller = GetComponent<CharacterController>();
    }

    // Funcao chamada a cada frame
    private void Update()
    {
        HandleForwardMovement();   // Move o jogador para frente constantemente
        HandleSwitchLane();        // Verifica entrada do jogador para mudar de lane
        MoveTowardsTargetLane();   // Move o jogador para a lane desejada
    }

    // Responsavel pelo movimento para frente
    private void HandleForwardMovement()
    {
        // Cria um vetor de movimento para frente com base na velocidade e no deltaTime
        Vector3 forwardMove = transform.forward * forwardSpeed * Time.deltaTime;

        // Converte o vetor local para o sistema de coordenadas global
        Vector3 worldForwardMove = transform.TransformDirection(forwardMove);

        // Move o personagem usando o CharacterController
        controller.Move(worldForwardMove);
    }

    // Verifica se as teclas de seta esquerda ou direita foram pressionadas
    private void HandleSwitchLane()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLane(-1); // Move para a lane a esquerda
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLane(1); // Move para a lane a direita
        }
    }

    // Atualiza a variavel targetLane de acordo com a direcao (esquerda = -1, direita = +1)
    private void MoveLane(int direction)
    {
        targetLane += direction; // Altera a lane
        targetLane = Mathf.Clamp(targetLane, 0, 2); // Mantem as lanes entre 0 e 2
    }

    // Calcula a posicao alvo de acordo com a lane desejada
    private Vector3 CalculateTargetPosition()
    {
        // Base da posição alvo e a posição atual, mantendo altura e profundidade
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        // Adiciona deslocamento lateral com base na lane
        if (targetLane == 0) // esquerda
        {
            targetPosition += Vector3.left * laneDistance; 
        }
        else if (targetLane == 2) // direita
        {
            targetPosition += Vector3.right * laneDistance;
        }
        return targetPosition;
    }

    // Move o jogador suavemente em direçao a posicao alvo
    private void MoveTowardsTargetLane()
    {
        // Posicao que o jogador deve alcancar
        Vector3 targetPosition = CalculateTargetPosition();

        if (transform.position != targetPosition)
        {
            // Diferença entre a posição atual e a desejada
            Vector3 diff = targetPosition - transform.position;

            // Direção de movimento com velocidade aplicada
            Vector3 moveDir = diff.normalized * sideSpeed * Time.deltaTime;

            // Verifica se o movimento não vai ultrapassar a posição alvo
            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir); // Move uma parte do caminho
            else
                controller.Move(diff); // Move diretamente até o alvo
        }
    }
}
