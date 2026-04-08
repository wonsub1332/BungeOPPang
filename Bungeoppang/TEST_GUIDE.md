# 🐟 붕어빵 굽기 시스템 테스트 가이드 (Unity 6)

본 문서는 Phase 1의 핵심 굽기 로직(FSM), UI 게이지, 실제 에셋 연동 및 **소(팥/슈크림) 넣기 기능**을 Unity 6(6000.4.1f1) 환경에서 완벽하게 테스트하기 위한 가이드입니다.

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

1.  **[부모] BungeoSlot_0**: 
    *   `Empty Object` 생성 후 `Box Collider 2D`, `Bungeo Slot` 스크립트 추가.
2.  **[자식 1] Mold**: `2D Object -> Sprite` 생성. `mold.png` 할당. **`Order in Layer: 0`**.
3.  **[자식 2] Content**: `2D Object -> Sprite` 생성. **`Order in Layer: 1`**. (반죽 및 완성된 빵 표시)
4.  **[자식 3] Filling**: `2D Object -> Sprite` 생성. **`Order in Layer: 2`**. (팥/슈크림 소 표시)

### 🔘 소 선택 UI 버튼 (팥/슈크림 선택)
1.  **Canvas 생성**: `Hierarchy` 우클릭 -> `UI` -> `Canvas` (이름: `FillingUI`).
2.  **Button 생성**: `Canvas` 아래에 버튼 2개 생성 (`RedBeanBtn`, `CreamBtn`).
3.  **이벤트 연결 (중요)**:
    *   각 버튼의 **`On Click()`** 섹션에서 `+` 버튼 클릭.
    *   **Object**: `BungeoSlot_0` 오브젝트를 드래그하여 할당.
    *   **Function**: `BungeoSlot` -> **`SelectFilling (int)`** 선택.
    *   **인자 값(정수)**: 
        *   팥 버튼: **`1`** 입력
        *   슈크림 버튼: **`2`** 입력

---

## ⚙️ 3. 스크립트 인스펙터 설정 (Reference 연결)

`BungeoSlot_0`을 선택하고 `Bungeo Slot` 컴포넌트의 빈 칸을 다음과 같이 채웁니다.

| 필드명 | 대상 (드래그 앤 드롭) | 출처 |
| :--- | :--- | :--- |
| **Mold Renderer** | `Mold` 오브젝트 | Hierarchy |
| **Content Renderer** | `Content` 오브젝트 | Hierarchy |
| **Filling Renderer** | `Filling` 오브젝트 | Hierarchy |
| **Target Width** | `2.5` (적절히 조절) | - |
| **Batter Sprite** | `batter.png` | Project |
| **Bread Sprite** | `bread_nbg.png` | Project |
| **Red Bean Sprite** | `RedBean_nbg.png` | Project |
| **Cream Sprite** | `cream_nbg.png` | Project |
| **Gauge Slider** | `Slider` 오브젝트 | Hierarchy |

---

## 🎮 4. 테스트 시나리오 (소 넣기 포함)

1.  **소 선택**: 화면의 `팥` 또는 `슈크림` 버튼을 누릅니다. (콘솔에 로그 확인)
2.  **반죽 붓기**: 틀을 **한 번** 클릭합니다. (흰색 반죽 나타남)
3.  **소 넣기**: 틀을 **한 번 더** 클릭합니다. (반죽 위에 선택한 소 이미지가 나타남)
4.  **자동 굽기**: 소를 넣으면 자동으로 게이지가 올라가며 굽기가 시작됩니다.
5.  **수확**: 빵이 노랗게 익었을 때(`Perfect`) 클릭하여 수확합니다. (로그에 종류 확인)

---

## 🔍 문제 해결

*   **Q: 소가 반죽 뒤에 가려져요.**
    *   A: `Filling` 오브젝트의 `Order in Layer`를 `2`로, `Content`를 `1`로 설정했는지 확인하세요.
*   **Q: 버튼을 눌러도 소가 안 바뀌어요.**
    *   A: 버튼의 `On Click` 이벤트에 `SelectFilling` 함수와 숫자(`1` 또는 `2`)가 정확히 입력되었는지 확인하세요.
