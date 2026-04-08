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
        
        [Header("Visual Auto-Scaling")]
        [Tooltip("내용물이 화면에 표시될 가로 크기 (Unity Unit)")]
        public float targetWidth = 2.5f;
        [Tooltip("내용물의 추가적인 스케일 보정값")]
        public Vector3 scaleMultiplier = Vector3.one;
        
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
            Debug.Log($"<color=cyan>[BungeoSlot] 클릭 감지됨! 현재 상태: {currentState}</color>");
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
                    contentRenderer.enabled = false;
                    break;

                case BungeoState.Batter:
                    contentRenderer.enabled = true;
                    contentRenderer.sprite = batterSprite;
                    contentRenderer.color = new Color(1f, 1f, 0.8f);
                    break;

                case BungeoState.Cooking:
                    contentRenderer.sprite = batterSprite;
                    contentRenderer.color = new Color(1f, 0.8f, 0.4f);
                    break;

                case BungeoState.Perfect:
                    contentRenderer.sprite = breadSprite;
                    contentRenderer.color = Color.white;
                    break;

                case BungeoState.Burnt:
                    contentRenderer.sprite = breadSprite;
                    contentRenderer.color = new Color(0.2f, 0.1f, 0.1f);
                    break;
            }

            // 스프라이트 크기에 상관없이 targetWidth에 맞게 localScale 자동 조정
            if (contentRenderer.enabled && contentRenderer.sprite != null)
            {
                float spriteWidth = contentRenderer.sprite.rect.width / contentRenderer.sprite.pixelsPerUnit;
                if (spriteWidth > 0)
                {
                    float calculatedScale = targetWidth / spriteWidth;
                    contentRenderer.transform.localScale = new Vector3(calculatedScale * scaleMultiplier.x, 
                                                                     calculatedScale * scaleMultiplier.y, 
                                                                     1f);
                }
            }
        }
    }
}
