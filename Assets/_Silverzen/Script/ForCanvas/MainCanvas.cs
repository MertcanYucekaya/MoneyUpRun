using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.Tools;
using SupersonicWisdomSDK;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameFailedPanel;
    [SerializeField] private Image glowEffectImage;
    [Header("Diamond")]
    [SerializeField] public Transform diamondPanel;
    [SerializeField] private Image diamondPanelImage;
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private GameObject diamondImage;
    [SerializeField] private Transform diamondImageTarget;
    [SerializeField] private TextMeshProUGUI gettinDiamondText;
    [SerializeField] private string diamondImageText;
    [SerializeField] private TextMeshProUGUI currentLevelText;

    private void Start()
    {
        DOTween.Init();
        setDiamondText();
    }
    public void setCurrentLevelText(int level)
    {
        currentLevelText.text = "Level " + level;
    }
    public void setDiamondText()
    {
        diamondText.text = gameManager.getDiamond().ToString();
    }
    public void activateGameOverPanel(int currentDiamond)
    {
        gameOverPanel.SetActive(true);
        gettinDiamondText.text = currentDiamond.ToString()+ diamondImageText;
        SupersonicWisdom.Api.NotifyLevelCompleted(PlayerPrefs.GetInt("level"), null);
    }
    public void activateGameFailedPanel()
    {
        gameFailedPanel.SetActive(true);
    }
    public void instantDiamondImage(Transform instantDiamondTransform,int diamondAmount)
    {
        GameObject g = Instantiate(diamondImage, Camera.main.WorldToScreenPoint(instantDiamondTransform.position), Quaternion.identity);
        g.transform.SetParent(diamondPanel);
        g.transform.DOScale(diamondImageTarget.localScale, 1);
        g.transform.DOMove(diamondImageTarget.position, 1).OnComplete(() =>
        {
            gameManager.setDiamond(diamondAmount, true);
            Destroy(g);
        });
    }
    public void glowEffect()
    {
        glowEffectImage.gameObject.SetActive(true);
        glowEffectImage.DOFade(.6f, .3f)
            .OnComplete(() => 
            {
                glowEffectImage.DOFade(0, .3f)
                .OnComplete(() =>
                {
                    glowEffectImage.gameObject.SetActive(false);
                });
            });
    }
    public void diamondPanelGlow()
    {
        diamondPanelImage.DOColor(Color.red, .5f).OnComplete(() => 
        {
            diamondPanelImage.DOColor(Color.white, .5f);
        });
    }
}
