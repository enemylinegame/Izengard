using TMPro;
using UnityEngine;
using ResourceSystem;

public class TopResUiVew : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI woodCount;    
    [SerializeField]
    private TextMeshProUGUI ironCount;
    [SerializeField]
    private TextMeshProUGUI deercount;
    [SerializeField]
    private TextMeshProUGUI horsecount;
    [SerializeField]
    private TextMeshProUGUI textilecount;
    [SerializeField]
    private TextMeshProUGUI steelcount;
    [SerializeField]
    private TextMeshProUGUI magikStonescount;
    [SerializeField]
    private TextMeshProUGUI workerCount;
    [SerializeField]
    private TextMeshProUGUI goldCount;


    public void UpdateResursesCount(ResourceType type, float count)
    {
        switch(type)
        {
            case ResourceType.Wood:
                woodCount.text = $"{count}";
                break;
            case ResourceType.Iron:
                ironCount.text = $"{count}";
                break;
            case ResourceType.Deer:
                deercount.text = $"{count}";
                break;
            case ResourceType.Horse:
                horsecount.text = $"{count}";
                break;
            case ResourceType.Textile:
                textilecount.text = $"{count}";
                break;
            case ResourceType.Steele:
                steelcount.text = $"{count}";
                break;
            case ResourceType.MagikStones:
                magikStonescount.text = $"{count}";
                break;
            case ResourceType.Gold:
                goldCount.text = $"{count}";
                break;
        }
    }
    public void UpdatePeopleCount(int maxValue)
    {
        workerCount.text = $"{maxValue}";
    }
}
