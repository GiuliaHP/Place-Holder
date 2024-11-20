using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float gravity = -9.8f;
    public float rotationSpeed = 200f;
    [Space(20)]
    [Header("Interaction")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode interactKey = KeyCode.C;
    public KeyCode dialogueKey = KeyCode.Space;
    [Space(20)]
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Gérer la gravité et vérifier si le joueur est au sol
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Déplacement avec les touches directionnelles
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Rotation avec les flèches gauche/droite
        RotateWithArrows();

        // Gérer la vitesse (marche ou course)
        float speed = Input.GetKey(runKey) ? runSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);

        // Appliquer la gravité
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Déclencher une interaction
        if (Input.GetKeyDown(interactKey))
        {
            Interact();
        }

        // Lancer un dialogue
        if (Input.GetKeyDown(dialogueKey))
        {
            StartDialogue();
        }
    }

    private void RotateWithArrows()
    {
        float rotationInput = Input.GetAxis("Horizontal");

        Vector3 rotation = new Vector3(0, rotationInput * rotationSpeed * Time.deltaTime, 0);
        transform.Rotate(rotation);
    }

    private void Interact()
    {
        Debug.Log("Interaction déclenchée !");
    }

    private void StartDialogue()
    {
        Debug.Log("Dialogue déclenché !");
    }
}
