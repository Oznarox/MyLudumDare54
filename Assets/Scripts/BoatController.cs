using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class BoatController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float speed = 5f;
    public float reverseSpeed = 2f;
    public float rotationSpeed = 200f;

    [Header("Passenger Properties")]
    public Transform passengersParent;
    public int maxPassengers;
    public int currentPassengers = 0;
    public Slots boatSlots;
    public float ballast = 1f;
    public int CurrentPassengers
    {
        get
        {
            int count = 0;
            foreach (Transform slot in boatSlots.transform)
            {
                if (slot.childCount > 0) count++;
            }
            return count;
        }
    }

    void Start()
    {
        // A maximális túlélõk száma a slotok számával egyezik meg.
        maxPassengers = boatSlots.transform.childCount;
    }

    void Update()
    {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.E)) HandlePassengerTransfer();
        currentPassengers = CurrentPassengers;

        switch (currentPassengers)
        {
            case 0:
                ballast = 1f;
                break;
            case 1:
                ballast = 0.98f;
                break;
            case 2:
                ballast = 0.95f;
                break;
            case 3:
                ballast = 0.90f;
                break;
            case 4:
                ballast = 0.8f;
                break;
            default:
                ballast = 1f;
                break;
        }

    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Vertical") * (Input.GetAxis("Vertical") > 0 ? speed : reverseSpeed) * ballast * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * ballast * Time.deltaTime;
        transform.Translate(Vector3.up * move, Space.Self);
        transform.Rotate(0, 0, -rotation);
    }

    void HandlePassengerTransfer()
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, 1f);

        if (TryPlaceSurvivorOnIsland(nearbyObjects)) return;
        TryPickupSurvivorFromSinkingObject(nearbyObjects);
    }

    bool TryPlaceSurvivorOnIsland(Collider2D[] nearbyObjects)
    {
        foreach (var obj in nearbyObjects)
        {
            IslandController island = obj.GetComponent<IslandController>();
            if (island != null)
            {
                foreach (Transform slot in boatSlots.transform)
                {
                    Survivor survivor = slot.GetComponentInChildren<Survivor>();
                    if (survivor != null)
                    {
                        Transform availableIslandSlot = island.TryAddSurvivor(survivor);
                        if (availableIslandSlot != null) // ha találtunk szabad slotot a szigeten.
                        {
                            survivor.SetTarget(availableIslandSlot); // beállítjuk a túlélõ célpontját.
                            RemovePassengerFromBoat(slot, survivor); // eltávolítjuk a túlélõt a csónaktól.
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    void TryPickupSurvivorFromSinkingObject(Collider2D[] nearbyObjects)
    {
        foreach (var obj in nearbyObjects)
        {
            SinkingObjectController sinkingObject = obj.GetComponent<SinkingObjectController>();
            if (sinkingObject != null)
            {
                Transform availableSlot = boatSlots.GetAvailableSlot();
                if (availableSlot != null)
                {
                    Survivor survivor = sinkingObject.RemoveSurvivor();
                    if (survivor != null)
                    {
                        SetSurvivorToSlot(survivor, availableSlot);
                        break;
                    }
                }
            }
        }
    }
    void SetSurvivorToSlot(Survivor survivor, Transform availableSlot)
    {
        survivor.transform.SetParent(availableSlot);
        survivor.parentObject = gameObject;
        survivor.SetTarget(availableSlot);
    }
    void RemovePassengerFromBoat(Transform boatSlot, Survivor survivor)
    {
        // kivesszük a túlélõt a csónak slotjából.
        survivor.transform.SetParent(null);
        boatSlot.DetachChildren();
    }
}

