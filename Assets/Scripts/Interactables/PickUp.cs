using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform itemHolder;
    public string pickupSoundName; // Name of the sound in AudioManager
    private Transform originalParent;
    private Rigidbody rb;
    private Vector3 originalScale;

    private void Start()
    {
        gameObject.SetActive(true);
        originalParent = transform.parent;
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
    }

    public bool IsItemHolderEmpty()
    {
        return itemHolder.childCount == 0;
    }

    public void pickUp()
    {
        if (itemHolder != null && IsItemHolderEmpty())
        {
            Debug.Log("Picking up item: " + gameObject.name);

            // Play the pickup sound
            if (!string.IsNullOrEmpty(pickupSoundName))
            {
                AudioManager.Instance.Play(pickupSoundName);
            }

            transform.SetParent(itemHolder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
        }
    }

    public void Drop()
    {
        if (itemHolder != null && transform.parent == itemHolder)
        {
            Debug.Log("Dropping item: " + gameObject.name);
            transform.SetParent(null);
            transform.localScale = originalScale;
            transform.position = itemHolder.position;
            transform.rotation = Quaternion.identity;
            GetComponent<Collider>().enabled = true;
            rb.isKinematic = false;

            Vector3 forwardDirection = itemHolder.forward;
            Vector3 tossForce = forwardDirection * 5f + Vector3.down * 2f;
            rb.AddForce(tossForce, ForceMode.Impulse);

            Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rb.AddTorque(randomTorque * 10f, ForceMode.Impulse);
        }
    }
}
