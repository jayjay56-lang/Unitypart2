using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public Transform player;
    public float throwForce = 18f;
    public float throwArcHeight = 1.5f;
    public float dribbleHeight = 0.3f;
    public float dribbleSpeed = 4f;
    public float dribbleForwardOffset = 1f;
    public float dribbleSideOffset = 0.5f; // moves ball to right
    public float ballGroundHeight = 0.3f;
    public float pickupDistance = 2f;

    [HideInInspector] public bool isHeld = false;

    private Rigidbody rb;
    private float dribbleOffset = 0f;
    private bool goingUp = true;
    private PlayerMovement playerMovement;

    private float currentThrowForce;
    public float minThrowForce = 10f;
    public float maxThrowForce = 25f;
    public float powerStep = 1f;

    public Slider powerBar; // UI Slider for shot power

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = player.GetComponent<PlayerMovement>();
        currentThrowForce = throwForce;

        if (powerBar != null)
        {
            powerBar.minValue = minThrowForce;
            powerBar.maxValue = maxThrowForce;
            powerBar.value = currentThrowForce;
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying()) return;

        if (isHeld)
        {
            // Dribble position (forward + slightly right)
            Vector3 basePos = player.position + player.forward * dribbleForwardOffset + player.right * dribbleSideOffset;
            basePos.y = ballGroundHeight;

            if (playerMovement.IsMoving())
            {
                dribbleOffset += (goingUp ? 1 : -1) * dribbleSpeed * Time.deltaTime;
                if (dribbleOffset >= dribbleHeight) goingUp = false;
                if (dribbleOffset <= 0) goingUp = true;
            }
            else { dribbleOffset = 0f; goingUp = true; }

            transform.position = basePos + Vector3.up * dribbleOffset;

            // Adjust shot power with arrows
            if (Input.GetKey(KeyCode.UpArrow))
                currentThrowForce = Mathf.Min(currentThrowForce + powerStep * Time.deltaTime * 10f, maxThrowForce);
            if (Input.GetKey(KeyCode.DownArrow))
                currentThrowForce = Mathf.Max(currentThrowForce - powerStep * Time.deltaTime * 10f, minThrowForce);

            // Update UI
            if (powerBar != null) powerBar.value = currentThrowForce;

            // Shoot
            if (Input.GetKeyDown(KeyCode.Space))
                ThrowBall();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F) && Vector3.Distance(transform.position, player.position) < pickupDistance)
                PickUp();
        }
    }

    void PickUp()
    {
        isHeld = true;
        rb.isKinematic = true;
        dribbleOffset = 0f;
        goingUp = true;
        transform.position = player.position + player.forward * dribbleForwardOffset + player.right * dribbleSideOffset + Vector3.up * ballGroundHeight;
        currentThrowForce = throwForce;
    }

    public void ThrowBall()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;

        Vector3 throwDir = player.forward.normalized + Vector3.up * throwArcHeight / currentThrowForce;
        rb.AddForce(throwDir * currentThrowForce, ForceMode.VelocityChange);
    }
}
