using UnityEngine;
using TMPro; 
using System.Collections;


public class IslandController : MonoBehaviour
{

    [Header("UI Elements")]
    public TextMeshProUGUI survivorCounterText;
    [SerializeField] public Canvas targetCanvas;

    [Header("Survivor Properties")]
    public int maxSurvivors; // az Island maximális túlélõszáma, ez most a survivorSlots.Length lesz.
    public Transform survivorsParent; // objektum, ami tartalmazza a túlélõk szülõit, hogy rendezettek maradjanak a hierarchiában.
    public int currentSurvivors;
    public Slots slots; // a szigeten lévõ slotok szülõobjektuma


    [Header("Text Properties")]
    public TMP_FontAsset font;
    public Color fontColorEmpty = Color.white;
    public Color fontColorHalf = Color.white;
    public Color fontColorFull = Color.white;
    public int fontSize = 20;
    public TextAlignmentOptions textAlignment = TextAlignmentOptions.Center;

    public Vector3 textPositionOffset = new Vector3(0, -1.2f, 0);

    public int fontSizeEnd = 30; // az új font méret
    public Color fontColorFullEnd; // az új szín
    public Vector3 textPositionOffsetEnd = new Vector3(1, 2, 1);
    private LevelManager levelManager;


    void Start()
    {
       // a maximális túlélõk száma a slotok számával egyezik meg.
        maxSurvivors = slots.transform.childCount;
        CreateCounterUI();
                
        levelManager = FindObjectOfType<LevelManager>();

    }

    private void Update()
    {
        currentSurvivors = CurrentSurvivors;

        if (survivorCounterText != null)
        {
            CounterUpdate();
        }
    }


    public Transform TryAddSurvivor(Survivor survivor)

    {
        foreach (Transform slot in slots.transform)
        {
            Transform availableSlot = slots.GetAvailableSlot();
            if (availableSlot != null)

                if (survivor != null)
                {
                    SetSurvivorToSlot(survivor, availableSlot);
                    levelManager.IncrementSurvivedSurvivors();
                    break;
                }
        }
        return null; // visszatérés null-lal, ha nincs üres slot.
    }

    void SetSurvivorToSlot(Survivor survivor, Transform availableSlot)
    {
        survivor.transform.SetParent(availableSlot);
        survivor.parentObject = gameObject;
        survivor.SetTarget(availableSlot);
    }


    void CounterUpdate()
    {
        if (survivorCounterText == null) return;

        survivorCounterText.text = $"{currentSurvivors} / {maxSurvivors}";
        Vector3 islandPosition = transform.position + textPositionOffset;
        survivorCounterText.rectTransform.position = Camera.main.WorldToScreenPoint(islandPosition);

        if (currentSurvivors < 1) 
        { 
            survivorCounterText.color = fontColorEmpty;
        }
        else if (currentSurvivors >= maxSurvivors)
        { 
            survivorCounterText.color = fontColorFull;
            StartCoroutine(ChangeTextProperties(survivorCounterText, fontSize, fontSizeEnd, fontColorFull, fontColorFullEnd));
        }
        else
            survivorCounterText.color = fontColorHalf;
    }

    void CreateCounterUI()
    {
        // TextMeshProUGUI objektum létrehozása
        GameObject textObj = new GameObject("SurvivorCounterText");
        survivorCounterText = textObj.AddComponent<TextMeshProUGUI>();

        // font és más beállítások konfigurálása az Inspectorban beállított változók alapján
        survivorCounterText.font = font;
        survivorCounterText.fontSize = fontSize;
        survivorCounterText.color = fontColorEmpty;
        survivorCounterText.alignment = textAlignment;

        // a Canvas szülõ beállítása a targetCanvas változó alapján
        if (targetCanvas)
        {
            survivorCounterText.transform.SetParent(targetCanvas.transform);
        }
        else
        {
            Debug.LogWarning("target canvas nincs belõve. Inspectorba be kell állítani");
        }
    }

    void DestroyCounter()
    {
        if (survivorCounterText != null)
        {
            Destroy(survivorCounterText.gameObject);
        }
            
    }

    private IEnumerator ChangeTextProperties(TextMeshProUGUI survivorCounterText, int startSize, int endSize, Color startColor, Color endColor)
    {
        float elapsedTime = 0f;
        float duration = 2f; // 2 másodperc alatt

        Vector3 startPositionOffset = textPositionOffset;

        while (elapsedTime < duration)
        {
            if (survivorCounterText == null) yield break;

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // a teljes idõtartamhoz viszonyított eltelt idõ

            // lineáris interpoláció (lerp) a kezdeti és végsõ értékek között
            survivorCounterText.fontSize = Mathf.Lerp(startSize, endSize, t);
            survivorCounterText.color = Color.Lerp(startColor, endColor, t);
            textPositionOffset = Vector3.Lerp(startPositionOffset, textPositionOffsetEnd, t);

            Vector3 islandPosition = transform.position + textPositionOffset;
            survivorCounterText.rectTransform.position = Camera.main.WorldToScreenPoint(islandPosition);

            yield return null; // vár egy frame-et a következõ iteráció elõtt
        }

        // a végsõ értékeket beállítjuk, hogy biztosak legyünk benne, hogy a kívánt értékeket kapjuk
        survivorCounterText.fontSize = endSize;
        survivorCounterText.color = endColor;
        textPositionOffset = textPositionOffsetEnd;

        Invoke("DestroyCounter", duration+3f);
    }


    public int CurrentSurvivors
    {
        get
        {
            int count = 0;
            foreach (Transform slot in slots.transform)
            {
                if (slot.childCount > 0) count++;
            }
            return count;
        }
    }


}
