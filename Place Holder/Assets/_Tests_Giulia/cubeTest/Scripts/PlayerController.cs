using UnityEngine;
using DG.Tweening;

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
    public KeyCode pauseKey = KeyCode.Escape;

    [Space(20)]
    public CanvasGroup pauseCanvas;

    public Animator foxAnimator;  // Référence à l'Animator
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public bool pauseBool;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        if (!pauseBool)
        {
            RotateWithArrows();
        }

        float speed = Input.GetKey(runKey) ? runSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);

        // Appliquer la gravité
        velocity.y += gravity * Time.deltaTime;
        if (!pauseBool)
        {
            controller.Move(velocity * Time.deltaTime);

            if (Input.GetKeyDown(interactKey))
            {
                Interact();
            }

            if (Input.GetKeyDown(pauseKey))
            {
                Pause();
            }
        }
        else if (pauseBool)
        {
            // Stopper la vélocité quand le jeu est en pause
            velocity.y = 0f; // Empêche la chute
        }

        // **Mise à jour de l'Animator**
        UpdateAnimator(move, speed);
    }

    private void UpdateAnimator(Vector3 move, float speed)
    {
        if (foxAnimator != null)
        {
            // Vérifie si le personnage se déplace
            bool isWalking = move.magnitude > 0.1f && speed == walkSpeed;
            bool isRunning = move.magnitude > 0.1f && speed == runSpeed;

            // Met à jour les paramètres de l'Animator
            foxAnimator.SetBool("isWalking", isWalking);
            foxAnimator.SetBool("isRunning", isRunning);
            foxAnimator.SetBool("isGrounded", isGrounded);
            foxAnimator.SetFloat("velocityY", velocity.y);
        }
    }

    public void Pause()
    {
        pauseBool = true;
        if (pauseCanvas != null)
        {
            pauseCanvas.transform.localScale = Vector3.zero;
            pauseCanvas.alpha = 0f;

            pauseCanvas.gameObject.SetActive(true);

            pauseCanvas.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            pauseCanvas.DOFade(1f, 0.5f);
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
