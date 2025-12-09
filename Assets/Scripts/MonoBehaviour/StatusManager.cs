using UnityEngine;
using UnityEngine.UI; 

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance; 

    // --- BARRAS NARRATIVAS ---
    [Header("1. Valores de Estado")]
    [Range(0, 100)] public float Felicidad = 100f;
    [Range(0, 100)] public float Duda = 0f;
    [Range(0, 100)] public float ConfianzaPuntuacion = 30f;
    public float MaxValue = 100f;
    
    [Header("2. Interfaz Felicidad")]
    public Image FelicidadFillImage; 
    public Text FelicidadValueText; 
    
    [Header("3. Interfaz Duda")]
    public GameObject DudaPanel; 
    public Image DudaFillImage;
    public Text DudaValueText;

    [Header("4. Interfaz Confianza")]
    public Text ConfianzaStateText; 
    
    [Header("5. Configuración General")]
    public float tiempoNombre = 3f; 
    public float umbralAparicionDuda = 50f;

    [Header("6. Sistema de Diálogo (Canvas HUD)")]
    public GameObject panelDialogo;      
    public Text txtNombreNPC;            
    public Text txtMensajeNPC;           
    public Image imgPersonajeUI;         
    public AudioSource audioSourceDialogo; 

    private DialogoData npcActualData;   
    private bool enDialogo = false;

    public enum EstadoConfianza { Confiable, Dudable, Desconfiable }
    private readonly Color colorConfiable = new Color(0.01f, 0.66f, 0.95f);
    private readonly Color colorDudable = new Color(1f, 0.75f, 0.02f);
    private readonly Color colorDesconfiable = new Color(0.84f, 0.26f, 0.08f);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            
            if (DudaPanel != null) DudaPanel.SetActive(false);
            if (panelDialogo != null) panelDialogo.SetActive(false);
            
            if (audioSourceDialogo == null) 
                audioSourceDialogo = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

void Update()
    {
        Felicidad = Mathf.Clamp(Felicidad, 0, MaxValue);
        Duda = Mathf.Clamp(Duda, 0, MaxValue);
        ConfianzaPuntuacion = Mathf.Clamp(ConfianzaPuntuacion, 0, MaxValue);

        if (Felicidad <= umbralAparicionDuda)
        {
            if (DudaPanel != null && !DudaPanel.activeSelf) DudaPanel.SetActive(true);
        }

        if (FelicidadFillImage != null) 
            FelicidadFillImage.fillAmount = Felicidad / MaxValue;
        
        if (DudaPanel != null && DudaPanel.activeSelf && DudaFillImage != null) 
            DudaFillImage.fillAmount = Duda / MaxValue;

        if (FelicidadValueText != null) 
        {
            if (!IsInvoking("RevertirFelicidadTexto"))
                FelicidadValueText.text = Felicidad.ToString("0") + "%";
        }
        
        if (DudaValueText != null && DudaPanel.activeSelf) 
        {
            if (!IsInvoking("RevertirDudaTexto"))
                DudaValueText.text = Duda.ToString("0") + "%";
        }

        ActualizarEstadoConfianza();
    }

    public void IniciarOAvanzarDialogo(DialogoData data)
    {
        if (!enDialogo || npcActualData != data)
        {
            npcActualData = data;
            npcActualData.ResetDialogo();
            enDialogo = true;
            panelDialogo.SetActive(true);
        }

        if (npcActualData.QuedanDialogos())
        {
            if (txtNombreNPC) txtNombreNPC.text = npcActualData.GetNombreActual();
            if (txtMensajeNPC) txtMensajeNPC.text = npcActualData.GetMensajeActual();
            if (imgPersonajeUI && npcActualData.imagenPersonajeUI) imgPersonajeUI.sprite = npcActualData.imagenPersonajeUI;

            AudioClip clip = npcActualData.GetClipActual();
            if (clip != null && audioSourceDialogo != null)
            {
                audioSourceDialogo.Stop(); 
                audioSourceDialogo.PlayOneShot(clip);
            }

            npcActualData.SiguienteDialogo();
        }
        else
        {
            CerrarDialogo();
        }
    }

    public void CerrarDialogo()
    {
        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (audioSourceDialogo != null) audioSourceDialogo.Stop();
        
        enDialogo = false;
        if(npcActualData != null) npcActualData.ResetDialogo();
        npcActualData = null;
    }
    
    public bool EstaEnDialogo() { return enDialogo; }
    public void CambiarFelicidad(float cambio) { Felicidad = Mathf.Clamp(Felicidad + cambio, 0, MaxValue); }
    public void CambiarDuda(float cambio) { if (DudaPanel.activeSelf) Duda = Mathf.Clamp(Duda + cambio, 0, MaxValue); }
    public void CambiarConfianza(float cambio) { ConfianzaPuntuacion = Mathf.Clamp(ConfianzaPuntuacion + cambio, 0, MaxValue); }
    
    public void MostrarNombreTemporalmente(string nombre, Text texto)
    {
        CancelInvoke("RevertirFelicidadTexto");
        CancelInvoke("RevertirDudaTexto");
        texto.text = nombre.ToUpper();
        if (nombre == "FELICIDAD") Invoke("RevertirFelicidadTexto", tiempoNombre);
        else if (nombre == "DUDA") Invoke("RevertirDudaTexto", tiempoNombre);
    }
    private void RevertirFelicidadTexto() { } 
    private void RevertirDudaTexto() { }

    void ActualizarEstadoConfianza()
    {
        if (ConfianzaPuntuacion >= 70) ConfigurarTextoConfianza(EstadoConfianza.Confiable, colorConfiable);
        else if (ConfianzaPuntuacion <= 30) ConfigurarTextoConfianza(EstadoConfianza.Desconfiable, colorDesconfiable);
        else ConfigurarTextoConfianza(EstadoConfianza.Dudable, colorDudable);
    }

    void ConfigurarTextoConfianza(EstadoConfianza estado, Color color)
    {
        ConfianzaStateText.text = estado.ToString().ToUpper();
        ConfianzaStateText.color = color;
    }
}