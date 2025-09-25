using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    public float moveRange = 2f;
    public float difficultyIncrease = 0.5f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Add score
            ScoreManager.Instance.AddScore(1);

            // Move hoop slightly for difficulty
            float offsetX = Random.Range(-moveRange, moveRange) + ScoreManager.Instance.GetScore() * difficultyIncrease;
            float offsetZ = Random.Range(-moveRange, moveRange) + ScoreManager.Instance.GetScore() * difficultyIncrease;

            transform.position = new Vector3(
                originalPosition.x + offsetX,
                originalPosition.y,
                originalPosition.z + offsetZ
            );

            // Check win condition
            GameManager.Instance?.CheckWinCondition(ScoreManager.Instance.GetScore());
        }
    }
}
