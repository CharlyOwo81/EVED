using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Transiciones : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("Nombre EXACTO de la escena a la que se debe transicionar (ej: 'SalonClases')")]
    public string sceneName;
    
    [Header("Control de Audio (Opcional)")]
    [Tooltip("Tiempo en segundos para el Fade Out antes de cambiar de escena. 0 para corte inmediato.")]
    public float fadeOutTime = 1.0f; 

    private bool isTransitioning = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;
            
            StartCoroutine(DoTransition());
        }
    }

    IEnumerator DoTransition()
    {
        if (MusicController.Instance != null && fadeOutTime > 0)
        {
            yield return StartCoroutine(MusicController.Instance.FadeOut(fadeOutTime));
        }
        SceneManager.LoadScene(sceneName);
    }
}