using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour, IObserveHealthBarChange
{
    [SerializeField]
    private KingControler king;
    private GameObject[] heartContainers;
    private Image[] heartFills;
    
    public Transform heartsParent;
    public GameObject heartContainerPrefab;
    private const int WIDTH = 67;

    void Start()
    {
        heartContainers = new GameObject[(int)king.KingStats.MaxHealth];
        heartFills = new Image[(int)king.KingStats.MaxHealth];

        SubjectHealthBarChange.Instance.AddObserverTellHealthBarChange(OnNotifyHealthBarChange);

        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    public void OnNotifyHealthBarChange()
    {
        UpdateHeartsHUD();
    }

    public void UpdateHeartsHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }

    void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < king.KingStats.MaxHealth)
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }

    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < king.KingStats.CurrHealth)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }

        if (king.KingStats.CurrHealth % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(king.KingStats.CurrHealth);
            heartFills[lastPos].fillAmount = king.KingStats.CurrHealth % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < king.KingStats.MaxHealth; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab, heartsParent, false);
            temp.transform.localPosition += new Vector3(i * WIDTH, 0, 0);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }

}
