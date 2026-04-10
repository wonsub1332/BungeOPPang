# Game Design Document: BungeOPPang

- Version: 1.1.0
- Last Updated: 2026-04-10
- Focus: FSM Cooking Loop, Mobile-first UX, Data-driven Extension

## 1. 프로젝트 개요
- 장르: 캐주얼 조리/경영 시뮬레이션
- 플랫폼: Mobile (Android/iOS)
- 엔진: Unity 6 (`6000.4.1f1`)
- 핵심 컨셉: 2G 피처폰 감성의 리드미컬한 붕어빵 굽기

## 2. 핵심 게임 루프
1. 재료 선택 (반죽/소)
2. 슬롯 조리 진행 (상태 전이)
3. 완성품 수확
4. 보상/재고 반영
5. 재료 보충 및 반복

## 3. FSM 설계
현재 코드 기준 상태 정의:

- `Empty`: 빈 틀
- `Batter`: 하단 반죽 입력
- `Filling`: 소 입력
- `Covering`: 상단 반죽 덮기
- `Cooking`: 굽기 진행
- `Perfect`: 적정 조리 완료
- `Burnt`: 과조리

상태 전이(현재 구현):
- `Empty --(반죽 선택 + 클릭)--> Batter`
- `Batter --(소 선택 + 클릭)--> Filling`
- `Filling --(반죽 선택 + 클릭)--> Covering`
- `Covering --(0.5초 후)--> Cooking`
- `Cooking --(cookingTime 경과)--> Perfect`
- `Perfect --(perfectDuration 경과)--> Burnt`
- `Perfect/Burnt --(클릭)--> Empty`

## 4. 시스템 아키텍처
- `BungeoSlot`
  - 슬롯 단위 FSM/타이머/비주얼/수확 로직
- `InventoryManager` (Singleton)
  - 골드/재료/완성품 수량 보관
  - 변경 이벤트(`OnInventoryChanged`) 발행
- `MainUIManager`
  - 골드/목표/트레이 재고/보충 버튼 UI 갱신
- `ResultPopupUI`
  - 결과 팝업 표시 컴포넌트 (현재 트리거 연동은 후속 작업)

## 5. 데이터 모델 (현재)
- 재화
  - `currentGold`
- 재료
  - `batterCount`, `redBeanCount`, `creamCount`
- 완성품
  - `redBeanBungeoCount`, `creamBungeoCount`

## 6. 현재 구현 범위 (Phase 1)
완료:
- 슬롯 클릭 기반 조리 루프
- 조리 게이지 및 상태별 색/스프라이트 피드백
- 인벤토리 변경 이벤트 기반 UI 반영
- 재료 보충 버튼 및 골드 차감(고정 비용)

진행 필요:
- 소(팥/슈크림) 재료 소비 검증 로직 강화
- 점수/정산/결과 팝업 트리거 연동
- 손님 주문/판매 루프

## 7. UI/UX 구성
- Top Bar: 골드, 목표 슬라이더, 목표 텍스트
- Center: 붕어빵 슬롯들
- Bottom Bar: 반죽/소 선택 및 재료 보충
- Tray: 완성품 수량 표시

## 8. 기술 방향
- 상태 전이는 `enum + switch` 기반으로 단순/명확하게 유지
- UI는 이벤트 구독 방식으로 결합도 최소화
- 수치 데이터는 추후 JSON Scriptable 데이터로 외부화
- 모바일 빌드 기준 프레임/메모리 사용량 점검

## 9. 로드맵
- Phase 1: FSM 및 핵심 조리 루프 안정화
- Phase 2: 재료/보상 밸런스 및 상점 확장
- Phase 3: 손님 주문/판매/정산 시스템
- Phase 4: 세이브/로드 및 데이터 외부화
- Phase 5: 라이브 운영 요소(이벤트/랭킹 등) 검토
