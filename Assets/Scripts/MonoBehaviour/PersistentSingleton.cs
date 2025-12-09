using UnityEngine;

public abstract class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

public static T Instance
{
    get
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
            
            if (instance == null)
            {
                Debug.LogError("FATAL ERROR: No hay ninguna instancia de " + typeof(T).ToString() + " en la escena. Debe existir una configurada en la escena inicial.");
            }
        }
        return instance;
    }
}

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            DontDestroyOnLoad(gameObject);
            
            Initialize(); 
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
                Debug.LogWarning("Se intentó crear una segunda instancia de " + typeof(T).ToString() + ". Destruyendo el duplicado.");
            }
        }
    }
    
    protected virtual void Initialize() { }
}