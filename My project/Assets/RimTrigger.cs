using UnityEngine;

public class RimTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Optional: check if ball is moving downward
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb.velocity.y < 0)
            {
                ScoreManager.Instance.AddScore(1);
                GameManager.Instance?.CheckWinCondition(ScoreManager.Instance.GetScore());
            }
        }
    }
}
