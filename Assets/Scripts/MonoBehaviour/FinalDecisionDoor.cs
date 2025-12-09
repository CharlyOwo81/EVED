using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FinalDecisionDoor : MonoBehaviour
{
    [Header("Nombres EXACTOS de tus escenas (archivos)")]
    public string escenaRebelde = "Video_Rebelde"; 
    public string escenaConformista = "Video_Conformista"; 
    public string escenaCaptura = "Video_Captura"; 

    private static List<string> bolsaDeFinales = new List<string>();
    private static int frameDeCarga = -1;
    private string escenaAsignada;

    void Awake()
    {
        if (frameDeCarga != Time.frameCount)
        {
            frameDeCarga = Time.frameCount;
            ReiniciarYBarajarBolsa();
        }

        AsignarFinal();
    }

    void ReiniciarYBarajarBolsa()
    {
        bolsaDeFinales.Clear();
        bolsaDeFinales.Add(escenaRebelde);
        bolsaDeFinales.Add(escenaConformista);
        bolsaDeFinales.Add(escenaCaptura);

        for (int i = 0; i < bolsaDeFinales.Count; i++)
        {
            string temp = bolsaDeFinales[i];
            int randomIndex = Random.Range(i, bolsaDeFinales.Count);
            bolsaDeFinales[i] = bolsaDeFinales[randomIndex];
            bolsaDeFinales[randomIndex] = temp;
        }
    }

    void AsignarFinal()
    {
        if (bolsaDeFinales.Count > 0)
        {
            escenaAsignada = bolsaDeFinales[0];
            bolsaDeFinales.RemoveAt(0); 
            gameObject.name = "Puerta_" + escenaAsignada; 
        }
        else
        {
            escenaAsignada = escenaCaptura;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"El jugador entró en: {gameObject.name}. Cargando escena...");
            SceneManager.LoadScene(escenaAsignada);
        }
    }
}