using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject wasdObject; // a Canvas-en lévõ W, A, S, D objektum (tutorialhoz)
    public Transform playerTransform; // a játékos pozíciójához
    public Vector3 offset; // offset az objektum pozíciójának finomításához

    void Start()
    {
        if (wasdObject != null)
        {
            wasdObject.SetActive(true); 
        }
        else
        {
            Debug.LogError("nincs W, A, S, D object hozzáadva");
        }
    }

    void Update()
    {
        if (wasdObject != null && wasdObject.activeSelf) // ellenõrzi, hogy az objektum be van-e kapcsolva
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                wasdObject.SetActive(false); // kikapcsolja a W, A, S, D objektumot
            }
        }
    }
}