using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : MonoBehaviour
{
    public GameObject parentObject;
    public float moveSpeed = 2f;
    public float walkSpeed = 0.5f;
    public Transform targetPosition; // a c�lpont, ahov� a t�l�l� mozogni fog
    public bool escaped = false;
    public GameObject targetObject;
    public GameObject spawnObject;

    public float swimingTime = 6f;
    public float sinkingTime = 4f;
    public bool isSunk = false;

    public SinkingObjectController spawnObjectController;
    public float currentSwimingTime;
    public float elapsedTime = 0f;
    public SpriteRenderer spriteRenderer;
    public bool isSwiming = false;

    public LevelManager levelManager;

    public bool survived = false;

    public List<AudioClip> helpSounds;
    public List<AudioClip> escapedSounds;
    public List<AudioClip> survivedSounds;

    [SerializeField]
    public AudioSource audioSource;

    private bool isEscaped = false;
    private bool isSurvived = false;

    void Start()
    {
        
        spawnObject = this.transform.parent.gameObject;
        targetObject = GameObject.FindObjectOfType<BoatController>().gameObject;

        spawnObjectController = spawnObject.GetComponent<SinkingObjectController>();
        currentSwimingTime = swimingTime;
        if (spriteRenderer == null)
        spriteRenderer = GetComponent<SpriteRenderer>();

        levelManager = FindObjectOfType<LevelManager>();
        levelManager.IncrementAllSurvivors();


        /*
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned in the inspector");
            return;
        }
        StartCoroutine(PlayHelpSounds());*/
    }

    void Update()
    {
        if (targetPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            if (transform.position == targetPosition.position)
            {
                targetPosition = null; // c�lpont el�r�se ut�n nullra �ll�tjuk
            }
        }

        if (!escaped)
        {
            MoveTowardsTarget();
        }


        if (!escaped && spawnObjectController.sunk)
        {
            currentSwimingTime -= Time.deltaTime;
            if (currentSwimingTime < 1 && !isSunk)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= sinkingTime)
                {
                    isSunk = true;
                    spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
                }
                else
                {
                    float alpha = 1f - (elapsedTime / sinkingTime);
                    spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
                }
            }
            isSwiming = true;
        }
        else if (escaped)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            currentSwimingTime = swimingTime;
            elapsedTime = 0f;
            isSwiming = false;
        }
        if (isSunk && !escaped) 
        {
            // itt le kell vonni egyet a spawnObjec-en l�v� survivors v�ltoz� sz�m�b�l

            if (spawnObjectController != null)
            {
                spawnObjectController.survivors--; 
            }
            levelManager.IncrementDeadSurvivors();
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target)
    {
       // OnEscaped();
        escaped = true;
        isEscaped = true;
        
        RigidbodyOff();
        targetPosition = target;

    }

    void MoveTowardsTarget()
    {
        if (targetObject != null)
        {
            // ir�ny kisz�m�t�sa
            Vector2 direction = ((Vector2)targetObject.transform.position - (Vector2)transform.position).normalized;
            Vector2 newPosition = (Vector2)transform.position + direction * walkSpeed * Time.deltaTime;

            // collider ellen�rz�se
            Collider2D spawnerCollider = spawnObject.GetComponent<Collider2D>();
            if (spawnerCollider.OverlapPoint(newPosition))
            {
                // ha a Collider-�n bel�l van, mozgasd a Survivor-t
                transform.position = newPosition;
            }
            else
            {
                // ha k�v�l van, akkor maradjon a legk�zelebbi ponton
                Vector2 closestPoint = spawnerCollider.ClosestPoint(newPosition);
                transform.position = closestPoint;
            }
        }
    }

    void RigidbodyOff()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }
    }
    /*
    private IEnumerator PlayHelpSounds()
    {
        while (!escaped)
        {
            if (helpSounds.Count > 0)
            {
                AudioClip clip = helpSounds[Random.Range(0, helpSounds.Count)];
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(clip.length);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void OnEscaped()
    {
        if (!isEscaped && escapedSounds.Count > 0)
        {
            isEscaped = true;
            AudioClip clip = escapedSounds[Random.Range(0, escapedSounds.Count)];
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void OnSurvived()
    {
        if (!isSurvived && survivedSounds.Count > 0)
        {
            isSurvived = true;
            AudioClip clip = survivedSounds[Random.Range(0, survivedSounds.Count)];
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    */
    // ide tov�bbi tulajdons�gok �s viselked�sek a Survivor-ekhez adni (megfullad�s sebess�g, �sz�si sebess�g, kiadott hangok, s�lya stb. )
}