using UnityEngine;
using System.Collections;

public class LevelEightGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct NewspaperSet
    {
        public GameObject newspaper;
        public AudioClip radioSound;
    }

    public NewspaperSet[] newspaperSets;
    public AudioSource radioSource;

    private int currentSet = 0;

    void Start()
    {
        ActivateNewspaperSet(0);
    }

    public void ActivateNewspaperSet(int setIndex)
    {
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

        // Stop and clear the audio source completely before setting the new clip
        radioSource.Stop();
        radioSource.clip = null;

        // Activate the selected newspaper
        newspaperSets[setIndex].newspaper.SetActive(true);

        // Start a coroutine to safely set the audio clip with a slight delay
        StartCoroutine(SetRadioClipDelayed(newspaperSets[setIndex].radioSound));

        currentSet = setIndex;
    }

    // Coroutine to add a slight delay when assigning the new clip
    private IEnumerator SetRadioClipDelayed(AudioClip newClip)
    {
        yield return new WaitForEndOfFrame(); // Delay for one frame
        radioSource.clip = newClip;
    }

    public void NextNewspaperSet()
    {
        int nextSet = (currentSet + 1) % newspaperSets.Length;
        ActivateNewspaperSet(nextSet);
    }
}
