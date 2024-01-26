using UnityEngine;

public class ETrigger : MonoBehaviour
{
    public GameObject boatObject; // a boat objektum
    public GameObject eObject;    // az E objektum a tutorialhoz

    private bool isBoatInside = false;

    private void Start()
    {
        if (eObject != null)
        {
            eObject.SetActive(false); // az E objektumot letiltjuk kezdésnél
        }
    }

    private void Update()
    {
        // ha a boat közel van és az E gombot megnyomják, akkor destroyoljuk az E trigger objektumot
        if (isBoatInside && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
        }
    }

    // amikor a boat belemegy a triggerbe
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == boatObject && eObject != null)
        {
            eObject.SetActive(true); // Bekapcsoljuk az E objektumot
            isBoatInside = true;
        }
    }

    // amikor a boat kimegy a triggerbõl
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == boatObject && eObject != null)
        {
            eObject.SetActive(false); // kikapcsoljuk az E objektumot
            isBoatInside = false;
        }
    }
}