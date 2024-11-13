using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UnityTestAction : MonoBehaviour
{
    public UnityAction firstAction;

    public bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        firstAction += Test;
        Debug.Log("Test: " + test);
    }

    public void InputTest(InputAction.CallbackContext context)
    {
        firstAction.Invoke();
    }

    private void Test()
    {
        Debug.Log("Test: " + test);
        test = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
