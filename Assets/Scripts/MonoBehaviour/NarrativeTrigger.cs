using UnityEngine;

public class NarrativeTrigger : MonoBehaviour
{
    // Cambiamos StatusManager a StatusManager (o el nombre que uses)
    public StatusManager statusManager; 

    // Define qué barra afecta y cuánto
    public enum BarraAfectada { Felicidad, Confianza, Duda }
    public BarraAfectada barra; 
    public float cantidadDeCambio = 10f; // Positivos para subir, negativos para bajar

    [Header("Configuración de Colisión")]
    public bool destruirAlColisionar = true; 
    public bool efectoPersistente = false;

    void Start()
    {
        if (statusManager == null)
        {
            statusManager = StatusManager.Instance;

            if (statusManager == null)
            {
                Debug.LogError("ERROR CRÍTICO: No se encontró 'StatusManager' en la escena. Asegúrate de arrastrar el prefab '_StatusManager' a esta escena o iniciar el juego desde el Menú Principal.");
                this.enabled = false; 
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (statusManager == null) return;

        if (other.CompareTag("Player")) 
        {
            if (!efectoPersistente)
            {
                AplicarCambio();
                if (destruirAlColisionar)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (statusManager == null) return;

        if (other.CompareTag("Player") && efectoPersistente)
        {
            float cambioPorSegundo = cantidadDeCambio * Time.deltaTime * 0.1f;
            AplicarCambio(cambioPorSegundo);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && efectoPersistente)
        {
            Debug.Log(gameObject.name + " dejó de afectar la barra.");
        }
    }

    private void AplicarCambio(float cantidad = 0f)
    {
        float cambio = (cantidad == 0f) ? cantidadDeCambio : cantidad;

        switch (barra)
        {
            case BarraAfectada.Felicidad:
                statusManager.CambiarFelicidad(cambio); 
                statusManager.MostrarNombreTemporalmente("FELICIDAD", statusManager.FelicidadValueText);
                break;
                
            case BarraAfectada.Confianza:
                statusManager.CambiarConfianza(cambio);
                break;
                
            case BarraAfectada.Duda:
                statusManager.CambiarDuda(cambio);
                statusManager.MostrarNombreTemporalmente("DUDA", statusManager.DudaValueText);
                break;
        }
    }
}