# 🐟 붕어빵 굽기 시스템 테스트 가이드 (Unity 6)

본 문서는 Phase 1의 핵심 굽기 로직(FSM), UI 게이지, 실제 에셋 연동을 Unity 6(6000.4.1f1) 환경에서 완벽하게 테스트하기 위한 가이드입니다.

---

## 🛠️ 1. 프로젝트 환경 설정 (최초 1회)

### 📱 세로 모드(Portrait) 고정
1.  **Game 창 설정**: 상단 메뉴바 아래 `Game` 탭에서 해상도를 **`1080 x 1920 (9:16)`**으로 변경합니다. (목록에 없다면 `+`를 눌러 추가)
2.  **빌드 설정 고정**: 
    *   `Edit` -> `Project Settings` -> `Player` 메뉴로 들어갑니다.
    *   `Resolution and Presentation` 섹션을 확장합니다.
    *   **`Orientation`** 항목의 `Default Orientation`을 **`Portrait`**으로 설정합니다.
    *   `Allowed Orientations for Auto Rotation`에서 Portrait을 제외한 나머지는 체크 해제하여 화면 회전을 방지합니다.

### 🖱️ 입력 시스템 및 카메라 설정 (클릭 감지 필수)
1.  **Main Camera**: `Hierarchy`에서 `Main Camera` 선택 후 `Add Component` -> **`Physics 2D Raycaster`**를 추가합니다.
2.  **EventSystem**: `Hierarchy` 우클릭 -> `UI` -> **`Event System`**을 추가합니다.
    *   `Inspector`에서 `Replace with InputSystemUIInputModule` 버튼이 보인다면 클릭하여 업데이트합니다.

---

## 🧱 2. 오브젝트(틀) 및 UI 구성

### 🎨 붕어빵 슬롯 구조 만들기 (자식 오브젝트 생성 포함)
하나의 붕어빵 틀은 **부모(로직) + 자식 1(틀 이미지) + 자식 2(빵 이미지)**의 3층 구조로 만듭니다.

1.  **[부모] 로직 오브젝트 생성**: 
    *   `Hierarchy` 빈 공간 우클릭 -> **`Create Empty`** 클릭. 이름을 **`BungeoSlot_0`**으로 변경.
    *   `Inspector`에서 `Add Component` -> **`Box Collider 2D`** 추가.
    *   `Inspector`에서 `Add Component` -> **`Bungeo Slot`** 스크립트 추가.
2.  **[자식 1] 틀 이미지(Mold) 생성**:
    *   `Hierarchy`에서 방금 만든 **`BungeoSlot_0` 오브젝트 위에서 우클릭**합니다.
    *   **`2D Object` -> `Sprites` -> `Square`**를 선택합니다. 이름을 **`Mold`**로 변경. (이렇게 하면 부모 아래로 쏙 들어갑니다.)
    *   `Inspector`의 `Sprite Renderer` -> `Sprite` 칸에 `mold.png`를 드래그해서 넣습니다.
    *   **`Order in Layer`를 `0`으로 설정**합니다.
3.  **[자식 2] 내용물 이미지(Content) 생성**:
    *   다시 **`BungeoSlot_0` 오브젝트 위에서 우클릭**합니다.
    *   **`2D Object` -> `Sprites` -> `Square`**를 선택합니다. 이름을 **`Content`**로 변경.
    *   `Inspector`의 `Sprite Renderer` -> `Sprite` 칸은 비워두거나 `batter.png`를 넣어둡니다.
    *   **`Order in Layer`를 `1`로 설정**합니다. (그래야 틀 위에 빵이 보입니다.)
4.  **스크립트 레퍼런스 연결 (마무리)**:
    *   `Hierarchy`에서 다시 부모인 **`BungeoSlot_0`**를 선택합니다.
    *   `Bungeo Slot` 컴포넌트의 빈칸들에 자식들을 드래그해서 넣습니다:
        *   `Mold Renderer`: 자식인 `Mold`를 드래그.
        *   `Content Renderer`: 자식인 `Content`를 드래그.
        *   `Batter Sprite`: `Assets/Sprites/Pastry/batter.png` 연결.
        *   `Bread Sprite`: `Assets/Sprites/Pastry/bread.png` 연결.

### 🟢 월드 스페이스 게이지 바 (UI)
1.  **Canvas 생성**: `BungeoSlot_0` 오브젝트 위에서 우클릭 -> **`UI` -> `Canvas`** 생성.
    *   **Render Mode**: **`World Space`**로 설정.
    *   **Rect Transform**: `Scale`을 `0.005, 0.005, 0.005`로 줄이고 빵 틀 위쪽으로 배치합니다.
2.  **Slider 생성**: 생성된 `Canvas` 위에서 우클릭 -> **`UI` -> `Slider`** 생성.
    *   `Handle Slide Area` 자식 오브젝트는 삭제합니다.
3.  **연결**: 부모 `BungeoSlot_0`를 선택하고, `Bungeo Slot` 스크립트의 **`Gauge Slider`** 칸에 이 슬라이더를 드래그 앤 드롭합니다.

---

## 🎮 3. 테스트 시나리오

| 액션 | 기대 결과 |
| :--- | :--- |
| **틀 클릭** | 회색 틀 위에 **흰색 반죽**이 나타나며 게이지가 차오름. |
| **대기 (익음)** | 반죽 색상이 노란색으로 변하다가, 어느 순간 **붕어빵 이미지**로 교체됨. |
| **대기 (탐)** | 붕어빵 이미지가 점점 어두워져 **검은색**이 됨. |
| **수확 클릭** | 빵과 게이지가 사라지고 콘솔에 `성공` 또는 `실패` 메시지 출력. |

---

## 🔍 문제 해결 (Checklist)

*   **Q: 자식 오브젝트가 부모 밖으로 나가요.**
    *   A: `Hierarchy` 창에서 자식 오브젝트를 마우스로 잡고 부모 오브젝트 이름 위로 드래그하면 다시 자식으로 들어갑니다.
*   **Q: 클릭했는데 반응이 없어요.**
    *   A1: `Main Camera`에 **`Physics 2D Raycaster`**가 있나요?
    *   A2: `BungeoSlot_0` 오브젝트에 **`Box Collider 2D`**가 있고 사이즈가 적절한가요?
*   **Q: 빵 이미지가 틀 뒤에 가려져요.**
    *   A: `Content` 오브젝트의 `Sprite Renderer` -> **`Order in Layer`** 값이 `Mold`보다 높아야 합니다.
