using System.Threading;
using TMPro;
using UnityEngine;

public class SinkingObjectController : MonoBehaviour
{
    public int survivors = 3; // a süllyedõ objektumon lévõ túlélõk száma

    public GameObject survivorPrefab; // a Survivor prefabot az inspectorban kell hozzáadni
    public Transform survivorsParent; // szülõ amely tartalmazza a túlélõk helyét
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
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>(); // ha nem lett kézzel hozzárendelve, próbáljuk meg automatikusan lekérni.

        currentSinkingTime = sinkingTime;
        CreateCountDownUI();
    }



    public void Update()
    {
        if (!sunk)
        {
            timeElapsedSinceLastDecrease += Time.deltaTime;
            if (timeElapsedSinceLastDecrease >= 1f) // ha eltelt 1 másodperc
            {
                currentSinkingTime--; // csökkentjük a currentSinkingTime-ot
                timeElapsedSinceLastDecrease = 0f; // reseteljük az eltelt idõt
            }
            Sinking();
        }

        if (survivors <= 0 && sunk)
        {
            Destroy(gameObject);
            if (CountDownText != null)
                Destroy(CountDownText.gameObject);
        }
        if (CountDownText != null) // ezt jól van megcsinálva?
        {
            CountdownTextUpdate();
        }


    }

    private Vector3 GetRandomPosition()
    {
        // Itt kell meghatározni egy random pozíciót a SinkingObject-en belül a Survivor instanciákhoz.
        // Például:
        float x = Random.Range(-0.1f, 0.1f);
        float y = Random.Range(-0.1f, 0.1f);
        return transform.position + new Vector3(x, y, 0);
    }
    public Survivor RemoveSurvivor()
    {
        if (survivors > 0)
        {
            survivors--;
            // A túlélõ instanciáját kiveszi a listából és visszaküldi.
            Survivor survivor1 = survivorsParent.GetChild(survivors).GetComponent<Survivor>();
            Survivor survivor = survivor1;
            survivor.transform.SetParent(null); // eltávolítja a szülõtõl
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
                sunk = true; // Megjelöljük, hogy az objektum elsüllyedt.
                spriteRenderer.color = new Color(1f, 1f, 1f, 0f); // Teljesen átlátszóvá tesszük a sprite-ot.
            }
            else
            {
                float alpha = 1f - (elapsedTime / sinkingDelay); // Az idõ múlásával csökken az alpha értéke (átlátszóság).
                spriteRenderer.color = new Color(1f, 1f, 1f, alpha); // Az alpha érték frissítése.
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

        // ha currentSinkingTime < 1 itt destroyolni kell a visszaszámlálót (CountDownText)
    }


    void CreateCountDownUI()
    {

        // TextMeshProUGUI objektum létrehozása
        GameObject textObj = new GameObject("SinkingCountDownText");
        CountDownText = textObj.AddComponent<TextMeshProUGUI>();

        // Font és más beállítások konfigurálása az Inspectorban beállított változók alapján
        CountDownText.font = font;
        CountDownText.fontSize = fontSize;
        CountDownText.color = fontColorWhite;
        CountDownText.alignment = textAlignment;

        // UI hierarchiában Canvas szülõ beállítása
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas)
        {
            CountDownText.transform.SetParent(canvas.transform);
        }

    }


}