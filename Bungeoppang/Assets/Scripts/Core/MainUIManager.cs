using UnityEngine;
using UnityEngine.UI;

namespace Bungeoppang.Core
{
    /// <summary>
    /// 메인 화면의 전역 UI(골드, 목표 진행도, 재료 보충 버튼)를 관리합니다.
    /// </summary>
    public class MainUIManager : MonoBehaviour
    {
        [Header("Top Bar")]
        public Text goldText;
        public Slider goalProgressSlider;
        public Text goalText;

        [Header("Bottom Bar (Refill Buttons)")]
        public Button refillBatterButton;
        public Button refillRedBeanButton;
        public Button refillCreamButton;

        private void Start()
        {
            // 버튼 이벤트 연결
            if (refillBatterButton != null) refillBatterButton.onClick.AddListener(() => Refill(10, 0, 0));
            if (refillRedBeanButton != null) refillRedBeanButton.onClick.AddListener(() => Refill(0, 5, 0));
            if (refillCreamButton != null) refillCreamButton.onClick.AddListener(() => Refill(0, 0, 5));

            UpdateUI();

            // 인벤토리 변경 시 UI 갱신 구독
            if (InventoryManager.Instance != null)
                InventoryManager.Instance.OnInventoryChanged += UpdateUI;
        }

        private void OnDestroy()
        {
            if (InventoryManager.Instance != null)
                InventoryManager.Instance.OnInventoryChanged -= UpdateUI;
        }

        private void Refill(int batter, int redBean, int cream)
        {
            if (InventoryManager.Instance != null)
            {
                // 보충 시 골드 차감 로직 (예시: 500골드)
                if (InventoryManager.Instance.currentGold >= 500)
                {
                    InventoryManager.Instance.currentGold -= 500;
                    InventoryManager.Instance.RefillIngredients(batter, redBean, cream);
                    Debug.Log("<color=cyan>재료를 보충했습니다! (-500 Gold)</color>");
                }
                else
                {
                    Debug.LogWarning("골드가 부족합니다!");
                }
            }
        }

        private void UpdateUI()
        {
            if (InventoryManager.Instance == null) return;

            if (goldText != null) goldText.text = $"{InventoryManager.Instance.currentGold} G";
            
            // 목표 진행도 (현재는 예시로 0/20 고정)
            if (goalProgressSlider != null) goalProgressSlider.value = 0.5f; 
            if (goalText != null) goalText.text = "0 / 20";
        }
    }
}
