using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessView : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _textBusinessName;
    [SerializeField] private TextMeshProUGUI _textBusinessLevel;
    [SerializeField] private TextMeshProUGUI _textBusinessIncome;

    [SerializeField] private TextMeshProUGUI _textNextLevelCost;
    [SerializeField] private TextMeshProUGUI _textUpgrade1;
    [SerializeField] private TextMeshProUGUI _textUpgrade2;

    [SerializeField] private Button _buttonBuyLevel;
    [SerializeField] private Button _buttonUpgrade1;
    [SerializeField] private Button _buttonUpgrade2;

    [SerializeField] private Slider _sliderIncomeProgress;

    public void Init() {
        
    }

    /// <summary>
    /// Настройка коллбэков для кнопок покупки
    /// </summary>
    public void SetupCallbacks(System.Action onBuyLevel, System.Action onBuyUpgrade1, System.Action onBuyUpgrade2) {
        _buttonBuyLevel.onClick.AddListener(() => onBuyLevel());
        _buttonUpgrade1.onClick.AddListener(() => onBuyUpgrade1());
        _buttonUpgrade2.onClick.AddListener(() => onBuyUpgrade2());
    }

    /// <summary>
    /// Обновляет текст названия бизнеса
    /// </summary>
    /// <param name="displayName"></param>
    public void UpdateBusinessName(string displayName) {
        _textBusinessName.text = displayName;
    }

    /// <summary>
    /// Обновляет значение индикатора прогресса дохода
    /// </summary>
    /// <param name="progress"></param>
    public void UpdateIncomeProgress(float progress) {
        _sliderIncomeProgress.value = progress;
    }

    /// <summary>
    /// Обновляет текст уровня бизнеса
    /// </summary>
    /// <param name="level"></param>
    public void UpdateBusinessLevel(int level) {
        _textBusinessLevel.text = "LVL: " + level.ToString();
    }

    /// <summary>
    /// Обновляет текст дохода бизнеса
    /// </summary>
    /// <param name="income"></param>
    public void UpdateBusinessIncome(float income) {
        _textBusinessIncome.text = "Доход: " + income.ToString() + "$";
    }

    /// <summary>
    /// Обновляет текст кнопки покупки следующего уровня
    /// </summary>
    /// <param name="cost"></param>
    public void UpdateNextLevelCost(float cost) {
        _textNextLevelCost.text = "LVL UP\nЦена: " + cost.ToString() + "$";
    }

    /// <summary>
    /// Обновляет текст кнопки покупки первого улучшения
    /// </summary>
    /// <param name="cost"></param>
    public void UpdateUpgrade1(string upgradeName, float cost, float multiplier, bool ready) {
        _textUpgrade1.text = upgradeName + "\n" 
        + "Доход: " + ((multiplier - 1) * 100) + "%" + "\n" 
        + "Цена: " + (ready ? "Куплено" : (cost.ToString() + "$"));
        _buttonUpgrade1.interactable = !ready;
    }

    /// <summary>
    /// Обновляет текст кнопки покупки второго улучшения
    /// </summary>
    /// <param name="cost"></param>
    public void UpdateUpgrade2(string upgradeName, float cost, float multiplier, bool ready) {
        _textUpgrade2.text = upgradeName + "\n" 
        + "Доход: " + ((multiplier - 1) * 100) + "%" + "\n" 
        + "Цена: " + (ready ? "Куплено" : (cost.ToString() + "$"));
        _buttonUpgrade2.interactable = !ready;
    }
    
    
    
    



}
