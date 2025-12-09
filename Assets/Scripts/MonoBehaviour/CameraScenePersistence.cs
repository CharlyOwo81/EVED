using UnityEngine;

public class CameraScenePersistence : MonoBehaviour
{
    private static CameraScenePersistence instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}