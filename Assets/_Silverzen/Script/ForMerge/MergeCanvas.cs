using DG.Tweening;
using SupersonicWisdomSDK;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MergeCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private TextMeshProUGUI diamondPriceText;
    [SerializeField] private Image startButtonImage;
    [SerializeField] private Button startButtonobject;
    [SerializeField] private Image addMoneyButtonImage;
    [SerializeField] private Button addMoneyButtonobject;
    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private RectTransform tutorialHand;
    [SerializeField] private Transform[] mergeMiddle;
    [SerializeField] private Transform handTarget;
    [SerializeField] private GameObject disableAddMoneyButton;
    Vector3 dropHandPos;
    Vector3 mergeHandPos;
    bool mergeTutorialCheck = false;
    [Header("Clases")]
    [SerializeField] private MergeManager mergeManager;
    [SerializeField] private SaveMoney saveMoney;
    [SerializeField] private AddMoney addMoney;
    void Start()
    {
        dropHandPos = Camera.main.WorldToScreenPoint(mergeMiddle[0].position);
        //mergeManager.setDiamond(2000, true);
        setDiamondText();
        addMoneyButtonUpdate(false);
    }
    private void Update()
    {
        if (mergeTutorialCheck)
        {
            foreach(Transform t in mergeMiddle)
            {
                if (t.childCount > 0)
                {
                    if (t.GetChild(0).name.Equals("2"))
                    {
                        disabelMergeTutorial();
                    }
                }
            }
        }
    }
    public void startButton()
    {
        saveMoney.allMoneySave();
        SceneManager.LoadSceneAsync("GameScene");
        SupersonicWisdom.Api.NotifyLevelStarted(PlayerPrefs.GetInt("level"), null);
    }
    public void addMoneyButton()
    {
        if (mergeManager.getDiamond() >= mergeManager.getDiamondPrice())
        {
            addMoney.addMoney(true);
            if (!PlayerPrefs.HasKey("MergeTutorial"))
            {
                PlayerPrefs.SetInt("MergeTutorial", 1);
                mergeHandPos = Camera.main.WorldToScreenPoint(handTarget.position);
                mergeTutorialCheck = true;
                enableMergeTutorial();
                //disableAddMoneyButton.SetActive(true);
            }
        }
        addMoney.checkGrid();
    }
    public void setDiamondText()
    {
        diamondText.text = mergeManager.getDiamond().ToString();
    }
    
    public void setDiamondPriceText(int price)
    {
        diamondPriceText.text = price.ToString();
    }
    public void addMoneyButtonUpdate(bool forceDisable)
    {
        if (forceDisable)
        {
            setColor(addMoneyButtonImage, .7f);
            addMoneyButtonobject.enabled = false;
            return;
        }
        if (mergeManager.getDiamond() < mergeManager.getDiamondPrice() && addMoneyButtonobject.enabled)
        {
            setColor(addMoneyButtonImage, .7f);
            addMoneyButtonobject.enabled = false;
        }
        else if(mergeManager.getDiamond() >= mergeManager.getDiamondPrice() && addMoneyButtonobject.enabled==false)
        {
            setColor(addMoneyButtonImage, 1);
            addMoneyButtonobject.enabled = true;
        }
    }
    public void startButtonUpdate(bool b)
    {
        if (b && startButtonobject.enabled == false)
        {
            setColor(startButtonImage, 1);
            startButtonobject.enabled = true;
            if (mergeTutorialCheck == false)
            {
                disableDropTurial();
            }
        }
        else if (b == false && startButtonobject.enabled)
        {
            setColor(startButtonImage, .7f);
            startButtonobject.enabled = false;
        }
    }
    void setColor(Image image,float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
    public void enableDropTutorial()
    {
        if (!tutorialPanel.activeSelf)
        {
            tutorialPanel.SetActive(true);
        }
        tutorialHand.position = dropHandPos;
        tutorialHand.DOScale(new Vector2(.9f, .9f), .5f).OnComplete(() => 
        {
            tutorialHand.DOMove(Camera.main.WorldToScreenPoint(handTarget.position), 1.3f).OnComplete(() =>
            {
                tutorialHand.DOScale(Vector2.one, .5f).OnComplete(() =>
                {
                    foreach (Transform t in mergeMiddle)
                    {
                        if (t.childCount > 0)
                        {
                            dropHandPos = Camera.main.WorldToScreenPoint(t.position);
                        }
                    }
                    enableDropTutorial();
                });
            });
        });
    }
    void enableMergeTutorial()
    {
        if (!tutorialPanel.activeSelf)
        {
            tutorialPanel.SetActive(true);
        }
        tutorialHand.position = mergeHandPos;
        tutorialHand.DOScale(new Vector2(.9f, .9f), .5f).OnComplete(() => 
        {
            tutorialHand.DOMove(dropHandPos, 1.3f).OnComplete(() => 
            {
                tutorialHand.DOScale(Vector2.one, .5f).OnComplete(() => 
                {
                    if (handTarget.childCount >  0)
                    {
                        mergeHandPos = Camera.main.WorldToScreenPoint(handTarget.position);
                        foreach (Transform t in mergeMiddle)
                        {
                            if (t.childCount > 0)
                            {
                                dropHandPos = Camera.main.WorldToScreenPoint(t.position);
                            }
                        }
                    }
                    else
                    {
                        int check = 0;
                        foreach (Transform t in mergeMiddle)
                        {
                            if (t.childCount > 0)
                            {
                                if(check == 0)
                                {
                                    mergeHandPos = Camera.main.WorldToScreenPoint(t.position);
                                    check++;
                                }
                                if(check == 1)
                                {
                                    dropHandPos = Camera.main.WorldToScreenPoint(t.position);
                                }     
                            }
                        }
                    }
                    enableMergeTutorial();
                });
            });
        });
    }
    public void disabelMergeTutorial()
    {
        if (tutorialPanel.activeSelf)
        {
            mergeTutorialCheck = false;
            tutorialHand.DOKill();
            tutorialPanel.SetActive(false);
        }
    }
    void disableDropTurial()
    {
        if (tutorialPanel.activeSelf)
        {
            tutorialHand.DOKill();
            tutorialPanel.SetActive(false);
        }
    }
    
}
