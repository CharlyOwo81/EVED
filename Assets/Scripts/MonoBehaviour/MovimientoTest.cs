using UnityEngine;

public class SinnyoMovementTP : MonoBehaviour
{
    [Header("Configuración Base")]
    public float velocidadBase = 450f; // Velocidad normal de caminar
    public float velocidadMovimientoActual; // La velocidad que se usa en el frame

    [Header("Configuración de Sprint")]
    public float multiplicadorSprint = 2.0f; // Multiplicador para correr (ej. 6 * 2.0 = 12 m/s)
    public float velocidadRotacion = 10f; 

    [Header("Configuración de Gravedad y Salto")]
    public float fuerzaGravedad = -10f; 
    public float fuerzaSalto = 3.0f; 
    private float velocidadVertical; 
    private int saltosRestantes;
    public int maxSaltos = 2;

    [Header("Referencias de Componentes")]
    private CharacterController controlador;
    private Animator animador; 

    void Start()
    {
        controlador = GetComponent<CharacterController>();
        animador = GetComponentInChildren<Animator>(); 
        
        saltosRestantes = maxSaltos; 
        velocidadMovimientoActual = velocidadBase;
    }

    void Update()
    {
        float movimientoX = Input.GetAxis("Horizontal");
        float movimientoZ = Input.GetAxis("Vertical"); 
        

        if (Input.GetKey(KeyCode.LeftShift) && movimientoZ > 0)
        {
            velocidadMovimientoActual = velocidadBase * multiplicadorSprint;
        }
        else
        {
            velocidadMovimientoActual = velocidadBase;
        }
        
        if (controlador.isGrounded)
        {
            velocidadVertical = -1f; 
            saltosRestantes = maxSaltos; 

            if (Input.GetButtonDown("Jump")) 
            {
                velocidadVertical = fuerzaSalto;
                saltosRestantes--;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && saltosRestantes > 0)
            {
                velocidadVertical = fuerzaSalto; 
                saltosRestantes--;
            }

            velocidadVertical += fuerzaGravedad * Time.deltaTime;
        }
 
        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        Vector3 movimiento = (forward * movimientoZ) + (right * movimientoX);

        Vector3 movimientoFinal = movimiento;
        movimientoFinal.y = velocidadVertical; 

        controlador.Move(movimientoFinal * Time.deltaTime * velocidadMovimientoActual); // Usa la velocidad ACTUAL

        if (movimiento.magnitude >= 0.1f) 
        {
            Quaternion nuevaRotacion = Quaternion.LookRotation(movimiento);
            transform.rotation = Quaternion.Slerp(transform.rotation, nuevaRotacion, 
                                                     Time.deltaTime * velocidadRotacion);
        }

        float velocidadAnimacion = movimiento.magnitude;
        
        if (velocidadMovimientoActual > velocidadBase)
        {
            animador.SetFloat("Speed", velocidadAnimacion * multiplicadorSprint); 
        }
        else
        {
            animador.SetFloat("Speed", velocidadAnimacion);
        }
    }
}