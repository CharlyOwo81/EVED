using UnityEngine;

public class StopMusicTrigger : MonoBehaviour
{
    [Header("Tiempo de Detención")]
    [Tooltip("Tiempo en segundos que la música permanecerá detenida.")]
    public float stopDuration = 10f; 
    public AudioClip clipToResume;
    
    [Header("Configuración")]
    [Tooltip("Marcar si el trigger debe destruirse después de usarse.")]
    public bool destroyAfterUse = true;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasTriggered)
        {
            if (MusicController.Instance != null)
            {
                MusicController.Instance.PauseForDuration(stopDuration, clipToResume);
                hasTriggered = true; 
                
                if (destroyAfterUse)
                {
                    Destroy(gameObject); 
                }
            }
        }
    }
}