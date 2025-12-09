using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensibilidad")]
    [Tooltip("Controla la velocidad de movimiento del ratón.")]
    public float mouseSensitivity = 100f;

    [Header("Límites de la Vista")]
    [Tooltip("Ángulo máximo para mirar hacia abajo.")]
    public float maxLookDown = 90f;
    [Tooltip("Ángulo máximo para mirar hacia arriba.")]
    public float maxLookUp = -90f; 

    // Variable para almacenar la rotación vertical actual de la cámara
    private float rotationX = 0f;
    
    // Referencia al transform del cuerpo del jugador (para la rotación horizontal)
    public Transform playerBody;

    void Start()
    {
        // Bloquear y ocultar el cursor del ratón en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        
        // Si no se asigna el cuerpo del jugador en el Inspector, asume que es el padre de la cámara
        if (playerBody == null && transform.parent != null)
        {
            playerBody = transform.parent.transform;
        }
    }

    void Update()
    {
        // 1. Obtener el input del ratón
        // Multiplicamos por Time.deltaTime para hacer el movimiento independiente de la tasa de frames
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. Controlar la rotación vertical (Arriba/Abajo)
        // Restamos mouseY porque el eje Y del ratón está invertido para la rotación de la cámara (mover el ratón hacia arriba es mirar hacia abajo en el eje X)
        rotationX -= mouseY;
        
        // Limitar la rotación vertical para que el personaje no pueda mirar completamente hacia atrás
        rotationX = Mathf.Clamp(rotationX, maxLookUp, maxLookDown);

        // Aplicar la rotación a la CÁMARA (Eje X local)
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // 3. Controlar la rotación horizontal (Izquierda/Derecha)
        // Aplicar la rotación al CUERPO del jugador (Eje Y global)
        if (playerBody != null)
        {
             playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}