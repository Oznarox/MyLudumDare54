using UnityEngine;

public class Slots : MonoBehaviour
{
    public Transform[] slots; // a cs�nak �l�helyei

    public Transform GetAvailableSlot()
    {
        foreach (var slot in slots)
        {
            if (slot.childCount == 0) // ha az �l�helyen nincs t�l�l�, akkor visszaadjuk
            {
                return slot;
            }
        }
        return null; // ha minden �l�hely foglalt, akkor nullt adunk vissza
    }
}

