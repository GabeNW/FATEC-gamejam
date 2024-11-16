using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _uniqueInstance = null;
    private static bool IsApplicationQuit = false;

    public static T Instance
    {
        get
        {
            if (_uniqueInstance == null && IsApplicationQuit == false)
            {
                _uniqueInstance = FindObjectOfType<T>();

                if (_uniqueInstance == null)
                {
                    GameObject InstancePrefab = Resources.Load<GameObject>(typeof(T).Name);
                    if (InstancePrefab != null)
                    {
                        GameObject InstanceObject = Instantiate<GameObject>(InstancePrefab);
                        if (InstanceObject != null)
                            _uniqueInstance = InstanceObject.GetComponent<T>();
                    }
                    if (_uniqueInstance == null)
                        _uniqueInstance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return _uniqueInstance;
        }
        private set
        {
            if (_uniqueInstance == null)
            {
                _uniqueInstance = value;
                DontDestroyOnLoad(_uniqueInstance.gameObject);
            }
            else if (_uniqueInstance != value)
            {
#if UNITY_EDITOR
                Debug.LogError("[" + typeof(T).Name + "] Tentou instanciar uma segunda " +
                    "classe IPersistentSingleton!", value.gameObject);
#endif
                DestroyImmediate(value.gameObject);
            }
        }
    }

    //Awake é chamado quando a instância do script for carregada
    protected virtual void Awake()
    {
        Instance = this as T;
    }

    //Essa função é chamada quando o MonoBehaviour for destruído
    private void OnDestroy()
    {
        if (_uniqueInstance == this)
        {
            _uniqueInstance = null;
        }
    }

    //Enviado a todos os objetos de jogo antes de sair do aplicativo
    private void OnApplicationQuit()
    {
        IsApplicationQuit = true;
    }
}
