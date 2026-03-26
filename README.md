# BungeOPPang
🐟 Bungeoppang Tycoon: Legacy Reboot
  - 아 그립다 2G 시절의 손맛
  - 내가 하고 싶어 만들어 보는 모바일 타이쿤 프로젝트
  - AI 활용

📌 Project Overview
 * 개발 기간: 2026.03 ~ 진행 중
 * 플랫폼: Mobile (Android/iOS)
 * 핵심 목표: 가벼운 모바일 환경에서의 최적화된 상태 관리(FSM)와 확장 가능한 데이터 구조 설계 구현
🛠 Tech Stack
 * Engine: Unity 2022.3 LTS
 * Language: Chttps://www.google.com/search?q=%23
 * Architecture:
   * State Pattern: 붕어빵의 실시간 상태(반죽-익음-탄) 관리
   * Singleton Pattern: 전역 매니저(GameManager, UI) 시스템
   * Data-Driven: JSON/CSV를 활용한 게임 밸런스 외부화
 * Version Control: Git (LFS 적용)
🚀 Key Features (Planned)
 * FSM 기반 굽기 시스템: 각 틀(Slot)별 독립적인 타이머와 상태 변화 로직
 * Combo & Rhythm System: 정확한 타이밍에 뒤집을 시 가산점 부여
 * Real-time Data Integration: (선택 사항) 외부 날씨 API를 활용한 손님 방문 빈도 조절
 * Optimization: 
📂 System Architecture (Draft)
graph TD
    A[GameManager] --> B[InputManager]
    A --> C[UI Manager]
    A --> D[Cooking System]
    D --> E[Bungeoppang State: FSM]
    E -->|Update| F[Visual/Sound Feedback]

📝 Documentations
 * GDD (Game Design Document) - 준비 중
 * TDD (Technical Design Document) - 준비 중
