using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class SubtitleController : MonoBehaviour
{
    [Header("Referencias UI y Audio")]
    [Tooltip("El componente TextMeshPro para mostrar los subtítulos.")]
    public TextMeshProUGUI subtitleText;

    [Tooltip("El componente AudioSource con el audio del Lore en coreano.")]
    public AudioSource narrativeAudioSource;

    [Header("Configuración de Subtítulos (Tiempo vs Texto)")]
    [Tooltip("Define los puntos en el tiempo donde debe cambiar el texto del subtítulo.")]
    public SubtitleLine[] subtitles;

    [System.Serializable]
    public struct SubtitleLine
    {
        [Tooltip("El tiempo (en segundos) en el que debe aparecer esta línea desde el inicio del audio.")]
        public float startTime;
        public string text;
        [Tooltip("Si es True, el texto permanecerá en pantalla hasta el siguiente subtítulo o hasta el final.")]
        public bool clearAfterDisplay;
    }

    [Header("Carga de Escena (Al terminar)")]
    public string nextSceneName = "Level1";


    void Start()
    {
        if (subtitleText == null || narrativeAudioSource == null)
        {
            Debug.LogError("Referencias de audio/texto faltantes. Asigna Subtitle Text y Audio Source en el Inspector.");
            return;
        }

        subtitleText.text = "";

        if (narrativeAudioSource.clip != null)
        {
            narrativeAudioSource.Play();
        }
        else
        {
             Debug.LogError("El AudioSource no tiene un clip de audio asignado.");
             return;
        }

        StartCoroutine(RunSubtitlesAndLoadScene());
    }

    IEnumerator RunSubtitlesAndLoadScene()
    {

        for (int i = 0; i < subtitles.Length; i++)
        {
            StartCoroutine(ShowSubtitle(subtitles[i]));
        }

        yield return new WaitForSeconds(narrativeAudioSource.clip.length);
        
        yield return new WaitForSeconds(1f);

        subtitleText.text = "";

        int buildIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + nextSceneName + ".unity"); 
        if (buildIndex != -1)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("ERROR: La escena de destino '" + nextSceneName + "' NO está en Build Settings. Añádela a 'File -> Build Settings...'");
        }
    }
    
    IEnumerator ShowSubtitle(SubtitleLine line)
    {
        yield return new WaitForSeconds(line.startTime);

        if (subtitleText != null)
        {
            subtitleText.text = line.text;
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            StopAllCoroutines();
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                 SceneManager.LoadScene(nextSceneName);
            }
            
        }
    }
}