using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Bungeoppang.Core
{
    /// <summary>
    /// 붕어빵 틀과 내용물(반죽/소/빵) 이미지를 교체하며 굽기 상태를 시각화합니다.
    /// </summary>
    public class BungeoSlot : MonoBehaviour, IPointerClickHandler
    {
        // 정적 변수로 현재 어떤 소가 선택되었는지 관리 (버튼에서 설정)
        public static BungeoFilling selectedFilling = BungeoFilling.RedBean;

        [Header("Settings")]
        public float cookingTime = 5f; 
        public float perfectDuration = 2f; 
        
        [Header("Sprite References")]
        [Tooltip("붕어빵 틀 이미지 (고정)")]
        public SpriteRenderer moldRenderer;
        [Tooltip("반죽/빵 이미지 렌더러")]
        public SpriteRenderer contentRenderer;
        [Tooltip("소(팥/슈크림) 이미지 렌더러")]
        public SpriteRenderer fillingRenderer;
        
        [Header("Visual Auto-Scaling")]
        [Tooltip("반죽/빵이 화면에 표시될 가로 크기 (Unity Unit)")]
        public float targetWidth = 2.5f;
        [Tooltip("소가 화면에 표시될 가로 크기 (Unity Unit)")]
        public float fillingTargetWidth = 1.2f;
        [Tooltip("내용물의 추가적인 스케일 보정값")]
        public Vector3 scaleMultiplier = Vector3.one;
        
        [Space]
        public Sprite batterSprite;   // 반죽 이미지
        public Sprite breadSprite;    // 완성된 빵 이미지
        public Sprite redBeanSprite;  // 팥 이미지
        public Sprite creamSprite;    // 슈크림 이미지

        [Header("UI Reference")]
        public Slider gaugeSlider;

        [Header("Runtime Status")]
        public BungeoState currentState = BungeoState.Empty;
        public BungeoFilling currentFilling = BungeoFilling.None;
        
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
            // Cooking 이후 단계에서만 타이머 작동
            if (currentState != BungeoState.Cooking && 
                currentState != BungeoState.Perfect && 
                currentState != BungeoState.Burnt) return;

            timer += Time.deltaTime;

            if (gaugeSlider != null)
            {
                float progress = Mathf.Clamp01(timer / cookingTime);
                gaugeSlider.value = progress;
                
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
            Debug.Log($"<color=cyan>[BungeoSlot] 클릭됨! 상태: {currentState}, 선택된 소: {selectedFilling}</color>");
            
            switch (currentState)
            {
                case BungeoState.Empty:
                    StartCooking(); // 1차 반죽
                    break;
                case BungeoState.Batter:
                    AddFilling();   // 소 넣기
                    break;
                case BungeoState.Filling:
                    CoverWithBatter(); // 2차 반죽 (덮기)
                    break;
                case BungeoState.Perfect:
                case BungeoState.Burnt:
                    Harvest();      // 수확
                    break;
            }
        }

        private void StartCooking()
        {
            timer = 0f;
            currentFilling = BungeoFilling.None;
            TransitionTo(BungeoState.Batter);
            Debug.Log("<color=white>○ [1단계] 하단 반죽을 부었습니다!</color>");
        }

        private void AddFilling()
        {
            currentFilling = selectedFilling;
            TransitionTo(BungeoState.Filling);
            Debug.Log($"<color=orange>● [2단계] {currentFilling} 소를 넣었습니다!</color>");
        }

        private void CoverWithBatter()
        {
            TransitionTo(BungeoState.Covering);
            Debug.Log("<color=white>○ [3단계] 상단 반죽으로 덮었습니다! 굽기 시작.</color>");
            
            // 덮으면 잠시 후 바로 굽기 시작
            Invoke(nameof(StartBaking), 0.5f);
        }

        private void StartBaking()
        {
            TransitionTo(BungeoState.Cooking);
            if (gaugeSlider != null) gaugeSlider.gameObject.SetActive(true);
        }

        private void Harvest()
        {
            string fillingName = currentFilling == BungeoFilling.RedBean ? "팥" : "슈크림";
            Debug.Log(currentState == BungeoState.Perfect ? 
                $"<color=yellow>★ 완벽한 {fillingName} 붕어빵 수확! ★</color>" : 
                $"<color=red>✖ 실패(탄 {fillingName} 붕어빵) ✖</color>");
            
            TransitionTo(BungeoState.Empty);
            if (gaugeSlider != null) gaugeSlider.gameObject.SetActive(false);
            timer = 0f;
            currentFilling = BungeoFilling.None;
        }

        private void TransitionTo(BungeoState newState)
        {
            currentState = newState;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (contentRenderer == null) return;

            // 기본 비활성화
            if (fillingRenderer != null) fillingRenderer.enabled = false;

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

                case BungeoState.Filling:
                    contentRenderer.enabled = true;
                    contentRenderer.sprite = batterSprite;
                    contentRenderer.color = new Color(1f, 1f, 0.8f);
                    ShowFilling();
                    break;

                case BungeoState.Covering:
                    contentRenderer.enabled = true;
                    contentRenderer.sprite = batterSprite;
                    contentRenderer.color = new Color(1f, 1f, 0.8f);
                    // 소를 가림 (반죽으로 덮였으므로)
                    break;

                case BungeoState.Cooking:
                    contentRenderer.enabled = true;
                    contentRenderer.sprite = batterSprite;
                    contentRenderer.color = new Color(1f, 0.8f, 0.4f);
                    break;

                case BungeoState.Perfect:
                    contentRenderer.enabled = true;
                    contentRenderer.sprite = breadSprite;
                    contentRenderer.color = Color.white;
                    break;

                case BungeoState.Burnt:
                    contentRenderer.enabled = true;
                    contentRenderer.sprite = breadSprite;
                    contentRenderer.color = new Color(0.2f, 0.1f, 0.1f);
                    break;
            }

            AutoFixScale(contentRenderer, targetWidth);
            if (fillingRenderer != null && fillingRenderer.enabled) 
                AutoFixScale(fillingRenderer, fillingTargetWidth);
        }

        private void ShowFilling()
        {
            if (fillingRenderer == null) return;
            
            fillingRenderer.enabled = true;
            if (currentFilling == BungeoFilling.RedBean) fillingRenderer.sprite = redBeanSprite;
            else if (currentFilling == BungeoFilling.Cream) fillingRenderer.sprite = creamSprite;
            else fillingRenderer.enabled = false;
        }

        private void AutoFixScale(SpriteRenderer renderer, float targetSize)
        {
            if (renderer == null || renderer.sprite == null) return;
            
            float spriteWidth = renderer.sprite.rect.width / renderer.sprite.pixelsPerUnit;
            if (spriteWidth > 0)
            {
                float calculatedScale = targetSize / spriteWidth;
                renderer.transform.localScale = new Vector3(calculatedScale * scaleMultiplier.x, 
                                                           calculatedScale * scaleMultiplier.y, 
                                                           1f);
            }
        }

        // 버튼에서 호출할 메서드 (static 변수를 수정하여 모든 슬롯이 공유)
        public void SelectFilling(int fillingIndex)
        {
            selectedFilling = (BungeoFilling)fillingIndex;
            Debug.Log($"<color=magenta>[BungeoSlot] 소 선택 변경: {selectedFilling}</color>");
        }
    }
}
