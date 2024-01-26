using UnityEngine;
using TMPro; 
using System.Collections;


public class IslandController : MonoBehaviour
{

    [Header("UI Elements")]
    public TextMeshProUGUI survivorCounterText;
    [SerializeField] public Canvas targetCanvas;

    [Header("Survivor Properties")]
    public int maxSurvivors; // az Island maxim�lis t�l�l�sz�ma, ez most a survivorSlots.Length lesz.
    public Transform survivorsParent; // objektum, ami tartalmazza a t�l�l�k sz�l�it, hogy rendezettek maradjanak a hierarchi�ban.
    public int currentSurvivors;
    public Slots slots; // a szigeten l�v� slotok sz�l�objektuma


    [Header("Text Properties")]
    public TMP_FontAsset font;
    public Color fontColorEmpty = Color.white;
    public Color fontColorHalf = Color.white;
    public Color fontColorFull = Color.white;
    public int fontSize = 20;
    public TextAlignmentOptions textAlignment = TextAlignmentOptions.Center;

    public Vector3 textPositionOffset = new Vector3(0, -1.2f, 0);

    public int fontSizeEnd = 30; // az �j font m�ret
    public Color fontColorFullEnd; // az �j sz�n
    public Vector3 textPositionOffsetEnd = new Vector3(1, 2, 1);
    private LevelManager levelManager;


    void Start()
    {
       // a maxim�lis t�l�l�k sz�ma a slotok sz�m�val egyezik meg.
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
        return null; // visszat�r�s null-lal, ha nincs �res slot.
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
        // TextMeshProUGUI objektum l�trehoz�sa
        GameObject textObj = new GameObject("SurvivorCounterText");
        survivorCounterText = textObj.AddComponent<TextMeshProUGUI>();

        // font �s m�s be�ll�t�sok konfigur�l�sa az Inspectorban be�ll�tott v�ltoz�k alapj�n
        survivorCounterText.font = font;
        survivorCounterText.fontSize = fontSize;
        survivorCounterText.color = fontColorEmpty;
        survivorCounterText.alignment = textAlignment;

        // a Canvas sz�l� be�ll�t�sa a targetCanvas v�ltoz� alapj�n
        if (targetCanvas)
        {
            survivorCounterText.transform.SetParent(targetCanvas.transform);
        }
        else
        {
            Debug.LogWarning("target canvas nincs bel�ve. Inspectorba be kell �ll�tani");
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
        float duration = 2f; // 2 m�sodperc alatt

        Vector3 startPositionOffset = textPositionOffset;

        while (elapsedTime < duration)
        {
            if (survivorCounterText == null) yield break;

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // a teljes id�tartamhoz viszony�tott eltelt id�

            // line�ris interpol�ci� (lerp) a kezdeti �s v�gs� �rt�kek k�z�tt
            survivorCounterText.fontSize = Mathf.Lerp(startSize, endSize, t);
            survivorCounterText.color = Color.Lerp(startColor, endColor, t);
            textPositionOffset = Vector3.Lerp(startPositionOffset, textPositionOffsetEnd, t);

            Vector3 islandPosition = transform.position + textPositionOffset;
            survivorCounterText.rectTransform.position = Camera.main.WorldToScreenPoint(islandPosition);

            yield return null; // v�r egy frame-et a k�vetkez� iter�ci� el�tt
        }

        // a v�gs� �rt�keket be�ll�tjuk, hogy biztosak legy�nk benne, hogy a k�v�nt �rt�keket kapjuk
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
