using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueManager : PersistentSingleton<DialogueManager> 
{
    [Header("Referencias de UI (ASIGNAR EN INSPECTOR)")]
    public GameObject dialogoPanel; 
    public Text textoDialogo; 
    public Text nombrePersonaje; 
    public Image imagenPersonaje;
    public AudioSource audioSource; 
    [Header("Configuración")]
    public float tiempoEntreCaracteres = 0.05f;
    
    private DialogoData dialogoActual; 
    private Coroutine typingRoutine;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BuscarReferenciasUI();
    }

    public void StartDialogue(DialogoData data)
    {
        if (dialogoPanel == null || textoDialogo == null || audioSource == null)
        {
            Debug.LogError("DialogueManager: Faltan referencias de UI. ¿Se asignaron en el Canvas persistente?");
            return;
        }

        dialogoActual = data;
        dialogoActual.ResetDialogo();
        
        dialogoPanel.SetActive(true);
        imagenPersonaje.sprite = dialogoActual.imagenPersonajeUI; 
        
        MostrarSiguienteLinea();
    }

    public void MostrarSiguienteLinea()
    {
        if (dialogoActual == null) return;
        
        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            typingRoutine = null;
            textoDialogo.text = dialogoActual.GetMensajeActual();
            dialogoActual.SiguienteDialogo();
            return;
        }

        if (dialogoActual.QuedanDialogos())
        {
            audioSource.Stop(); 
            
            string nombre = dialogoActual.GetNombreActual();
            string mensaje = dialogoActual.GetMensajeActual();
            AudioClip clip = dialogoActual.GetClipActual();

            nombrePersonaje.text = nombre;

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
            
            typingRoutine = StartCoroutine(TypeSentence(mensaje));
            
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogoPanel.SetActive(false);
        audioSource.Stop();
        dialogoActual = null;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        textoDialogo.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textoDialogo.text += letter;
            yield return new WaitForSeconds(tiempoEntreCaracteres);
        }
        dialogoActual.SiguienteDialogo();
        typingRoutine = null;
    }
    public void BuscarReferenciasUI()
    {
        GameObject canvasPersistente = GameObject.Find("Canvas_HUD");
        if (canvasPersistente == null)
        {
            Debug.LogWarning("DialogueManager: No se encontró el Canvas persistente para reconectar la UI.");
            return;
        }

        Transform panelTransform = canvasPersistente.transform.Find("PanelDialogo");
        
        if (panelTransform != null)
        {
            dialogoPanel = panelTransform.gameObject;
            
            textoDialogo = dialogoPanel.transform.Find("txtDialogo").GetComponent<Text>();
            nombrePersonaje = dialogoPanel.transform.Find("txtNombre").GetComponent<Text>();
            imagenPersonaje = dialogoPanel.transform.Find("imgPersonaje").GetComponent<Image>();
            audioSource = GetComponent<AudioSource>();
            Debug.Log("Referencias de Diálogo reconectadas al Canvas persistente.");

            if (textoDialogo != null && nombrePersonaje != null && imagenPersonaje != null)
                    {
                        Debug.Log("Referencias de Diálogo reconectadas al Canvas persistente. ¡AHORA SÍ!");
                        if (dialogoPanel.activeSelf)
                        {
                            dialogoPanel.SetActive(false); 
                        }
                    }
                    else
                    {
                        Debug.LogError("DialogueManager: Error en la reconexión. No se encontró 'txtDialogo', 'txtNombre' o 'imgPersonaje' dentro de PanelDialogo.");
                    }

        }
        else
        {
            Debug.LogError("DialogueManager: No se encontró el 'Panel_Dialogo' dentro del Canvas.");
        }
    }
}