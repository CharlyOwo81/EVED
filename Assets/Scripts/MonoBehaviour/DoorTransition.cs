using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    [Header("Configuración de Transición")]
    [Tooltip("El nombre de la escena a la que quieres hacer la transición.")]
    public string targetSceneName = "Level_02"; 
    
    [Tooltip("La música de fondo para la escena de destino (Opcional).")]
    public AudioClip newSceneBGM;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            Debug.Log("Jugador detectado. Iniciando transición a escena: " + targetSceneName);
            
            SceneTransitionManager manager = FindObjectOfType<SceneTransitionManager>();
            
            if (manager != null)
            {
                manager.StartSceneTransition(targetSceneName, newSceneBGM);
            }
            else
            {

                Debug.LogError("SceneTransitionManager no encontrado en la escena. Cargando escena sin transición suave.");
                UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
            }
        }
    }
}