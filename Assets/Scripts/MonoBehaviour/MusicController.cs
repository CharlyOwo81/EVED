using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicController : PersistentSingleton<MusicController>
{
    private AudioSource audioSource;
    private float initialVolume;

    [Header("Música de Fondo por Defecto (Una Sola Pista)")]
    public AudioClip defaultBGM; 
    
    [Header("Música de Fondo (Ciclo Aleatorio)")]
    [Tooltip("La lista de canciones para el ciclo si se activa la opción de abajo.")]
    public AudioClip[] defaultBGMList;
    
    [Tooltip("Si es TRUE, el MusicController iniciará el ciclo aleatorio con 'defaultBGMList'.")]
    public bool startRandomCycleByDefault = false;
    
    [Tooltip("Tiempo de transición entre canciones si usa el ciclo aleatorio por defecto.")]
    public float defaultCrossFadeTime = 3.0f;
    
    // ------------------------------------------------------------------

protected override void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true; 
        }
        initialVolume = audioSource.volume;
        
        if (startRandomCycleByDefault && defaultBGMList != null && defaultBGMList.Length > 0)
        {
            StartCoroutine(CycleRandomMusic(defaultBGMList, defaultCrossFadeTime));
        }
        else if (defaultBGM != null && !audioSource.isPlaying)
        {
            audioSource.clip = defaultBGM;
            audioSource.Play();
        }
    }

    public void StopAndPlay(AudioClip newClip)
    {
        StopAllCoroutines(); 
        
        if (newClip != null)
        {
            if (audioSource.clip != newClip || !audioSource.isPlaying)
            {
                audioSource.clip = newClip;
                audioSource.Play();
                audioSource.volume = initialVolume;
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void StartRandomCycle(AudioClip[] newSongs, float crossFadeTime)
    {
        StopAllCoroutines(); 
        
        if (newSongs != null && newSongs.Length > 0)
        {
            StartCoroutine(CycleRandomMusic(newSongs, crossFadeTime));
        }
        else
        {
            audioSource.Stop(); 
        }
    }

    private IEnumerator CycleRandomMusic(AudioClip[] songs, float crossFadeTime)
    {
        int lastIndex = -1;
        float fadeOutDuration = crossFadeTime / 2f;
        float fadeInDuration = crossFadeTime / 2f;
        
        audioSource.loop = false; 
        
        while (true)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, songs.Length);
            } while (randomIndex == lastIndex && songs.Length > 1);
            
            AudioClip nextClip = songs[randomIndex];
            lastIndex = randomIndex;

            if (audioSource.isPlaying)
            {
                float startVolume = audioSource.volume;
                float timer = 0f;
                while (timer < fadeOutDuration)
                {
                    timer += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutDuration);
                    yield return null;
                }
                audioSource.Stop();
                audioSource.volume = initialVolume;
            }

            audioSource.clip = nextClip;
            audioSource.volume = 0f;
            audioSource.Play();

            float timerIn = 0f;
            while (timerIn < fadeInDuration)
            {
                timerIn += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0f, initialVolume, timerIn / fadeInDuration);
                yield return null;
            }
            audioSource.volume = initialVolume;

            float waitTime = audioSource.clip.length - fadeOutDuration;
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                yield return null; 
            }
        }
    }


    /// <summary>
    /// Desvanece la música gradualmente hasta el silencio.
    /// </summary>
    public IEnumerator FadeOut(float duration)
    {
        // Detener el ciclo aleatorio si está activo
        StopAllCoroutines();
        
        float startVolume = audioSource.volume;
        float timer = 0f;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = initialVolume; // Restablecer volumen para la siguiente vez
    }

    /// <summary>
    /// Inicia la música y aumenta el volumen gradualmente.
    /// </summary>
    public IEnumerator FadeIn(float duration)
    {
        if (audioSource.clip == null) yield break;
        
        // Detener el ciclo aleatorio si está activo
        StopAllCoroutines(); 
        
        audioSource.volume = 0f; // Empezamos desde cero
        audioSource.Play();
        
        float endVolume = initialVolume; 
        float timer = 0f;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, endVolume, timer / duration);
            yield return null;
        }
        audioSource.volume = initialVolume; // Asegurar el volumen final
    }

// ⭐️ Detención temporal y reanudación ⭐️
    /// <summary>
    /// Detiene la música, espera el retraso y luego reanuda el clip de la escena actual (si hay uno).
    /// </summary>
    public void PauseForDuration(float duration, AudioClip clipToResume)
    {
        // Detiene cualquier coroutine de Fade o el ciclo aleatorio
        StopAllCoroutines(); 
        audioSource.Stop();
        
        // Reinicia la coroutine que espera y luego reanuda la música de la escena
        StartCoroutine(ResumeMusicAfterDelay(duration, clipToResume));
    }
    
    IEnumerator ResumeMusicAfterDelay(float delay, AudioClip clipToResume)
    {
        yield return new WaitForSeconds(delay);
        // Cuando termine el delay, reanuda la música que estaba sonando o la música de fondo
        StopAndPlay(clipToResume);
    }
}