using UnityEngine;
using UnityEngine.UI;

public class DialogoData : MonoBehaviour
{
    [Header("Contenido del Diálogo")]
    [TextArea(1, 3)]
    public string[] nombresPersonaje; 
    [TextArea(3, 10)]
    public string[] mensajesDialogo; 
    
    [Header("Audio")]
    public AudioClip[] dialogoClips; 

    [Header("Imagen del Personaje")]
    public Sprite imagenPersonajeUI;
    
    private int indiceActual = 0;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("El objeto " + gameObject.name + " necesita un Collider para funcionar con este script.");
        }
    }

    public string GetMensajeActual()
    {
        if (indiceActual < mensajesDialogo.Length)
        {
            return mensajesDialogo[indiceActual];
        }
        return "FIN DEL DIÁLOGO";
    }

    public string GetNombreActual()
    {
        if (indiceActual < nombresPersonaje.Length)
        {
            return nombresPersonaje[indiceActual];
        }
        return "";
    }
    
    public void SiguienteDialogo()
    {
        indiceActual++;
    }

    public bool QuedanDialogos()
    {
        return indiceActual < mensajesDialogo.Length;
    }

    public void ResetDialogo()
    {
        indiceActual = 0;
    }

public AudioClip GetClipActual()
{
    if (indiceActual < dialogoClips.Length)
    {
        return dialogoClips[indiceActual];
    }
    return null;
}

}