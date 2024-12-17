using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target; // Reference to the unit this health bar should follow
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Offset above the unit

    private RectTransform healthBarRect;
    private Canvas canvas;

    private void Start()
    {
        healthBarRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("HealthBar must be inside a Canvas.");
        }
    }

    private void LateUpdate()
    {
        if (target != null && canvas != null)
        {
            // Convert the world position of the unit to a screen position
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position + offset);

            // Update the health bar's position in the canvas
            healthBarRect.position = screenPosition;

            // Hide the health bar if the unit is off-screen
            if (screenPosition.z < 0 || screenPosition.y < 0 || screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y > Screen.height)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}
