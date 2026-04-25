using UnityEngine;

public class AbilityFireball : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float MovementSpeed = 6f;

    private Vector2 MoveDirection;

    public void Initialize(Vector2 Direction)
    {
        MoveDirection = Direction.normalized;
    }

    private void Update()
    {
        transform.Translate(MoveDirection * MovementSpeed * Time.deltaTime);
    }
}