# 🐟 붕어빵 굽기 시스템 테스트 가이드 (Unity 6)

본 문서는 Phase 1에서 구현된 핵심 굽기 로직(FSM)과 UI 게이지, 그리고 실제 에셋 연동 방법을 설명합니다.

---

## 🛠️ 1. 유니티 프로젝트 초기 설정 (필수)

### 📱 세로 모드(Portrait) 설정
1.  **Game 창**: 상단 드롭다운 메뉴에서 `Free Aspect` 대신 **`1080 x 1920 (9:16)`** 또는 `Portrait`을 선택합니다.
2.  **Player Settings**: `Edit` -> `Project Settings` -> `Player` -> `Resolution and Presentation`에서 `Default Orientation`을 **`Portrait`**으로 설정합니다.

### 🖱️ 입력 및 카메라 설정
1.  **Main Camera**: `Hierarchy`에서 `Main Camera` 선택 후 `Add Component` -> **`Physics 2D Raycaster`** 추가.
2.  **Event System**: `Hierarchy` 우클릭 -> `UI` -> **`Event System`** 추가 (씬에 이미 있다면 생략).

---

## 🧱 2. 테스트 오브젝트(틀) 및 UI 구성

### 🎨 에셋 및 오브젝트 세팅
1.  **BungeoSlot 오브젝트 생성**: `Hierarchy` 우클릭 -> `Create Empty`로 빈 오브젝트 생성 후 이름을 **`BungeoSlot`**으로 변경.
2.  **자식 오브젝트 구성**: `BungeoSlot` 자식으로 두 개의 `Sprite Renderer`를 만듭니다.
    *   **Mold**: 붕어빵 틀 이미지를 넣을 곳 (Order in Layer: 0).
    *   **Content**: 반죽/빵 이미지가 나타날 곳 (Order in Layer: 1).
3.  **컴포넌트 추가 (`BungeoSlot`)**:
    *   **`Box Collider 2D`**: 클릭 영역 설정 (Size를 틀 이미지에 맞게 조절).
    *   **`Bungeo Slot`**: `Assets/Scripts/Core/BungeoSlot.cs` 추가.
4.  **스크립트 에셋 연결**: `BungeoSlot` 컴포넌트의 인스펙터 칸에 다음을 연결합니다.
    *   **Mold Renderer**: 자식인 `Mold` 오브젝트 연결.
    *   **Content Renderer**: 자식인 `Content` 오브젝트 연결.
    *   **Batter Sprite**: `Assets/Sprites/Pastry/batter.png` 연결.
    *   **Bread Sprite**: `Assets/Sprites/Pastry/bread.png` 연결.

### 🟢 게이지 바(Gauge Bar) 구성
1.  **Canvas 생성**: `BungeoSlot` 자식으로 `UI` -> **`Canvas`** 생성.
    *   **Render Mode**: **`World Space`**로 변경.
    *   **Scale**: `0.01, 0.01, 0.01` (매우 작게 설정).
    *   **Position**: 붕어빵 위쪽(`Y: 1.2` 정도)으로 배치.
2.  **Slider 생성**: 생성된 `Canvas` 자식으로 `UI` -> **`Slider`** 생성.
    *   `Handle Slide Area` 자식 오브젝트는 삭제 (게이지 용도이므로 손잡이 불필요).
3.  **연결**: `BungeoSlot`의 `Gauge Slider` 칸에 방금 만든 `Slider`를 드래그 앤 드롭합니다.

---

## 🎮 3. 테스트 진행 (Play Mode)

유니티 상단의 **Play(▶️) 버튼**을 누르고 다음 순서로 조작해 보세요.

| 단계 | 조작 | 시각적 변화 (이미지/게이지) | 콘솔 로그 (Console) |
| :--- | :--- | :--- | :--- |
| **1. 시작** | **클릭** | 흰색 **반죽 이미지** 나타남 | `○ 반죽 투하!` |
| **2. 익는 중** | 대기 | 반죽 색상이 서서히 **노란색**으로 변함 | (자동 전이) |
| **3. 완벽함** | 대기 | 이미지가 **붕어빵(Bread)**으로 교체됨 | (자동 전이) |
| **4. 타버림** | 대기 | 빵 이미지가 **검게** 변함 | (자동 전이) |
| **5. 수확** | **클릭** | 내용물 이미지와 게이지가 **사라짐** | `★ 성공! ★` 또는 `✖ 실패! ✖` |

---

## 💡 팁 및 문제 해결

*   **이미지가 안 보이나요?**: `Order in Layer`를 확인하세요 (틀: 0, 내용물: 1).
*   **클릭이 안 되나요?**: `Box Collider 2D`가 제대로 추가되었고 `Raycaster`가 카메라에 있는지 확인하세요.

---

## 🚀 다음 단계 (Phase 2 계획)
- [ ] 여러 개의 틀을 관리하는 `CookingManager` 구현
- [ ] 점수 및 재화 시스템 (`GameManager`)
- [ ] 수확 시 빵이 튀어오르는 애니메이션 연출
