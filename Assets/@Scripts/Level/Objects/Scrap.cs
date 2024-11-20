using UnityEngine;
using UnityEngine.Tilemaps;

public class Scrap : MonoBehaviour
{
	private Tilemap tilemap;
	private bool collected = false;
	
	void Start() 
	{
		tilemap = GetComponent<Tilemap>();
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collected) return;
		
		if (collision.CompareTag("Player"))
		{
			collected = true;
#if UNITY_EDITOR
			Debug.Log("Scrap collected!!!");
#endif
			Destroy(gameObject);
			GameManager.Instance.AddCollected();
		}
	}
}
