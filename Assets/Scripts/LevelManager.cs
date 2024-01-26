using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public enum SurvivorState { Basic, Swimming, Escaped, Survived, Dead }
    public int AllSurvivors;
    public int Survived;
    public int Dead;
    public GameObject EfficiencyScreen;
    public Button ContinueButton;
    public Button TryAgainButton;
    public string NextSceneName;
    public int RequiredSilver;
    public int RequiredBronze;

    public GameObject fail;
    public GameObject bronze;
    public GameObject silver;
    public GameObject gold;

    private void Start()
    {
        EfficiencyScreen.SetActive(false);
    }

    private void Update()
    {
        // Ellenõrizzük, hogy vége van-e a menetnek
        if (Survived + Dead >= AllSurvivors && AllSurvivors != 0)
        {
            StartCoroutine(EvaluatePerformanceCoroutine());
        }
    }


    private IEnumerator EvaluatePerformanceCoroutine()
    {
        yield return new WaitForSeconds(3f);
        EvaluatePerformance();
    }

    public void SurvivorEscaped()
    {
        Survived++;
    }

    public void SurvivorDied()
    {
        Dead++;
    }

    private void EvaluatePerformance()
    {
        string performance = "Failed";

        if (Survived == AllSurvivors)
        {
            performance = "Gold";
        }
        else if (Survived >= RequiredSilver)
        {
            performance = "Silver";
        }
        else if (Survived >= RequiredBronze)
        {
            performance = "Bronze";
        }

        EndScene(performance);
    }

    private void EndScene(string performance)
    {
        EfficiencyScreen.SetActive(true);

        Time.timeScale = 0f; // megállítja az idõt
        if (performance == "Failed")
        {
            ContinueButton.gameObject.SetActive(false);
            TryAgainButton.gameObject.SetActive(true);
            fail.gameObject.SetActive(true);
        }
        else if (performance == "Gold")
        {
            ContinueButton.gameObject.SetActive(true);
            TryAgainButton.gameObject.SetActive(false);
            gold.gameObject.SetActive(true);
        }
        else if (performance == "Bronze")
        {
            ContinueButton.gameObject.SetActive(true);
            TryAgainButton.gameObject.SetActive(true);
            bronze.gameObject.SetActive(true);
        }
        else
        {
            ContinueButton.gameObject.SetActive(true);
            TryAgainButton.gameObject.SetActive(true);
            silver.gameObject.SetActive(true);
        }

    }

    public void OnTryAgain()
    {
        // újra betölti a jelenlegi jelenetet
        Time.timeScale = 1f;
        SetFalse();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnContinue()
    {
        // betölti a következõ jelenetet
        Time.timeScale = 1f;
        SetFalse();
        SceneManager.LoadScene(NextSceneName);
    }

    public void SetFalse() 
    {
        fail.gameObject.SetActive(false);
        bronze.gameObject.SetActive(false);
        silver.gameObject.SetActive(false);
        gold.gameObject.SetActive(false);
    }

    public void IncrementAllSurvivors()
    {
        AllSurvivors++;
    }

    public void IncrementDeadSurvivors()
    {
        Dead++;
    }

    public void IncrementSurvivedSurvivors()
    {
        Survived++;
    }
}