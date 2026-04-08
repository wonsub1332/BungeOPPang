# 🐟 붕어빵 굽기 시스템 테스트 가이드 (Unity 6)

본 문서는 Phase 1의 핵심 굽기 로직(FSM), UI 게이지, 실제 에셋 연동 및 **도구 선택 시스템(반죽/소)**을 Unity 6(6000.4.1f1) 환경에서 완벽하게 테스트하기 위한 가이드입니다.

---

## 🛠️ 1. 프로젝트 환경 설정 (최초 1회)

### 📱 세로 모드(Portrait) 고정
1.  **Game 창 설정**: 상단 메뉴바 아래 `Game` 탭에서 해상도를 **`1080 x 1920 (9:16)`**으로 변경합니다.
2.  **빌드 설정 고정**: `Edit` -> `Project Settings` -> `Player` -> `Resolution and Presentation`에서 `Orientation`을 **`Portrait`**으로 설정합니다.

### 🖱️ 입력 시스템 및 카메라 설정 (클릭 감지 필수)
1.  **Main Camera**: `Hierarchy`에서 `Main Camera` 선택 후 `Add Component` -> **`Physics 2D Raycaster`**를 추가합니다.
2.  **EventSystem**: `Hierarchy` 우클릭 -> `UI` -> **`Event System`**을 추가합니다. (`Replace with InputSystemUIInputModule` 클릭)

---

## 🧱 2. 오브젝트 및 UI 구성

### 🎨 붕어빵 슬롯 구조 (4층 구조)
하나의 붕어빵 틀은 **부모(로직) + 자식 1(틀) + 자식 2(반죽/빵) + 자식 3(소)** 구조로 만듭니다.

1.  **[부모] BungeoSlot_0**: `Empty Object` 생성 후 `Box Collider 2D`, `Bungeo Slot` 스크립트 추가.
2.  **[자식 1] Mold**: `2D Object -> Sprite` 생성. `mold.png` 할당. **`Order in Layer: 0`**.
3.  **[자식 2] Content**: `2D Object -> Sprite` 생성. **`Order in Layer: 1`**.
4.  **[자식 3] Filling**: `2D Object -> Sprite` 생성. **`Order in Layer: 2`**.

### 🔘 도구 선택 버튼 설정 (반죽/소)
도구를 먼저 선택해야 틀에 내용물을 채울 수 있습니다.

1.  **버튼 생성**: `Hierarchy` 우클릭 -> `UI` -> `Button` 3개 생성 (`BatterBtn`, `RedBeanBtn`, `CreamBtn`).
2.  **이벤트 연결**:
    *   **BatterBtn (반죽)**:
        *   `On Click ()` -> `+` 클릭 -> `BungeoSlot_0` 할당.
        *   Function: **`BungeoSlot -> SelectBatterMode`** 선택.
    *   **RedBeanBtn (팥)**:
        *   `On Click ()` -> `+` 클릭 -> `BungeoSlot_0` 할당.
        *   Function: **`BungeoSlot -> SelectFillingMode (int)`** 선택 -> 인자값 **`1`** 입력.
    *   **CreamBtn (슈크림)**:
        *   `On Click ()` -> `+` 클릭 -> `BungeoSlot_0` 할당.
        *   Function: **`BungeoSlot -> SelectFillingMode (int)`** 선택 -> 인자값 **`2`** 입력.

---

## 🎮 3. 테스트 시나리오 (도구 교체 루프)

실제 붕어빵을 굽는 순서대로 도구를 바꿔가며 클릭해야 합니다.

1.  **반죽 선택**: `반죽 버튼`을 누릅니다.
2.  **1차 반죽**: 틀을 클릭합니다. (바닥 반죽 채워짐)
3.  **소 선택**: `팥` 또는 `슈크림` 버튼을 누릅니다.
4.  **소 넣기**: 틀을 클릭합니다. (**선택한 소 이미지**가 나타남)
5.  **반죽 선택**: 다시 `반죽 버튼`을 누릅니다.
6.  **2차 반죽**: 틀을 클릭합니다. (반죽이 소를 덮고 **굽기 게이지 시작**)
7.  **수확**: 빵이 노랗게 익었을 때 클릭하여 수확합니다.

---

## 🔍 문제 해결

*   **Q: 클릭했는데 "반죽을 먼저 선택해야 합니다!" 경고가 떠요.**
    *   A: 현재 선택된 도구가 상태와 맞지 않는 것입니다. 화면의 버튼을 눌러 올바른 도구(반죽 주전자 또는 소 국자)를 선택했는지 확인하세요.
*   **Q: 버튼을 눌러도 반응이 없어요.**
    *   A: 버튼의 `On Click` 이벤트에 함수가 정확히 연결되었는지 확인하세요. (`SelectBatterMode`는 인자가 없고, `SelectFillingMode`는 숫자가 필요합니다.)
