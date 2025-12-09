// CameraPivotLink.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraPivotLink : MonoBehaviour
{
    private Camera sceneCamera; 

    void Awake(){
        
        sceneCamera = GetComponentInChildren<Camera>(true); 
        DontDestroyOnLoad(gameObject); 
        SceneManager.sceneLoaded += OnSceneLoaded;


        if(sceneCamera != null){
            sceneCamera.gameObject.SetActive(false); 
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        StopAllCoroutines();
        StartCoroutine(FindAttachAndEnableCamera());
    }

    private System.Collections.IEnumerator FindAttachAndEnableCamera(){
        
        GameObject player = null;
        int attempts = 0;
        yield return null;

        while (player == null && attempts < 20) 
        {
            player = GameObject.FindGameObjectWithTag("Player"); 
            if (player == null)
            {
                yield return null;
                attempts++;
            }
        }

        if (player != null){

            transform.SetParent(player.transform, true); 
            transform.localPosition = Vector3.zero;
            
            if (sceneCamera != null){
                sceneCamera.gameObject.SetActive(true);
            }

            DisableSceneCameras(sceneCamera); 

            Debug.Log("CameraPivot enganchado y cámara activada en el nuevo personaje: " + player.name);
        }
        else{
            if (sceneCamera != null) sceneCamera.gameObject.SetActive(false);
            Debug.LogError("Error: No se pudo encontrar el jugador instanciado (Tag 'Player'). La cámara persistente fue desactivada.");
        }
    }
    

    private void DisableSceneCameras(Camera persistentCamera)
    {
        Camera[] sceneCameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in sceneCameras)
        {
            if (cam != persistentCamera)
            {
                cam.gameObject.SetActive(false);
                if(cam.CompareTag("MainCamera"))
                {
                    cam.tag = "Untagged";
                }
            }
        }
    }

    void OnDestroy()
    {
        // Limpieza
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}