using UnityEngine;

public class TPCameraController : MonoBehaviour
{
    [Header("Configuración de la Cámara")]
    [Tooltip("El Transform del personaje que la cámara debe seguir.")]
    public Transform target;
    [Tooltip("Distancia horizontal (lado a lado) de la cámara respecto al personaje.")]
    public float lateralOffset = 0f;
    [Tooltip("Distancia vertical (altura) de la cámara respecto al personaje.")]
    public float height = 2f;
    [Tooltip("Distancia detrás del personaje.")]
    public float distance = 4f;

    [Header("Control de Mouse")]
    public float mouseSensitivity = 100f;
    [Tooltip("Límite de rotación vertical hacia arriba (ej: 70).")]
    public float verticalClamp = 70f;
    
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        if (target == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");

            if (playerObject != null) 
            {
                target = playerObject.transform;
            }
            else
            {
                Debug.LogError("TPCameraController no encontró un GameObject con la etiqueta 'Player'. Asegúrate de que tu personaje tenga la etiqueta correcta.");
                return;
            }
        }
        
        Cursor.lockState = CursorLockMode.Locked; 
        
        if (target != null)
        {
            rotationX = target.eulerAngles.y;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX += mouseX;
        
        rotationY -= mouseY;

        rotationY = Mathf.Clamp(rotationY, -verticalClamp, verticalClamp);

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        
        target.rotation = Quaternion.Euler(0, rotationX, 0); 

        Vector3 negDistance = new Vector3(lateralOffset, height, -distance);
        
        transform.rotation = rotation;
        transform.position = target.position + rotation * negDistance; 
    }
}