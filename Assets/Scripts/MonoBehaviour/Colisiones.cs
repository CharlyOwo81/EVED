using UnityEngine;

public class Colisiones : MonoBehaviour
{
    private DialogoData npcCercano; 

    void Update()
    {
        if (npcCercano != null && Input.GetKeyDown(KeyCode.E))
        {
            StatusManager.Instance.IniciarOAvanzarDialogo(npcCercano);
        }
    }

    private void OnTriggerEnter(Collider objeto)
    {
        DialogoData data = objeto.GetComponent<DialogoData>();
        if (data != null)
        {
            npcCercano = data;
        }
    }
    
    private void OnTriggerExit(Collider objeto)
    {
        DialogoData data = objeto.GetComponent<DialogoData>();
        if (data != null && data == npcCercano)
        {
            if(StatusManager.Instance.EstaEnDialogo())
            {
                StatusManager.Instance.CerrarDialogo();
            }
            npcCercano = null;
        }
    }
}