using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEnder : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string escenaMenu = "MenuPrincipal"; // Opcional si quieres reiniciar
    public bool cerrarJuegoAlFinal = true;

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // Suscribirse al evento de "Fin del video"
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video terminado.");

        if (cerrarJuegoAlFinal)
        {
            Debug.Log("Cerrando aplicación...");
            Application.Quit();
            
            // Si estás en el editor de Unity, esto detiene el play mode
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        else
        {
            // O cargar menú
            SceneManager.LoadScene(escenaMenu);
        }
    }
}