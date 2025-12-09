using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : PersistentSingleton<SceneTransitionManager>
{
    [Header("Configuración de Transición")]
    [Tooltip("El script CanvasFader adjunto al Canvas de Transición.")]
    public CanvasFader canvasFader; 
    
    [Tooltip("Duración del fundido a negro (Fade Out) en segundos.")]
    public float fadeOutTime = 1.0f;
    
    [Tooltip("Duración del fundido de negro a transparente (Fade In) en segundos.")]
    public float fadeInTime = 1.0f;

    [Header("Configuración del Jugador")]
    [Tooltip("El Prefab del GameObject del jugador que se instanciará en la nueva escena.")]
    public GameObject playerPrefab;
    
    [Tooltip("El nombre del GameObject que actúa como punto de aparición en la nueva escena (p. ej., 'SpawnPoint').")]
    public string spawnPointName = "SpawnPoint";

    private GameObject currentPlayer;
    private bool wasRandomCycleActive = false;
    private AudioClip lastPlayingClip = null;
    public void StartSceneTransition(string sceneName, AudioClip newMusicClip = null)
    {
        currentPlayer = GameObject.FindWithTag("Player");
        if (MusicController.Instance != null)
        {
            MusicController music = MusicController.Instance.GetComponent<MusicController>();
            
            wasRandomCycleActive = music != null && music.defaultBGMList != null && 
                                   MusicController.Instance.GetComponent<AudioSource>().loop == false;
            
            if (!wasRandomCycleActive)
            {
                lastPlayingClip = MusicController.Instance.GetComponent<AudioSource>().clip;
            }
        }

        StartCoroutine(TransitionRoutine(sceneName, newMusicClip));
    }

    private IEnumerator TransitionRoutine(string sceneName, AudioClip newMusicClip)
    {
        yield return StartCoroutine(canvasFader.Fade(1f, fadeOutTime)); 

        if (MusicController.Instance != null)
        {
            yield return StartCoroutine(MusicController.Instance.FadeOut(fadeOutTime));
        }

        DestroyExistingPlayer();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SpawnPlayerAtSpawnPoint();

        if (MusicController.Instance != null)
        {
            MusicController music = MusicController.Instance.GetComponent<MusicController>();

            if (newMusicClip != null)
            {
                MusicController.Instance.GetComponent<AudioSource>().clip = newMusicClip;
                yield return StartCoroutine(MusicController.Instance.FadeIn(fadeInTime));
            }
            else if (wasRandomCycleActive && music != null)
            {
                music.StartRandomCycle(music.defaultBGMList, music.defaultCrossFadeTime);
            }
            else if (lastPlayingClip != null)
            {
                MusicController.Instance.GetComponent<AudioSource>().clip = lastPlayingClip;
                yield return StartCoroutine(MusicController.Instance.FadeIn(fadeInTime));
            }
        }

        yield return StartCoroutine(canvasFader.Fade(0f, fadeInTime)); 
    }

    private void DestroyExistingPlayer()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
            currentPlayer = null;
            Debug.Log("Jugador existente destruido antes de la transición (referencia local).");
            return;
        }

        GameObject playerToDestroy = GameObject.FindWithTag("Player");
        if (playerToDestroy != null)
        {
            Destroy(playerToDestroy);
            Debug.Log("Jugador existente (encontrado por Tag) destruido antes de la transición.");
        }
    }

    private void SpawnPlayerAtSpawnPoint()
    {
        Debug.Log(">>> INTENTANDO SPAWN DEL JUGADOR. Buscando SpawnPoint...");

        if (playerPrefab == null)
        {
            Debug.LogError("FATAL ERROR: Player Prefab no está asignado en SceneTransitionManager. ¡El jugador no aparecerá!");
            return;
        }

        GameObject spawnPointObject = GameObject.Find(spawnPointName);

        if (spawnPointObject != null)
        {
            Debug.Log($"SpawnPoint '{spawnPointName}' encontrado. Instanciando jugador...");
            currentPlayer = Instantiate(playerPrefab, spawnPointObject.transform.position, spawnPointObject.transform.rotation);
            currentPlayer.tag = "Player";
            Debug.Log("Jugador instanciado correctamente en el SpawnPoint.");
            
            ConnectCameraToPlayer(currentPlayer.transform);
        }
        else
        {
            Debug.LogError($"SpawnPoint con el nombre '{spawnPointName}' NO encontrado en la escena. ¡El jugador no aparecerá! Asegúrate de que exista y se llame exactamente 'SpawnPoint'.");
        }
    }
    private void ConnectCameraToPlayer(Transform newTarget)
    {
        TPCameraController cameraController = FindObjectOfType<TPCameraController>();

        if (cameraController != null)
        {
            cameraController.target = newTarget;
            Debug.Log("Cámara (TPCameraController) conectada al nuevo jugador.");
        }
        else
        {
            Debug.LogWarning("TPCameraController no encontrado. Asegúrate de que exista y sea persistente.");
        }
    }
}