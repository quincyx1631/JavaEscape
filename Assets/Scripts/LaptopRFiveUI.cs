using UnityEngine;

public class LaptopRFiveUI : MonoBehaviour
{
    public GameObject laptopSlidesUI;    // UI that contains the next slide button
    public GameObject laptop3DSlides;    // The 3D object in the scene that displays the laptop's slides
    public GameObject previousUI;        // UI elements to disable
    public GameObject previous3DObject;  // 3D objects to disable
      // Reference to your existing camera switch script

    // This is called when the player presses the LaptopButton in the Laptop UI
    public void OnLaptopButtonPress()
    {
        // Disable the previous UI and 3D object
        if (previousUI != null)
        {
            previousUI.SetActive(false);
        }

        if (previous3DObject != null)
        {
            previous3DObject.SetActive(false);
        }

        // Enable the slides UI in the UI canvas
        laptopSlidesUI.SetActive(true);

        // Enable the 3D slides object in the scene
        laptop3DSlides.SetActive(true);

      
    }
}
