using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string level1SceneName = "LoreIntro";


    public string optionsSceneName = "options";

    public void StartGame()
    {
        Debug.Log("Iniciando partida...");
        SceneManager.LoadScene(level1SceneName);
    }


    public void OpenOptions()
    {
        if (SceneUtility.GetBuildIndexByScenePath(optionsSceneName) != -1)
        {
            Debug.Log("Abriendo Menú de Opciones...");
            SceneManager.LoadScene(optionsSceneName);
        }
        else
        {   
            Debug.Log("ERROR: Asigna una escena válida a 'optionsSceneName' o usa la lógica de Panel UI.");
        }
    }


    public void QuitGame()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}