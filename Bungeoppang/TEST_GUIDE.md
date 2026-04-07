# 🐟 붕어빵 굽기 시스템 테스트 가이드 (Unity 6)

본 문서는 Phase 1에서 구현된 핵심 굽기 로직(FSM)을 유니티 에디터에서 시각적으로 테스트하는 방법을 설명합니다.

---

## 🛠️ 1. 유니티 프로젝트 초기 설정 (필수)

Unity 6 및 New Input System 환경에서 클릭(터치)을 정확히 감지하기 위해 다음 설정이 필요합니다.

1.  **Main Camera 설정**:
    *   `Hierarchy` 창에서 `Main Camera`를 선택합니다.
    *   `Inspector` 창 하단의 `Add Component`를 클릭합니다.
    *   **`Physics 2D Raycaster`**를 검색하여 추가합니다. (이게 있어야 2D 오브젝트 클릭이 가능합니다.)

2.  **Event System 확인**:
    *   `Hierarchy` 창에 `EventSystem` 오브젝트가 있는지 확인합니다.
    *   없다면 `Hierarchy` 우클릭 -> `UI` -> **`Event System`**을 선택하여 추가합니다.

---

## 🧱 2. 테스트 오브젝트(틀) 생성 및 세팅

1.  **Square(붕어빵 틀) 생성**:
    *   `Hierarchy` 우클릭 -> `2D Object` -> `Sprites` -> **`Square`**를 생성합니다.
2.  **컴포넌트 구성**:
    *   생성된 `Square` 오브젝트의 `Inspector` 창에서 다음을 수행합니다:
    *   **`Box Collider 2D` 추가**: `Add Component` -> `Box Collider 2D` (클릭 영역 설정).
    *   **`Bungeo Slot` 스크립트 추가**: `Assets/Scripts/Core/BungeoSlot.cs` 파일을 드래그하여 넣습니다.

---

## 🎮 3. 테스트 진행 (Play Mode)

유니티 상단의 **Play(▶️) 버튼**을 누르고 다음 순서로 조작해 보세요.

| 단계 | 조작 | 시각적 변화 (색상) | 콘솔 로그 (Console) |
| :--- | :--- | :--- | :--- |
| **1. 시작** | **클릭** | 회색(Empty) → **흰색(Batter)** | `○ 반죽 투하!` |
| **2. 익는 중** | 대기 (2초) | 흰색 → **노란색(Cooking)** | (자동 전이) |
| **3. 완벽함** | 대기 (3초) | 노란색 → **황금 갈색(Perfect)** | (자동 전이) |
| **4. 타버림** | 대기 (2초) | 황금 갈색 → **검은색(Burnt)** | (자동 전이) |
| **5. 수확** | **클릭** (Perfect/Burnt 상태) | 현재 색상 → **회색(Empty)** | `★ 성공! ★` 또는 `✖ 실패! ✖` |

---

## 💡 팁 및 문제 해결

*   **클릭이 안 되나요?**:
    *   `Main Camera`에 `Physics 2D Raycaster`가 있는지 다시 확인하세요.
    *   `Square` 오브젝트의 `Z` 축 위치가 카메라보다 뒤에 있는지 확인하세요 (보통 0이면 무난합니다).
*   **속도를 조절하고 싶나요?**:
    *   `BungeoSlot` 컴포넌트의 `Cooking Time`(익는 시간)과 `Perfect Duration`(완성 유지 시간) 수치를 직접 수정해 보세요.

---

## 🚀 다음 단계 (Phase 2 계획)
- [ ] 여러 개의 틀을 관리하는 `CookingManager` 구현
- [ ] 붕어빵 머리 위에 진행률을 보여주는 **Gauge Bar UI** 추가
- [ ] 실제 붕어빵 스프라이트(이미지) 리소스 적용
