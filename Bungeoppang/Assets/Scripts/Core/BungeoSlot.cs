using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Bungeoppang.Core
{
    /// <summary>
    /// 붕어빵 틀과 내용물(반죽/빵) 이미지를 교체하며 굽기 상태를 시각화합니다.
    /// </summary>
    public class BungeoSlot : MonoBehaviour, IPointerClickHandler
    {
        [Header("Settings")]
        public float cookingTime = 5f; 
        public float perfectDuration = 2f; 
        
        [Header("Sprite References")]
        [Tooltip("붕어빵 틀 이미지 (고정)")]
        public SpriteRenderer moldRenderer;
        [Tooltip("내용물(반죽/빵) 이미지 (변화)")]
        public SpriteRenderer contentRenderer;
        
        [Space]
        public Sprite batterSprite; // 반죽 이미지
        public Sprite breadSprite;  // 완성된 빵 이미지

        [Header("UI Reference")]
        public Slider gaugeSlider;

        [Header("Runtime Status")]
        public BungeoState currentState = BungeoState.Empty;
        
        private float timer = 0f;

        private void Awake()
        {
            if (gaugeSlider != null) 
            {
                gaugeSlider.gameObject.SetActive(false);
                gaugeSlider.minValue = 0f;
                gaugeSlider.maxValue = 1f;
            }
            UpdateVisual();
        }

        private void Update()
        {
            if (currentState == BungeoState.Empty) return;

            timer += Time.deltaTime;

            if (gaugeSlider != null)
            {
                float progress = Mathf.Clamp01(timer / cookingTime);
                gaugeSlider.value = progress;
                
                // 상태별 게이지 색상
                if (currentState == BungeoState.Perfect) 
                    gaugeSlider.fillRect.GetComponent<Image>().color = Color.green;
                else if (currentState == BungeoState.Burnt) 
                    gaugeSlider.fillRect.GetComponent<Image>().color = Color.red;
                else 
                    gaugeSlider.fillRect.GetComponent<Image>().color = Color.yellow;
            }

            // FSM 기반 상태 전이
            switch (currentState)
            {
                case BungeoState.Batter:
                    if (timer >= cookingTime * 0.4f) TransitionTo(BungeoState.Cooking);
                    break;
                case BungeoState.Cooking:
                    if (timer >= cookingTime) TransitionTo(BungeoState.Perfect);
                    break;
                case BungeoState.Perfect:
                    if (timer >= cookingTime + perfectDuration) TransitionTo(BungeoState.Burnt);
                    break;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (currentState == BungeoState.Empty) StartCooking();
            else if (currentState == BungeoState.Perfect || currentState == BungeoState.Burnt) Harvest();
        }

        private void StartCooking()
        {
            timer = 0f;
            TransitionTo(BungeoState.Batter);
            if (gaugeSlider != null) gaugeSlider.gameObject.SetActive(true);
            Debug.Log("<color=white>○ 반죽을 부었습니다!</color>");
        }

        private void Harvest()
        {
            Debug.Log(currentState == BungeoState.Perfect ? "<color=yellow>★ 완벽 수확! ★</color>" : "<color=red>✖ 실패(탄 빵) ✖</color>");
            TransitionTo(BungeoState.Empty);
            if (gaugeSlider != null) gaugeSlider.gameObject.SetActive(false);
            timer = 0f;
        }

        private void TransitionTo(BungeoState newState)
        {
            currentState = newState;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (contentRenderer == null) return;

            switch (currentState)
            {
                case BungeoState.Empty:
                    contentRenderer.enabled = false; // 빈 틀일 땐 내용물 숨김
                    break;

                case BungeoState.Batter:
                    contentRenderer.enabled = true;
                    contentRenderer.sprite = batterSprite;
                    contentRenderer.color = new Color(1f, 1f, 0.8f); // 흰색 반죽
                    break;

                case BungeoState.Cooking:
                    contentRenderer.sprite = batterSprite;
                    contentRenderer.color = new Color(1f, 0.8f, 0.4f); // 익어가는 노란색
                    break;

                case BungeoState.Perfect:
                    contentRenderer.sprite = breadSprite; // 이미지 교체: 빵
                    contentRenderer.color = Color.white;  // 빵 원본 색상
                    break;

                case BungeoState.Burnt:
                    contentRenderer.sprite = breadSprite;
                    contentRenderer.color = new Color(0.2f, 0.1f, 0.1f); // 탄 색상
                    break;
            }
        }
    }
}
