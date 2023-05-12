using System.Collections;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    public GameObject outerCirclePrefab;
    public GameObject innerCirclePrefab;
    public float impactAreaSize;
    public float timeToImpact;

    private GameObject redCircle;
    private GameObject whiteCircle;
    public void Attack()
    {
        // Create the circle game objects
        redCircle = Instantiate(outerCirclePrefab, transform.position, Quaternion.identity);
        whiteCircle = Instantiate(innerCirclePrefab, transform.position, Quaternion.identity);

        // Set the scale of the red circle to the impact area size
        redCircle.transform.localScale = new Vector3(impactAreaSize, impactAreaSize, 1f);

        // Start the coroutine to expand the white circle over time
        StartCoroutine(ExpandWhiteCircle());
    }

    private IEnumerator ExpandWhiteCircle()
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < timeToImpact)
        {
            // Calculate the current scale of the white circle based on the elapsed time
            float currentScale = Mathf.Lerp(0f, impactAreaSize, elapsedTime / timeToImpact);
            whiteCircle.transform.localScale = new Vector3(currentScale, currentScale, 1f);

            // Wait for the next frame
            yield return null;

            // Update the elapsed time
            elapsedTime = Time.time - startTime;
        }

        // Destroy the circle game objects
        Destroy(redCircle);
        Destroy(whiteCircle);
    }
}
