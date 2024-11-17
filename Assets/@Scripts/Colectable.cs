using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colectable : MonoBehaviour
{
    [SerializeField] private int value;
    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            //adicione pontuação aqui
            Destroy(gameObject);
        }
    }
}
