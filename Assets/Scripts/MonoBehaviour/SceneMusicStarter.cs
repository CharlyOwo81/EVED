using UnityEngine;

public class SceneMusicStarter : MonoBehaviour
{
    [Header("Música Específica de esta Escena")]
    [Tooltip("Asigna el clip que debe sonar en esta escena. Si es NULL, la música se detiene.")]
    public AudioClip sceneBGM;
    
    [Header("Control de Audio")]
    [Tooltip("Tiempo en segundos para el Fade In de la nueva canción.")]
    public float fadeInTime = 1.0f; 

    void Start()
    {
        if (MusicController.Instance != null)
        {
            MusicController.Instance.StopAndPlay(sceneBGM);

            if (sceneBGM != null && fadeInTime > 0)
            {
                StartCoroutine(MusicController.Instance.FadeIn(fadeInTime));
            }
        }
    }
}