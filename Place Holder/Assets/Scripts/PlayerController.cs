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

    public Animator foxAnimator; // Référence à l'Animator

    [Space(20)]
    [Header("Inactivité")]
    public float digTriggerTimeMin = 5f; // Temps minimum d'inactivité avant l'animation
    public float digTriggerTimeMax = 10f; // Temps maximum d'inactivité avant l'animation
    private float digTimer;
    private float randomDigTime;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public bool pauseBool;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        ResetDigTimer();
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
        Vector3 move = Vector3.zero;

        if (!pauseBool)
        {
            move = transform.forward * vertical;
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
            velocity.y = 0f; // Empêche la chute en pause
        }

        // **Mise à jour de l'Animator**
        UpdateAnimator(move, horizontal, speed);

        // Vérifier l'inactivité
        CheckDig(move);
    }

    private void UpdateAnimator(Vector3 move, float horizontal, float speed)
    {
        if (foxAnimator != null)
        {
            bool isWalking = move.magnitude > 0.1f && speed == walkSpeed;
            bool isRunning = move.magnitude > 0.1f && speed == runSpeed;
            bool isStrafingLeft = horizontal < -0.1f;
            bool isStrafingRight = horizontal > 0.1f;

            foxAnimator.SetBool("isWalking", isWalking);
            foxAnimator.SetBool("isRunning", isRunning);
            foxAnimator.SetBool("isGrounded", isGrounded);
            foxAnimator.SetFloat("velocityY", velocity.y);
            foxAnimator.SetBool("isStrafingLeft", isStrafingLeft);
            foxAnimator.SetBool("isStrafingRight", isStrafingRight);
        }
    }

    private void CheckDig(Vector3 move)
    {
        // Calculer la magnitude totale du déplacement (inclut strafe)
        float totalMovementMagnitude = move.magnitude + Mathf.Abs(Input.GetAxis("Horizontal"));

        if (totalMovementMagnitude < 0.1f) // Si le personnage ne bouge pas significativement
        {
            digTimer += Time.deltaTime;

            // Si le temps d'inactivité dépasse le temps aléatoire
            if (digTimer >= randomDigTime)
            {
                SetDigAnimation(true);
            }
        }
        else
        {
            // Si le personnage bouge, désactiver l'animation d'inactivité
            SetDigAnimation(false);
            ResetDigTimer();
        }
    }


    private void SetDigAnimation(bool state)
    {
        if (foxAnimator != null)
        {
            foxAnimator.SetBool("Dig", state);
        }
    }

    private void ResetDigTimer()
    {
        digTimer = 0f;
        randomDigTime = Random.Range(digTriggerTimeMin, digTriggerTimeMax);
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
}
