using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LevelManager))]
public class WorldSwitcher : MonoBehaviour
{
    //Listas para armazenar os tilemaps e backgrounds
    [SerializeField] private List<GameObject> tileMaps;
    [SerializeField] private List<GameObject> backgroundMaps;
    [SerializeField] private LevelManager levelManager;
    //Variável para armazenar o mundo atual
    private int currentWorld = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentWorld = levelManager.currentLevel.startingDimension - 1;
        SwitchWorld(currentWorld);
    }

    //Função para alternar para a próxima dimensão (sentido 1)
    public void SwitchWorldUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!(currentWorld + 1 >= levelManager.currentLevel.dimensionsAvailable))
                currentWorld++;
            else
                currentWorld = 0;
            SwitchWorld(currentWorld);
        }
    }

    //Função para alternar para a próxima dimensão (sentido 2)
    public void SwitchWorldDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentWorld > 0)
                currentWorld--;
            else
                currentWorld = levelManager.currentLevel.dimensionsAvailable - 1;
            SwitchWorld(currentWorld);
        }
    }

    //Função para alternar entre os mundos
    private void SwitchWorld(int target = 0) 
    {
        for(int i = 0; i < tileMaps.Count; i++)
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
