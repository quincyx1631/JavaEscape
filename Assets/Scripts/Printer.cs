using System.Collections;
using UnityEngine;

public class Printer : MonoBehaviour
{
    public GameObject paperPrefab; // Reference to the paper prefab
    public Transform paperSpawnPoint; // The point where paper comes out of the printer
    public Transform itemHolder; // Player's item holder (for checking ink)
    public GameObject ink; // Ink item required for printing
    public float paperSpeed = 2f; // Speed at which paper moves forward
    public float rotationSpeed = 1f; // Rotation speed to the target rotation
    public Vector3 printForce = new Vector3(5f, 0f, 20f); // Horizontal sideward and forward force
    public float cooldownTime = 5f; // Cooldown time before the printer becomes interactable again

    public string printPressSound; // Sound effect for pressing the print button
    public string printingSound;   // Sound effect for printing process

    private bool isPrinting = false;
    private bool isPaperPrinted = false; // Track if paper has been printed

    public AlertUI alertUI;

    private void Start()
    {
        // Initially set the printer as interactable
        gameObject.tag = "Interactables";
    }

    private bool IsInkInHolder()
    {
        return ink != null && ink.transform.parent == itemHolder;
    }

    // Start printing process
    public void StartPrinting()
    {
        if (!isPrinting && !isPaperPrinted && gameObject.CompareTag("Interactables"))
        {
            if (IsInkInHolder())
            {
                ink.transform.SetParent(null); // Remove ink from holder
                ink.SetActive(false); // Hide ink to simulate usage

                gameObject.tag = "Printing"; // Change printer's tag to show it's printing

                // Play sound when the user presses the print button
                AudioManager.Instance.Play(printPressSound);

                StartCoroutine(PrintPaper());
            }
            else
            {
                alertUI.ShowAlert("The printer needs ink to operate");
                Debug.Log("The printer needs ink to operate.");
            }
        }
    }

    // Coroutine to handle paper printing
    private IEnumerator PrintPaper()
    {
        isPrinting = true;

        // Play sound when printing starts
        AudioManager.Instance.Play(printingSound);

        // Spawn the paper once
        SpawnPaper();

        // Wait for the paper animation to finish before allowing another print
        yield return new WaitForSeconds(2f); // Adjust timing to match paper print duration

        // Set the printer to "Untagged" after printing
        gameObject.tag = "Untagged"; // Stop printing process after paper has finished printing
        isPrinting = false;
        isPaperPrinted = true;

        // Start the cooldown timer before the printer becomes interactable again
        yield return new WaitForSeconds(cooldownTime); // Wait for cooldown time

        // Set the printer back to "Interactables" so it can print again
        gameObject.tag = "Interactables";
        isPaperPrinted = false; // Ready to print again
    }

    // Function to spawn paper, animate it, and apply force
    private void SpawnPaper()
    {
        // Instantiate the paper at the spawn point, with a fixed position and rotation
        GameObject newPaper = Instantiate(paperPrefab, paperSpawnPoint.position, paperSpawnPoint.rotation);

        // Set the position and rotation manually if needed
        newPaper.transform.position = paperSpawnPoint.position;
        newPaper.transform.rotation = paperSpawnPoint.rotation;

        // Apply force to simulate the printing force (sideward and forward)
        Rigidbody paperRb = newPaper.GetComponent<Rigidbody>();
        if (paperRb != null)
        {
            paperRb.AddForce(printForce, ForceMode.Impulse); // Apply the sideward force to the paper
        }

        // You can also rotate the paper while it moves
        StartCoroutine(MoveAndRotatePaper(newPaper));
    }

    // Coroutine to animate paper moving sideward and rotating
    private IEnumerator MoveAndRotatePaper(GameObject paper)
    {
        float timeElapsed = 0f;
        Vector3 startingPosition = paper.transform.position;
        Quaternion startingRotation = paperSpawnPoint.rotation; // Use the spawn point's rotation as starting rotation

        // Define target position and rotation for correct orientation
        Vector3 targetPosition = startingPosition + paperSpawnPoint.forward * 0.4f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0); // Adjust to the correct orientation as needed

        // Move and rotate the paper sideward
        while (timeElapsed < 1f)
        {
            paper.transform.position = Vector3.Lerp(startingPosition, targetPosition, timeElapsed);
            paper.transform.rotation = Quaternion.Lerp(startingRotation, targetRotation, timeElapsed * rotationSpeed);
            timeElapsed += Time.deltaTime * paperSpeed;
            yield return null;
        }
    }
}
