using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void SwitchWorldUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!(currentWorld+1 >= tileMaps.Count))
                currentWorld++;
            else
                currentWorld = 0;
            SwitchWorld(currentWorld);
        }
    }

    public void SwitchWorldDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentWorld > 0)
                currentWorld--;
            else
                currentWorld = tileMaps.Count - 1;
            SwitchWorld(currentWorld);
        }
    }

    private void SwitchWorld(int target = 0) 
    {
        for(int i=0; i<tileMaps.Count; i++)
        {
            if(i == target)
            {
                tileMaps[i].SetActive(true);
                backgroundMaps[i].SetActive(true);
            }
            else
            {
                tileMaps[i].SetActive(false);
                backgroundMaps[i].SetActive(false);
            }
        }
    }
}
