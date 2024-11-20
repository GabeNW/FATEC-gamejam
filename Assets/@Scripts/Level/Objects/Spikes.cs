using UnityEngine;

public class Spikes : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			//Debug.Log("Inescapable death!!!");
			collision.GetComponent<PlayerMovement>().canMove = false;
			GameManager.Instance.Restart();
		}
	}
}
