# BungeOPPang

붕어빵 타이쿤: Legacy Reboot

2G 피처폰 감성의 손맛을 현대 모바일 환경에 맞게 다시 만드는 캐주얼 조리/경영 프로젝트입니다.

## 프로젝트 개요
- 개발 기간: 2026.03 ~ 진행 중
- 플랫폼: Mobile (Android/iOS)
- 엔진: Unity 6 (`6000.4.1f1`)
- 렌더 파이프라인: URP
- 언어: C#

## 핵심 목표
- FSM 기반 조리 상태 관리
- 매니저/이벤트 기반 UI 갱신 구조
- 데이터 외부화(JSON/CSV) 가능한 확장형 구조
- 모바일 친화적 성능 최적화

## 현재 구현 상태
- 슬롯별 굽기 FSM 구현
  - `Empty -> Batter -> Filling -> Covering -> Cooking -> Perfect -> Burnt`
- 도구 선택 기반 상호작용
  - 반죽/소(팥, 슈크림) 선택 후 슬롯 클릭
- 인벤토리/재고 기본 시스템
  - 반죽 소모, 완성품(팥/슈크림) 적재, 골드/보충 UI 연동
- 메인 씬 구성
  - 단일 플레이 씬: `Assets/Scenes/SampleScene.unity`

## 기술 스택
- Unity Packages (주요)
  - `com.unity.render-pipelines.universal`
  - `com.unity.inputsystem`
  - `com.unity.ugui`
  - `com.unity.2d.*`
- 아키텍처
  - State Pattern (BungeoSlot 조리 상태)
  - Singleton Pattern (InventoryManager)
  - Event-driven UI (`OnInventoryChanged`)

## 프로젝트 구조
- `Bungeoppang/Assets/Scripts/Core`
  - `BungeoSlot.cs`: 슬롯 FSM + 조리 인터랙션
  - `BungeoState.cs`: 상태/소 enum
  - `InventoryManager.cs`: 재고/재화 싱글톤
  - `MainUIManager.cs`: 전역 UI 갱신/보충 버튼
  - `ResultPopupUI.cs`: 결과 팝업 UI 컴포넌트
- `Bungeoppang/Assets/Sprites`: 배경/틀/반죽/UI 스프라이트
- `Bungeoppang/ProjectSettings`, `Bungeoppang/Packages`: Unity 설정

## 문서
- GDD: `GDD.md`
- 테스트 가이드: `Bungeoppang/TEST_GUIDE.md`
- 스프라이트 최적화 로그: `Bungeoppang/SPRITE_OPTIMIZATION_LOG.md`

## 로드맵
- Phase 1: 굽기 루프(FSM) 안정화
- Phase 2: 재료 소비/보상/상점 밸런싱 강화
- Phase 3: 손님 주문/판매 루프 추가
- Phase 4: 데이터 외부화(JSON) + 세이브/로드
- Phase 5: 모바일 최적화 및 운영 기능 확장
