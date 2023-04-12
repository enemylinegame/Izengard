using ResurseSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsUI : MonoBehaviour
{
    [Header("Building Space Face")]

    [SerializeField]
    private Image BuildingIconHolder;
    [SerializeField]
    private TextMeshProUGUI BuildingNameHolder;
    [SerializeField]
    private TextMeshProUGUI _HPValuetxt;
    [SerializeField]
    private Slider _HPBarSlider;
    
    #region Поля пространства UI стока здания

    [Header("Building Stock Space Parameters")]

    [SerializeField]
    private GameObject StockBuildSpace;
    [SerializeField]
    private GameObject goldHolder;
    [SerializeField]
    private Image goldIcon;
    [SerializeField]
    private TextMeshProUGUI goldTextHolder;

    [SerializeField]
    private GameObject woodHolder;
    [SerializeField]
    private Image woodIcon;
    [SerializeField]
    private TextMeshProUGUI woodTextholder;

    [SerializeField]
    private GameObject ironholder;
    [SerializeField]
    private Image ironIcon;
    [SerializeField]
    private TextMeshProUGUI ironTextholder;

    [SerializeField]
    private GameObject deersholder;
    [SerializeField]
    private Image deersIcon;
    [SerializeField]
    private TextMeshProUGUI deersTextholder;

    [SerializeField]
    private GameObject horseholder;
    [SerializeField]
    private Image horseIcon;
    [SerializeField]
    private TextMeshProUGUI horseTextholder;

    [SerializeField]
    private GameObject textileholder;
    [SerializeField]
    private Image textileIcon;
    [SerializeField]
    private TextMeshProUGUI textileTextholder;

    [SerializeField]
    private GameObject steelholder;
    [SerializeField]
    private Image steelIcon;
    [SerializeField]
    private TextMeshProUGUI steelTextholder;

    [SerializeField]
    private GameObject magikstonesholder;
    [SerializeField]
    private Image magikStonesIcon;
    [SerializeField]
    private TextMeshProUGUI magikStoneTextholder;
    #endregion
    #region Поля пространства UI производства здания

    [Header("Building Produce Space Parameters")]

    [SerializeField]
    private GameObject ProduceBuildingSpace;
    [SerializeField]
    private TextMeshProUGUI ProduceTitle;
    [SerializeField]
    public Button StartProduceButton;
    [SerializeField]
    public Button CancelProduceButton;
    [SerializeField]
    public Toggle AutoProduceToggle;
    [SerializeField]
    public Slider ProduceValueSlider;
    [SerializeField]
    private TextMeshProUGUI produceValueSliderText;
    [SerializeField]
    private List<ProduceItemButtonView> ProduceButtons;

    #endregion
    #region LoadSpace UI
    [Header("Building Load Space Parameters")]

    [SerializeField]
    private List<ProduceItemButtonView> _loadProduceButtons;

    [SerializeField]
    private GameObject _LoadSpace;
    [SerializeField]
    private Slider _loadSlider;
    #endregion
    /// <summary>
    /// Set Active All BuildingUI
    /// </summary>
    /// <param name="value"></param>
    public void SetActivBuildingUI(bool value)
    {
        gameObject.SetActive(value);
    }
    /// <summary>
    /// Set value for HP text building
    /// </summary>
    /// <param name="text"></param>
    public void SetHPValueTxt(string text)
    {
        _HPValuetxt.text = text;
    }
    /// <summary>
    /// Set hp value for HP slider building UI
    /// </summary>
    /// <param name="maxValue"></param>
    /// <param name="currentValue"></param>
    public void SetHPSliderValue(float maxValue,float currentValue)
    {
        _HPBarSlider.maxValue = maxValue;
        _HPBarSlider.value = currentValue;
    }
    /// <summary>
    /// Change value of HP slider building UI
    /// </summary>
    /// <param name="value"></param>
    public void ChangeHPSliderValue(float value)
    {
        _HPBarSlider.value = value;
    }
    /// <summary>
    /// Set active hp bar building
    /// </summary>
    /// <param name="value"></param>
    public void SetActiveHPSpace(bool value)
    {
        _HPBarSlider.gameObject.SetActive(value);
    }
    #region Stock UI Metods
    public void SetGoldValue(Sprite icon, float currvalue, float maxvalue)
    {
        goldIcon.sprite = icon;
        goldTextHolder.text = $"{currvalue} / {maxvalue}";
        goldHolder.SetActive(true);
    }
    public void SetWoodValue(Sprite icon, float currvalue, float maxvalue)
    {
        woodIcon.sprite = icon;
        woodTextholder.text = $"{currvalue} / {maxvalue}";
        woodHolder.SetActive(true);
    }
    public void SetIronValue(Sprite icon, float currvalue, float maxvalue)
    {
        ironIcon.sprite = icon;
        ironTextholder.text = $"{currvalue} / {maxvalue}";
        ironholder.SetActive(true);
    }
    public void SetDeersValue(Sprite icon, float currvalue, float maxvalue)
    {
        deersIcon.sprite = icon;
        deersTextholder.text = $"{currvalue} / {maxvalue}";
        deersholder.SetActive(true);
    }
    public void SetHorseValue(Sprite icon, float currvalue, float maxvalue)
    {
        horseIcon.sprite = icon;
        horseTextholder.text = $"{currvalue} / {maxvalue}";
        horseholder.SetActive(true);
    }
    public void SetSteelValue(Sprite icon, float currvalue, float maxvalue)
    {
        steelIcon.sprite = icon;
        steelTextholder.text = $"{currvalue} / {maxvalue}";
        steelholder.SetActive(true);
    }
    public void SetMagikStonesValue(Sprite icon, float currvalue, float maxvalue)
    {
        magikStonesIcon.sprite = icon;
        magikStoneTextholder.text = $"{currvalue} / {maxvalue}";
        magikstonesholder.SetActive(true);
    }
    public void SetTextileValue(Sprite icon, float currvalue, float maxvalue)
    {
        textileIcon.sprite = icon;
        textileTextholder.text = $"{currvalue} / {maxvalue}";
        textileholder.SetActive(true);
    }
    public void SetActiveStockSpace(bool value)
    {
        StockBuildSpace.SetActive(value);
    }
    public void SetBuildingFace(Sprite icon,string name)
    {
        BuildingIconHolder.sprite = icon;
        BuildingNameHolder.text = name; 
    }
    public void DisableAllStockHolders()
    {
        textileholder.SetActive(false);
        magikstonesholder.SetActive(false);
        steelholder.SetActive(false);
        horseholder.SetActive(false);
        deersholder.SetActive(false);
        ironholder.SetActive(false);
        woodHolder.SetActive(false);
        goldHolder.SetActive(false);
    }
    #endregion
    #region Produce UI Metods
    public int GetProduceSliderValue()
    {
        return ((int)(ProduceValueSlider.value));
    }    
    public void SetActiveProduceUI(bool value)
    {
        ProduceBuildingSpace.SetActive(value);
    }
    public List<ProduceItemButtonView> GetListProduceButtons()
    {
        return ProduceButtons;
    }
    public void DisableProduceButtons()
    {
        foreach(ProduceItemButtonView view in ProduceButtons)
        {
            view.SetActiveButton(false);
        }
    }
    public void SetProduceCostTitle(string txt)
    {
        ProduceTitle.text = "Produced Cost:\n"+txt;
    }
    public void SetProduceValueSliderText(float value)
    {
        produceValueSliderText.text = "produce value: " + value.ToString();
    }
    #endregion
    #region Load UI Metods
    public void SetActiveLoadUI(bool value)
    {
        _LoadSpace.SetActive(value);
    }
    public void SetActiveLoadUIValue(float buildTime)
    {
        _LoadSpace.SetActive(true);
        _loadSlider.value = buildTime;
    }
    public void SetLoadSliderValue (float value)
    {
        _loadSlider.value = value;
    }
    public void SetMaxLoadSliderValue(float MaxSliderValue)
    {
        _loadSlider.maxValue = MaxSliderValue;
    }
    public void SetProduceItemButtons(List<ItemProduct> products)
    {
        if (products.Count<_loadProduceButtons.Count && products!=null)
        {
            for (int i=0;i< products.Count;i++)
            {
                _loadProduceButtons[0].SetButtonView(products[0]);
            }
        }
    }
    public void SetProduceItemButtons(List<ResurseProduct> products)
    {
        if (products.Count < _loadProduceButtons.Count && products != null)
        {
            for (int i = 0; i < products.Count; i++)
            {
                _loadProduceButtons[0].SetButtonView(products[0]);
            }
        }
    }
    public void DisableAllProduceButtons()
    {
        foreach (ProduceItemButtonView button in _loadProduceButtons)
        {
            button.SetActiveButton(false);
        }
    }
    #endregion
    private void Awake()
    {
        gameObject.SetActive(false);
        SetActiveProduceUI(false);
        SetActiveLoadUI(false);
        SetActiveStockSpace(false);
        ProduceValueSlider.onValueChanged.AddListener(SetProduceValueSliderText);
    }


}