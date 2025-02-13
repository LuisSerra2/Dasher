using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI bulletAbilityText;
    public TextMeshProUGUI nukeAbilityText;
    public TextMeshProUGUI hurricaneAbilityText;

    [Header("XP")]

    public Slider xpSlider;
    public float lerpSpeed = 5f;

    public TextMeshProUGUI levelUpText;
    public TextMeshProUGUI xpText;

    [Header("Inputs")]
    public TextMeshProUGUI ab1;
    public TextMeshProUGUI ab2;
    public TextMeshProUGUI ab3;

    [Header("EndGameMenu")]
    public GameObject endGameMenu;
    public Button retryButton;
    public Button backToMenuButton;

    [Header("Boss")]
    public GameObject bossWarningPanel;

    private void Start()
    {
        LevelUpManager.Instance.OnBossIncoming += ShowBossIncomingWarning;

        if (retryButton == null && backToMenuButton == null) return;
        retryButton.onClick.AddListener(Retry);
        backToMenuButton.onClick.AddListener(BackToMenu);
    }


    public void UpdateAbilitiesIndexText(Dictionary<string, int> abilityUses)
    {
        if (abilityUses.ContainsKey("BulletAbility"))
        {
            bulletAbilityText.text = $"{abilityUses["BulletAbility"]}";
        }

        if (abilityUses.ContainsKey("NukeAbility"))
        {
            nukeAbilityText.text = $"{abilityUses["NukeAbility"]}";
        }

        if (abilityUses.ContainsKey("HurricaneAbility"))
        {
            hurricaneAbilityText.text = $"{abilityUses["HurricaneAbility"]}";
        }
    }

    public void XpUpdate(int xp, int maxValue, int levelUpCount)
    {
        if (GameController.Instance.gameManager == GameManager.Dead) return;
        xpSlider.maxValue = maxValue;
        xpSlider.value = xp;
        levelUpText.text = levelUpCount.ToString();
        xpText.text = $"{xp} / {maxValue}";
    }

    public void UpdateInputsText(KeyCode ab1, KeyCode ab2, KeyCode ab3)
    {
        if (this.ab1 == null) return;
        this.ab1.text = ab1.ToString();
        this.ab2.text = ab2.ToString();
        this.ab3.text = ab3.ToString();
    }

    public void EndGameMenu()
    {
        endGameMenu.transform.DOScale(Vector3.one, 0.5f).OnComplete(() => StartCoroutine(PopAnim()));
    }
    private void Retry()
    {
        Scene currentSceneIndex = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(currentSceneIndex.buildIndex);
    }
    private void BackToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void ShowBossIncomingWarning()
    {
        bossWarningPanel.SetActive(true);
        StartCoroutine(HideBossWarning());
    }

    private IEnumerator HideBossWarning()
    {
        yield return new WaitForSeconds(3f);
        bossWarningPanel.SetActive(false);
    }

    IEnumerator PopAnim()
    {
        for (int i = 0; i < endGameMenu.transform.childCount; i++)
        {
            endGameMenu.transform.GetChild(i).transform.DOScale(Vector3.one, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
