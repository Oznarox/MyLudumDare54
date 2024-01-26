using System.Threading;
using TMPro;
using UnityEngine;

public class SinkingObjectController : MonoBehaviour
{
    public int survivors = 3; // a s�llyed� objektumon l�v� t�l�l�k sz�ma

    public GameObject survivorPrefab; // a Survivor prefabot az inspectorban kell hozz�adni
    public Transform survivorsParent; // sz�l� amely tartalmazza a t�l�l�k hely�t
    public int sinkingTime = 60;
    public int currentSinkingTime = 60;
    public int extraTime = 5;
    public float sinkingDelay = 5f;
    public bool sunk = false;
    public SpriteRenderer spriteRenderer; // public SpriteRenderer referencia
    private float elapsedTime = 0f;
    private float timeElapsedSinceLastDecrease = 0f;

    [Header("UI Elements")]
    public TextMeshProUGUI CountDownText;

    [Header("Text Properties")]
    public TMP_FontAsset font;
    public Color fontColorWhite = Color.white;
    public Color fontColorYellow = Color.yellow;
    public Color fontColorRed = Color.red;
    public int fontSize = 20;
    public TextAlignmentOptions textAlignment = TextAlignmentOptions.Center;

    public Vector3 textPositionOffset = new Vector3(0, -1.2f, 0);

    private void Start()
    {
        for (int i = 0; i < survivors; i++)
        {
            Instantiate(survivorPrefab, GetRandomPosition(), Quaternion.identity, survivorsParent);
        }
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>(); // ha nem lett k�zzel hozz�rendelve, pr�b�ljuk meg automatikusan lek�rni.

        currentSinkingTime = sinkingTime;
        CreateCountDownUI();
    }



    public void Update()
    {
        if (!sunk)
        {
            timeElapsedSinceLastDecrease += Time.deltaTime;
            if (timeElapsedSinceLastDecrease >= 1f) // ha eltelt 1 m�sodperc
            {
                currentSinkingTime--; // cs�kkentj�k a currentSinkingTime-ot
                timeElapsedSinceLastDecrease = 0f; // resetelj�k az eltelt id�t
            }
            Sinking();
        }

        if (survivors <= 0 && sunk)
        {
            Destroy(gameObject);
            if (CountDownText != null)
                Destroy(CountDownText.gameObject);
        }
        if (CountDownText != null) // ezt j�l van megcsin�lva?
        {
            CountdownTextUpdate();
        }


    }

    private Vector3 GetRandomPosition()
    {
        // Itt kell meghat�rozni egy random poz�ci�t a SinkingObject-en bel�l a Survivor instanci�khoz.
        // P�ld�ul:
        float x = Random.Range(-0.1f, 0.1f);
        float y = Random.Range(-0.1f, 0.1f);
        return transform.position + new Vector3(x, y, 0);
    }
    public Survivor RemoveSurvivor()
    {
        if (survivors > 0)
        {
            survivors--;
            // A t�l�l� instanci�j�t kiveszi a list�b�l �s visszak�ldi.
            Survivor survivor1 = survivorsParent.GetChild(survivors).GetComponent<Survivor>();
            Survivor survivor = survivor1;
            survivor.transform.SetParent(null); // elt�vol�tja a sz�l�t�l
            currentSinkingTime = currentSinkingTime + extraTime;
            return survivor;
        }
        return null;
    }


    public void Sinking()
    {
        if (currentSinkingTime < 1 && !sunk)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= sinkingDelay)
            {
                sunk = true; // Megjel�lj�k, hogy az objektum els�llyedt.
                spriteRenderer.color = new Color(1f, 1f, 1f, 0f); // Teljesen �tl�tsz�v� tessz�k a sprite-ot.
            }
            else
            {
                float alpha = 1f - (elapsedTime / sinkingDelay); // Az id� m�l�s�val cs�kken az alpha �rt�ke (�tl�tsz�s�g).
                spriteRenderer.color = new Color(1f, 1f, 1f, alpha); // Az alpha �rt�k friss�t�se.
            }
        }
    }


    void CountdownTextUpdate()
    {

        if (currentSinkingTime < 1)
        {
            Destroy(CountDownText.gameObject);
            return;
        }

        CountDownText.text = $"{currentSinkingTime}";
        Vector3 islandPosition = transform.position + textPositionOffset;
        CountDownText.rectTransform.position = Camera.main.WorldToScreenPoint(islandPosition);

        if (currentSinkingTime > 10)
            CountDownText.color = fontColorWhite;
        else if (currentSinkingTime < 4)
            CountDownText.color = fontColorRed;
        else
            CountDownText.color = fontColorYellow;

        // ha currentSinkingTime < 1 itt destroyolni kell a visszasz�ml�l�t (CountDownText)
    }


    void CreateCountDownUI()
    {

        // TextMeshProUGUI objektum l�trehoz�sa
        GameObject textObj = new GameObject("SinkingCountDownText");
        CountDownText = textObj.AddComponent<TextMeshProUGUI>();

        // Font �s m�s be�ll�t�sok konfigur�l�sa az Inspectorban be�ll�tott v�ltoz�k alapj�n
        CountDownText.font = font;
        CountDownText.fontSize = fontSize;
        CountDownText.color = fontColorWhite;
        CountDownText.alignment = textAlignment;

        // UI hierarchi�ban Canvas sz�l� be�ll�t�sa
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas)
        {
            CountDownText.transform.SetParent(canvas.transform);
        }

    }


}