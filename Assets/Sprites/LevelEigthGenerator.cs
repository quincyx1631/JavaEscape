using UnityEngine;

public class LevelEigthGenerator : MonoBehaviour
{
    // Define a struct to pair each newspaper with a radio sound
    [System.Serializable]
    public struct NewspaperSet
    {
        public GameObject newspaper;
        public AudioClip radioSound;
    }

    public NewspaperSet[] newspaperSets; // Array of newspaper-sound pairs
    public AudioSource radioSource;      // The radio’s AudioSource component

    private int currentSet = 0;          // Keeps track of the current active set

    void Start()
    {
        ActivateRandomNewspaperSet(); // Start by activating a random newspaper set
    }

    // Function to activate the chosen set
    public void ActivateNewspaperSet(int setIndex)
    {
        // Validate the index
        if (setIndex < 0 || setIndex >= newspaperSets.Length)
        {
            Debug.LogWarning("Set index out of range");
            return;
        }

        // Deactivate all newspapers
        foreach (var set in newspaperSets)
        {
            set.newspaper.SetActive(false);
        }

        // Activate the chosen newspaper and play its corresponding radio sound
        newspaperSets[setIndex].newspaper.SetActive(true);
        radioSource.clip = newspaperSets[setIndex].radioSound;
        radioSource.Play();

        currentSet = setIndex; // Update the current set index
    }

    // Function to activate a random newspaper set
    public void ActivateRandomNewspaperSet()
    {
        int randomIndex = Random.Range(0, newspaperSets.Length); // Get a random index
        ActivateNewspaperSet(randomIndex); // Activate the set at the random index
    }
}
