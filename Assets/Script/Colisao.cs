using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Define um enum para representar a direção em que o personagem foi atingido no eixo Z (frente, trás, nenhuma, meio)
public enum HITZ { forward, backward, none, mid };

// Define um enum para representar a altura em que o personagem foi atingido (baixo, cima, nenhuma, meio)
public enum HITY { lower, upper, none, mid };

public class Colisao : MonoBehaviour
{
    public Transform head; // Referência para o ponto da cabeça do personagem (de onde o Raycast vai sair)
    public Transform feet; // Referência para os pés do personagem (não está sendo usado no código atual, mas pode ser útil futuramente)
    public float range; // Distância máxima do Raycast para detectar obstáculos à frente
    public bool isDead = false; // Variável que indica se o personagem está "morto"
    HITZ hitz = HITZ.none; // Variável que armazena a direção do impacto no eixo Z
    HITY hity = HITY.none; // Variável que armazena a altura do impacto
    public GameObject gameOver;
    public GameObject retryButton; // Botão UI que reinicia o jogo

    // Função chamada a cada frame do jogo
    void Update()
    {
        if(!isDead)
        {
            FrontCheck();
        }
    }

    // Essa função lança um Raycast a partir da cabeça do personagem na direção para frente
    public void FrontCheck()
    {
        RaycastHit hit; // Variável que armazenará informações sobre o que foi atingido

        // Desenha uma linha vermelha na cena para depuração (visível apenas na aba "Scene")
        Debug.DrawRay(head.position, transform.forward * range, Color.red);

        // Lança um Raycast da posição da cabeça, na direção para frente do objeto, com alcance definido por "range"
        if (Physics.Raycast(head.position, transform.forward, out hit, range))
        {
            // Verifica se o objeto atingido tem a tag "Train" ou "Hurdle"
            if (hit.collider.CompareTag("Train") || hit.collider.CompareTag("Hurdle"))
            {
                Debug.Log("Morreu"); // Escreve no console que o personagem morreu
                isDead = true; // Marca o personagem como morto
                hitz = HITZ.forward; // Registra que o impacto foi frontal
                hity = HITY.lower;   // Registra que o impacto foi na parte inferior (poderia ser ajustado com base na altura real do impacto)

                // Pausa o jogo
                Time.timeScale = 0f;

                gameOver.SetActive(true);
            }
        }
    }

    // Função pública chamada pelo botão na UI
    public void RestartGame()
    {
        Time.timeScale = 1f; // Volta ao tempo normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarrega a cena atual
    }
}