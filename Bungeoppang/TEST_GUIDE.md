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

### 🔘 소 선택 UI 버튼 설정 (상세 단계)
소 종류를 선택할 버튼 2개를 만들고 스크립트와 연결하는 정확한 방법입니다.

1.  **UI 생성**: `Hierarchy` 우클릭 -> `UI` -> `Button (Legacy 또는 TextMeshPro)` 생성.
2.  **버튼 이름 변경**: 하나는 `RedBeanBtn`, 다른 하나는 `CreamBtn`으로 변경.
3.  **이벤트 연결 (⭐ 중요 ⭐)**:
    *   `RedBeanBtn`을 클릭하여 선택합니다.
    *   `Inspector` 창 아래쪽의 **`On Click ()`** 섹션에서 **`+`** 아이콘을 누릅니다.
    *   **개체 슬롯(None (Object))**: `Hierarchy`에 있는 **`BungeoSlot_0`** 오브젝트를 드래그하여 이 칸에 넣습니다.
    *   **기능 선택(No Function)**: 드롭다운 클릭 -> **`BungeoSlot`** -> **`SelectFilling (int)`**를 선택합니다.
    *   **정수 입력 칸**: 함수 선택 후 아래에 생긴 숫자 입력 칸에 **`1`** (팥)을 입력합니다.
4.  **슈크림 버튼 반복**: `CreamBtn`에도 똑같이 설정하되, 마지막 숫자만 **`2`** (슈크림)로 입력합니다.

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

1.  **소 선택**: 화면의 `팥` 또는 `슈크림` 버튼을 누릅니다. (콘솔에 `소 선택 변경` 로그 확인)
2.  **반죽 붓기**: 틀을 **한 번** 클릭합니다. (흰색 반죽 나타남)
3.  **소 넣기**: 틀을 **한 번 더** 클릭합니다. (반죽 위에 선택한 소 이미지가 나타남)
4.  **자동 굽기**: 소를 넣으면 약 0.5초 뒤 자동으로 게이지가 올라가며 굽기가 시작됩니다.
5.  **수확**: 빵이 노랗게 익었을 때(`Perfect`) 클릭하여 수확합니다. (로그에 종류 확인)

---

## 🔍 문제 해결

*   **Q: OnClick 목록에 SelectFilling이 안 보여요.**
    *   A: `BungeoSlot_0` 오브젝트를 드래그했는지, 그리고 `BungeoSlot` 스크립트 메뉴를 제대로 열었는지 확인하세요.
*   **Q: 소가 반죽 뒤에 가려져요.**
    *   A: `Filling` 오브젝트의 `Order in Layer`를 `2`로, `Content`를 `1`로 설정했는지 확인하세요.
