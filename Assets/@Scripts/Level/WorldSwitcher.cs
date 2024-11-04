using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitcher : MonoBehaviour
{
    public List<GameObject> tileMaps;
    public List<GameObject> backgroundMaps;
    public int currentWorld = 0;
    // Start is called before the first frame update
    void Start()
    {
        SwitchWorld(currentWorld);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentWorld++;
            if (currentWorld >= tileMaps.Count)
            {
                currentWorld = 0;
            }
            SwitchWorld(currentWorld);
        }   
    }

    void SwitchWorld(int target = 0) {
        for(int i=0; i<tileMaps.Count; i++)
        {
            if(i == target)
            {
                tileMaps[i].SetActive(true);
                backgroundMaps[i].SetActive(true);
            } else
            {
                tileMaps[i].SetActive(false);
                backgroundMaps[i].SetActive(false);
            }
        }
    }
}
