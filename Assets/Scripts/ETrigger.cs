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
            eObject.SetActive(false); // az E objektumot letiltjuk kezd�sn�l
        }
    }

    private void Update()
    {
        // ha a boat k�zel van �s az E gombot megnyomj�k, akkor destroyoljuk az E trigger objektumot
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

    // amikor a boat kimegy a triggerb�l
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == boatObject && eObject != null)
        {
            eObject.SetActive(false); // kikapcsoljuk az E objektumot
            isBoatInside = false;
        }
    }
}