using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessView : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _businessNameText;
    [SerializeField] private TextMeshProUGUI _businessLevelText;
    [SerializeField] private TextMeshProUGUI _businessIncomeText;

    [SerializeField] private TextMeshProUGUI _nextLevelButtonText;
    [SerializeField] private TextMeshProUGUI _upgrade1ButtonText;
    [SerializeField] private TextMeshProUGUI _upgrade2ButtonText;

    [SerializeField] private Button _buyLevelButton;
    [SerializeField] private Button _upgrade1Button;
    [SerializeField] private Button _upgrade2Button;

    [SerializeField] private Slider _incomeProgressSlider;


    /// <summary>
    /// Setup callbacks for purchase buttons
    /// </summary>
    public void SetupCallbacks(System.Action onBuyLevel, System.Action onBuyUpgrade1, System.Action onBuyUpgrade2) {
        if (_buyLevelButton != null && _upgrade1Button != null && _upgrade2Button != null) {
            _buyLevelButton.onClick.AddListener(() => onBuyLevel());
            _upgrade1Button.onClick.AddListener(() => onBuyUpgrade1());
            _upgrade2Button.onClick.AddListener(() => onBuyUpgrade2());
        } else {
            Debug.LogWarning("Not all buttons are assigned in the BusinessView prefab");
        }
    }

    /// <summary>
    /// Updates the business name text
    /// </summary>
    /// <param name="displayName"></param>
    public void UpdateBusinessName(string displayName) {
        if (_businessNameText != null) {
            _businessNameText.text = displayName;
        } else {
            Debug.LogWarning("_businessNameText not assigned in the BusinessView prefab");
        }
    }

    /// <summary>
    /// Updates the income progress indicator
    /// </summary>
    /// <param name="progress"></param>
    public void UpdateIncomeProgress(float progress) {
        if (_incomeProgressSlider != null) {
            _incomeProgressSlider.value = progress;
        }
    }

    /// <summary>
    /// Updates the business level text
    /// </summary>
    /// <param name="level"></param>
    public void UpdateBusinessLevel(int level) {
        if (_businessLevelText != null) {
            _businessLevelText.text = "LVL: " + level.ToString();
        }
    }

    /// <summary>
    /// Updates the business income text
    /// </summary>
    /// <param name="income"></param>
    public void UpdateBusinessIncome(float income) {
        if (_businessIncomeText != null) {
            _businessIncomeText.text = "Доход: " + income.ToString() + "$";
        }
    }

    /// <summary>
    /// Updates the next level purchase button text
    /// </summary>
    /// <param name="cost"></param>
    public void UpdateNextLevelCost(float cost) {
        if (_nextLevelButtonText != null) {
            _nextLevelButtonText.text = "LVL UP\nЦена: " + cost.ToString() + "$";
        }
    }

    /// <summary>
    /// Updates the first upgrade purchase button text
    /// </summary>
    /// <param name="cost"></param>
    public void UpdateUpgrade1(string upgradeName, float cost, float multiplier, bool ready) {
        if (_upgrade1ButtonText != null) {
            _upgrade1ButtonText.text = upgradeName + "\n" 
            + "Доход: " + ((multiplier - 1) * 100) + "%" + "\n" 
            + "Цена: " + (ready ? "Куплено" : (cost.ToString() + "$"));
        }
        if (_upgrade1Button != null) {
            _upgrade1Button.interactable = !ready;
        }
    }

    /// <summary>
    /// Updates the second upgrade purchase button text
    /// </summary>
    /// <param name="cost"></param>
    public void UpdateUpgrade2(string upgradeName, float cost, float multiplier, bool ready) {
        if (_upgrade2ButtonText != null) {
            _upgrade2ButtonText.text = upgradeName + "\n" 
            + "Доход: " + ((multiplier - 1) * 100) + "%" + "\n" 
            + "Цена: " + (ready ? "Куплено" : (cost.ToString() + "$"));
        }
        if (_upgrade2Button != null) {
            _upgrade2Button.interactable = !ready;
        }
    }
    
    
    
    



}
