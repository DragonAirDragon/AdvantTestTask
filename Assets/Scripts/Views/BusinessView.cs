using UnityEngine;
using UnityEngine.UI;

public class BusinessView : MonoBehaviour {

    [SerializeField] private Text _businessNameText;
    [SerializeField] private Text _businessLevelText;
    [SerializeField] private Text _businessIncomeText;

    [SerializeField] private Text _nextLevelButtonText;
    [SerializeField] private Text _upgrade1ButtonText;
    [SerializeField] private Text _upgrade2ButtonText;

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
    /// Updates the income progress indicator only if value changed
    /// </summary>
    /// <param name="progress"></param>
    public void UpdateIncomeProgress(float progress) {
        if (_incomeProgressSlider != null) {
            // Проверяем, отличается ли новое значение от текущего
            if (Mathf.Abs(_incomeProgressSlider.value - progress) > 0.001f) {
                _incomeProgressSlider.value = progress;
            }
        }
    }

    /// <summary>
    /// Updates the business level text only if changed
    /// </summary>
    /// <param name="level"></param>
    public void UpdateBusinessLevel(int level) {
        if (_businessLevelText != null) {
            string newText = "LVL:\n" + level.ToString();
            if (_businessLevelText.text != newText) {
                _businessLevelText.text = newText;
            }
        }
    }

    /// <summary>
    /// Updates the business income text only if changed
    /// </summary>
    /// <param name="income"></param>
    public void UpdateBusinessIncome(float income) {
        if (_businessIncomeText != null) {
            string newText = "Доход:\n" + income.ToString() + "$";
            if (_businessIncomeText.text != newText) {
                _businessIncomeText.text = newText;
            }
        }
    }

    /// <summary>
    /// Updates the next level purchase button text only if changed
    /// </summary>
    /// <param name="cost"></param>
    public void UpdateNextLevelCost(float cost) {
        if (_nextLevelButtonText != null) {
            string newText = "LVL UP\nЦена: " + cost.ToString() + "$";
            if (_nextLevelButtonText.text != newText) {
                _nextLevelButtonText.text = newText;
            }
        }
    }

    /// <summary>
    /// Updates the first upgrade button only if changed
    /// </summary>
    public void UpdateUpgrade1(string upgradeName, float cost, float multiplier, bool ready) {
        if (_upgrade1ButtonText != null) {
            string newText = upgradeName + "\n" 
                + "Доход: " + ((multiplier - 1) * 100) + "%" + "\n" 
                + "Цена: " + (ready ? "Куплено" : (cost.ToString() + "$"));
            
            if (_upgrade1ButtonText.text != newText) {
                _upgrade1ButtonText.text = newText;
            }
        }
        
        if (_upgrade1Button != null && _upgrade1Button.interactable == ready) {
            _upgrade1Button.interactable = !ready;
        }
    }

    /// <summary>
    /// Updates the second upgrade button only if changed
    /// </summary>
    public void UpdateUpgrade2(string upgradeName, float cost, float multiplier, bool ready) {
        if (_upgrade2ButtonText != null) {
            string newText = upgradeName + "\n" 
                + "Доход: " + ((multiplier - 1) * 100) + "%" + "\n" 
                + "Цена: " + (ready ? "Куплено" : (cost.ToString() + "$"));
            
            if (_upgrade2ButtonText.text != newText) {
                _upgrade2ButtonText.text = newText;
            }
        }
        
        if (_upgrade2Button != null && _upgrade2Button.interactable == ready) {
            _upgrade2Button.interactable = !ready;
        }
    }
    
    
    
    



}
