using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoreSceneController : MonoBehaviour
{
    [Header("Configuración de Carga")]
    [Tooltip("Tiempo en segundos que durará la escena de introducción.")]
    public float sceneDuration = 60f;

    [Tooltip("El nombre exacto de la escena de Gameplay del Nivel 1.")]
    public string nextSceneName = "Level1";


    void Start()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("ERROR: 'nextSceneName' no está configurado en el Inspector.");
            return;
        }
        StartCoroutine(LoadNextSceneAfterDelay(sceneDuration));
    }

    IEnumerator LoadNextSceneAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);

        if (SceneUtility.GetBuildIndexByScenePath(nextSceneName) != -1)
        {
            Debug.Log("Introducción terminada. Cargando la escena de Gameplay: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
             Debug.LogError("ERROR: La escena de destino (" + nextSceneName + ") no está en Build Settings. ¡Añádela!");
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            LoadNextSceneAfterDelay(0f);
        }
    }
}