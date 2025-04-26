using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player; // arraste o Transform do personagem aqui
    public Vector3 offset = new Vector3(0, 5, -10);

    void LateUpdate()
    {
        // Move a câmera para a posição do jogador acrescida do offset
        transform.position = player.position + offset;
    }
}
