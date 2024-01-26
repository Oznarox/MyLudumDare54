using UnityEngine;

public class Slots : MonoBehaviour
{
    public Transform[] slots; // a csónak ülõhelyei

    public Transform GetAvailableSlot()
    {
        foreach (var slot in slots)
        {
            if (slot.childCount == 0) // ha az ülõhelyen nincs túlélõ, akkor visszaadjuk
            {
                return slot;
            }
        }
        return null; // ha minden ülõhely foglalt, akkor nullt adunk vissza
    }
}

