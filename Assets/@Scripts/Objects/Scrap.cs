using UnityEngine;

public class Scrap : MonoBehaviour
{
    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            //adicione pontuação aqui
            Destroy(gameObject);
            GameManager.Instance.AddCollected();
        }
    }

}
