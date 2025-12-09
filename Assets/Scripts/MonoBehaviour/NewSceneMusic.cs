using UnityEngine;

public class SceneMusicConfig : MonoBehaviour
{
    [Header("Configuración de Música de Escena")]
    [Tooltip("Marcar si quieres que las canciones se repitan en ciclo aleatorio.")]
    public bool useRandomCycle = false;
    
    [Tooltip("Tiempo de transición entre canciones (CrossFade). Ignorado si solo hay 1 canción.")]
    public float crossFadeTime = 3.0f; 

    [Header("Clips de Audio")]
    [Tooltip("Asigna aquí las canciones. Si solo pones una, se reproduce en bucle normal.")]
    public AudioClip[] sceneBGMs;
    
    [Tooltip("Tiempo de Fade In al cargar la escena.")]
    public float initialFadeInTime = 1.0f; 

    void Start()
    {
        if (MusicController.Instance == null)
        {
            Debug.LogError("MusicController no encontrado. Asegúrate de que existe en la escena inicial.");
            return;
        }

        if (sceneBGMs == null || sceneBGMs.Length == 0)
        {
            MusicController.Instance.StopAndPlay(null);
        }
        else if (sceneBGMs.Length == 1 || !useRandomCycle)
        {
            AudioClip singleClip = sceneBGMs[0];
            MusicController.Instance.StopAndPlay(singleClip);
            
            AudioSource source = MusicController.Instance.GetComponent<AudioSource>();
            if(source != null) source.loop = true; 

            if (initialFadeInTime > 0)
            {
                StartCoroutine(MusicController.Instance.FadeIn(initialFadeInTime));
            }
        }
        else
        {
            AudioSource source = MusicController.Instance.GetComponent<AudioSource>();
            if(source != null) source.loop = false; 

            MusicController.Instance.StartRandomCycle(sceneBGMs, crossFadeTime);
        }
    }
}