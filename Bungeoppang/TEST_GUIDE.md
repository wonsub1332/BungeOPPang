# 🐟 붕어빵 굽기 시스템 테스트 가이드 (Unity 6)

본 문서는 Phase 1의 핵심 굽기 로직(FSM), UI 게이지, 실제 에셋 연동 및 **현실적인 굽기 과정(반죽-소-반죽)**을 Unity 6(6000.4.1f1) 환경에서 완벽하게 테스트하기 위한 가이드입니다.

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
1.  **UI 생성**: `Hierarchy` 우클릭 -> `UI` -> `Button` 생성 (`RedBeanBtn`, `CreamBtn`).
2.  **이벤트 연결**:
    *   버튼 클릭 -> `Inspector` -> **`On Click ()`** 섹션에서 `+` 클릭.
    *   **개체 슬롯**: `Hierarchy`에 있는 **`BungeoSlot_0`**을 드래그하여 할당.
    *   **기능 선택**: **`BungeoSlot` -> `SelectFilling (int)`** 선택.
    *   **인자 값**: 팥 버튼은 **`1`**, 슈크림 버튼은 **`2`** 입력.

---

## ⚙️ 3. 스크립트 인스펙터 설정 (Reference 연결)

`BungeoSlot_0`을 선택하고 `Bungeo Slot` 컴포넌트의 빈 칸을 다음과 같이 채웁니다.

| 필드명 | 대상 (드래그 앤 드롭) | 비고 |
| :--- | :--- | :--- |
| **Mold Renderer** | `Mold` 오브젝트 | 틀 배경 |
| **Content Renderer** | `Content` 오브젝트 | 반죽/빵 렌더러 |
| **Filling Renderer** | `Filling` 오브젝트 | 소 렌더러 |
| **Target Width** | `2.5` | 반죽/빵 크기 |
| **Filling Target Width** | `1.2` | **소 크기 (여기서 조절!)** |
| **Batter Sprite** | `batter.png` | - |
| **Bread Sprite** | `bread_nbg.png` | - |
| **Red Bean Sprite** | `RedBean_nbg.png` | - |
| **Cream Sprite** | `cream_nbg.png` | - |
| **Gauge Slider** | `Slider` 오브젝트 | - |

---

## 🎮 4. 테스트 시나리오 (3단계 클릭 루프)

1.  **소 선택**: 화면의 `팥` 또는 `슈크림` 버튼을 누릅니다.
2.  **[1차 클릭] 하단 반죽**: 틀을 클릭하면 흰색 반죽이 채워집니다.
3.  **[2차 클릭] 소 넣기**: 한 번 더 클릭하면 반죽 위에 **선택한 소**가 나타납니다.
4.  **[3차 클릭] 상단 반죽**: 한 번 더 클릭하면 반죽이 소를 덮고(소가 안 보임) **굽기 게이지가 시작**됩니다.
5.  **[수확 클릭] 완벽 타이밍**: 게이지가 차고 빵이 노랗게 익었을 때 클릭하여 수확합니다.

---

## 🔍 문제 해결

*   **Q: 소가 너무 크거나 작아요.**
    *   A: `BungeoSlot` 컴포넌트의 **`Filling Target Width`** 값을 조절하세요. (추천: 1.0 ~ 1.5)
*   **Q: 3번 클릭해도 게이지가 안 올라가요.**
    *   A: 로그에 `[3단계] 상단 반죽으로 덮었습니다!`가 뜨는지 확인하세요. 3차 클릭 전까지는 굽기가 시작되지 않습니다.
