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

### 4.1 붕어빵 틀 위 재고 텍스트 추가
1.  **Text 생성**: `Hierarchy`에서 `BungeoSlot_0` -> `Canvas`를 우클릭하여 **`UI` -> `Text`** (Legacy)를 추가합니다.
    *   이름을 **`StockText`**로 변경합니다.
    *   `Rect Transform`에서 위치를 게이지 바(`Slider`)의 바로 위나 아래로 조정합니다.
    *   글자 크기를 적절히 줄이고(예: 14~18), 정렬을 중앙으로 맞춥니다.
2.  **스크립트 연결**: 부모인 **`BungeoSlot_0`**를 선택하고, `Bungeo Slot` 스크립트의 **`Stock Text`** 칸에 방금 만든 `StockText` 오브젝트를 드래그 앤 드롭합니다.

### 4.2 전역 매니저(Managers) 구성
1.  **Manager 오브젝트 생성**: `Hierarchy` 빈 공간 우클릭 -> **`Create Empty`**. 이름을 **`@Managers`**로 변경합니다.
2.  **컴포넌트 추가**: `@Managers` 오브젝트에 다음 스크립트들을 `Add Component` 합니다.
    *   **`Inventory Manager`**: 골드와 재료 데이터를 관리합니다. (싱글톤)
    *   **`Main UI Manager`**: 화면 상/하단 UI(골드, 재고 보충 버튼)를 관리합니다.
3.  **UI 레퍼런스 연결**: `Main UI Manager` 컴포넌트의 빈 칸들에 실제 UI 오브젝트들을 연결합니다.
    *   **Top Bar**: `Gold Text` (골드 표시), `Goal Progress Slider` (목표 진행도), `Goal Text` (목표 수치 표시)
    *   **Refill Buttons**: `Refill Batter Button`, `Refill Red Bean Button`, `Refill Cream Button`을 각각 하단 보충용 버튼 오브젝트와 연결합니다.

### 4.3 결과 팝업(Result Popup) 설정
1.  **Popup 구성**: `Canvas` 아래에 `Panel`을 만들고 이름을 **`ResultPopup`**으로 정합니다.
2.  **스크립트 추가**: `ResultPopup` 오브젝트에 **`Result Popup UI`** 스크립트를 추가합니다.
3.  **컴포넌트 연결**: `Result Popup UI`의 각 필드에 다음을 연결합니다.
    *   **Statistics**: `Score Text`, `Perfect Count Text`, `Earned Gold Text`
    *   **Buttons**: `Restart Button`, `Exit Button`
4.  **비활성화**: 연결이 끝난 뒤, **`ResultPopup` 오브젝트 자체를 비활성화(Inspector 상단 체크 해제)** 해둡니다. (게임 종료 시 코드가 활성화합니다.)
