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

### 🎨 에셋 및 스프라이트 구조
1.  **부모 오브젝트**: `Hierarchy` 우클릭 -> `Create Empty` (이름: **`BungeoSlot`**).
    *   **`Box Collider 2D` 추가**: 사이즈를 빵 틀 이미지 크기에 맞게 조절합니다.
    *   **`Bungeo Slot` 스크립트 추가**: `Assets/Scripts/Core/BungeoSlot.cs` 추가.
2.  **자식 오브젝트 (이미지)**:
    *   **`Mold` (틀)**: `Sprite Renderer` 추가, **Order in Layer: 0** 설정. `mold.png` 연결.
    *   **`Content` (반죽/빵)**: `Sprite Renderer` 추가, **Order in Layer: 1** 설정. (빵이 틀 위에 보여야 함)
3.  **스크립트 레퍼런스 연결**:
    *   `BungeoSlot` 인스펙터의 `Mold Renderer`와 `Content Renderer` 칸에 위에서 만든 자식 오브젝트들을 각각 드래그하여 연결합니다.
    *   `Batter Sprite`와 `Bread Sprite` 칸에 각각의 이미지를 연결합니다.

### 🟢 월드 스페이스 게이지 바 (UI)
1.  **Canvas 생성**: `BungeoSlot`의 자식으로 `UI` -> **`Canvas`** 생성.
    *   **Render Mode**: **`World Space`**로 설정.
    *   **Rect Transform**: `Scale`을 `0.005, 0.005, 0.005` 정도로 대폭 줄이고 빵 틀 위쪽으로 배치합니다.
2.  **Slider 생성**: `Canvas` 자식으로 `UI` -> **`Slider`** 생성.
    *   `Handle Slide Area`는 삭제합니다.
    *   `BungeoSlot` 스크립트의 **`Gauge Slider`** 칸에 이 슬라이더를 연결합니다.

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

*   **Q: 클릭했는데 반응이 없어요.**
    *   A1: `Main Camera`에 **`Physics 2D Raycaster`**가 있나요?
    *   A2: `BungeoSlot` 오브젝트에 **`Box Collider 2D`**가 있고 사이즈가 적절한가요?
    *   A3: 씬에 **`EventSystem`** 오브젝트가 존재하나요?
*   **Q: 빵 이미지가 틀 뒤에 가려져요.**
    *   A: `Content` 오브젝트의 `Sprite Renderer` -> **`Order in Layer`** 값을 `Mold`보다 높게(예: 1) 설정하세요.
*   **Q: 게이지 바가 너무 커서 화면을 다 가려요.**
    *   A: `Canvas`의 `Scale`을 더 줄이거나(0.001 등), `Slider`의 `Width/Height`를 조절하세요.
