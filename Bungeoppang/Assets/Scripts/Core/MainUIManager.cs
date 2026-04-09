using UnityEngine;
using UnityEngine.UI;

namespace Bungeoppang.Core
{
    /// <summary>
    /// 메인 화면의 전역 UI(골드, 목표 진행도, 재료 보충 버튼, 트레이 재고)를 관리합니다.
    /// </summary>
    public class MainUIManager : MonoBehaviour
    {
        [Header("Top Bar")]
        public Text goldText;
        public Slider goalProgressSlider;
        public Text goalText;

        [Header("Tray UI (Completed Products)")]
        public Text trayRedBeanText;
        public Text trayCreamText;

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
            
            // 트레이 재고 업데이트
            if (trayRedBeanText != null) trayRedBeanText.text = $"x {InventoryManager.Instance.redBeanBungeoCount}";
            if (trayCreamText != null) trayCreamText.text = $"x {InventoryManager.Instance.creamBungeoCount}";

            // 목표 진행도 (팥 붕어빵 10개 목표 예시)
            if (goalProgressSlider != null)
            {
                float progress = Mathf.Clamp01((float)InventoryManager.Instance.redBeanBungeoCount / 10f);
                goalProgressSlider.value = progress;
            }
            if (goalText != null)
            {
                goalText.text = $"{InventoryManager.Instance.redBeanBungeoCount} / 10";
            }
        }
    }
}
