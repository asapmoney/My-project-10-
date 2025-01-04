using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;     // Movement speed of the player
    public float sprintMultiplier = 2f; // Sprint speed multiplier

    // Update is called once per frame
    void Update()
    {
        // Get input for movement (W, A, S, D keys)
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A))  // Left
            horizontal = -1f;
        if (Input.GetKey(KeyCode.D))  // Right
            horizontal = 1f;
        if (Input.GetKey(KeyCode.W))  // Up
            vertical = 1f;
        if (Input.GetKey(KeyCode.S))  // Down
            vertical = -1f;

        // Create a movement vector from the input, using X and Z axes only
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Check if the Shift key is held down for sprinting
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Multiply the movement speed by the sprint multiplier when Shift is held
            movement *= sprintMultiplier;
        }

        // Apply movement to the player (direct transform-based movement, fixed Y position)
        if (movement.magnitude > 0f)
        {
            // Only move along the X and Z axes, leaving Y unchanged
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
