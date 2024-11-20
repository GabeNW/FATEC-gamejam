using System.Collections;
using UnityEngine;


public class OffScreen : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Destroy(collision.gameObject);
			StartCoroutine(Wait());
		}
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(0.1f);
		GameManager.Instance.Restart();
	}
}
