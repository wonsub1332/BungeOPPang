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

---

## 🛠️ 4. UI 및 재고 시스템 설정 (Phase 1-3)

앞서 만든 붕어빵 틀에 **실시간 재고 표시**와 **전역 UI(골드/보충)**를 연결하는 과정입니다.

### 4.1 붕어빵 트레이(Tray) UI 설정
1.  **Tray Panel 생성**: `Hierarchy`의 `Canvas` 아래에 `Panel`이나 `Empty Object`를 만들고 이름을 **`BungeoTray`**로 변경합니다.
2.  **재고 텍스트 추가**: `BungeoTray` 자식으로 두 개의 **`UI -> Text`**를 만듭니다.
    *   **`TrayRedBeanText`**: 완성된 팥 붕어빵 개수 표시용 (예: "x 0")
    *   **`TrayCreamText`**: 완성된 슈크림 붕어빵 개수 표시용 (예: "x 0")
3.  **스크립트 연결**: **`Main UI Manager`** 컴포넌트의 `Tray Red Bean Text`와 `Tray Cream Text` 칸에 위에서 만든 텍스트 오브젝트들을 각각 연결합니다.

### 4.2 전역 UI 요소 생성 및 매니저 연결
매니저 스크립트에 연결하기 전, 화면에 표시될 UI 요소들을 먼저 생성해야 합니다.

#### **Step 1: UI 레이아웃 구성 (Canvas 아래)**
1.  **Top Bar (상단바)**:
    *   `UI -> Text`: 이름을 **`GoldText`**로 하고 화면 상단에 배치합니다. (예: "1000 G")
    *   `UI -> Slider`: 이름을 **`GoalSlider`**로 하고 상단바 아래에 배치합니다.
    *   `UI -> Text`: 이름을 **`GoalText`**로 하고 슬라이더 옆에 배치합니다. (예: "0 / 10")
2.  **Bottom Bar (보충 버튼)**:
    *   `UI -> Button` 3개를 생성하여 화면 하단에 나란히 배치합니다.
    *   이름을 각각 **`RefillBatterBtn`**, **`RefillRedBeanBtn`**, **`RefillCreamBtn`**으로 정합니다.

#### **Step 2: 전역 매니저(Managers) 설정**
1.  **Manager 오브젝트 생성**: `Hierarchy` 빈 공간 우클릭 -> **`Create Empty`**. 이름을 **`@Managers`**로 변경합니다.
2.  **컴포넌트 추가**: `@Managers` 오브젝트에 다음 스크립트들을 `Add Component` 합니다.
    *   **`Inventory Manager`**: 골드, 원재료 재고, 완성된 붕어빵 재고를 관리합니다. (싱글톤)
    *   **`Main UI Manager`**: 화면 상/하단 UI 및 트레이 재고 표시를 관리합니다.
3.  **UI 레퍼런스 연결**: `Main UI Manager` 컴포넌트의 빈 칸들에 위에서 만든 UI 오브젝트들을 드래그 앤 드롭합니다.
    *   **Top Bar**: `Gold Text`, `Goal Progress Slider`, `Goal Text` 연결.
    *   **Tray UI**: `Tray Red Bean Text`, `Tray Cream Text` 연결.
    *   **Bottom Bar**: `Refill Batter Button`, `Refill Red Bean Button`, `Refill Cream Button` 연결.

### 4.3 결과 팝업(Result Popup) 설정
1.  **Popup 구성**: `Canvas` 아래에 `Panel`을 만들고 이름을 **`ResultPopup`**으로 정합니다.
2.  **내부 요소 생성**: `ResultPopup` 자식으로 다음을 추가합니다.
    *   `UI -> Text` 3개: **`ScoreText`**, **`PerfectCountText`**, **`EarnedGoldText`**.
    *   `UI -> Button` 2개: **`RestartBtn`**, **`ExitBtn`**.
3.  **스크립트 추가**: `ResultPopup` 오브젝트에 **`Result Popup UI`** 스크립트를 추가합니다.
4.  **컴포넌트 연결**: `Result Popup UI`의 각 필드에 위에서 만든 텍스트와 버튼들을 연결합니다.
5.  **비활성화**: 연결이 끝난 뒤, **`ResultPopup` 오브젝트 자체를 비활성화(Inspector 상단 체크 해제)** 해둡니다.
