# UnityAlgorithm
신구대학교 VR게임콘텐츠과 2학년 2학기 게임 알고리즘

<br>

## 그래프
### 그래프의 정의
#### **그래프 G = (V,E)**
- V : 정점 (Vertex) 또는 노드(Node)의 집합
- E : 간선(Edge)의 집합

<br>

#### **실생활 예시**
- 소셜 네트워크 : 사용자(V), 친구관계(E)
- 도로망 : 교차로(V), 도로(E)
- 컴퓨터 네트워크 : 컴퓨터(V), 연결(E)

<br>

### 그래프의 구성 요소
#### **노드(Node)**
- 데이터를 저장하는 기본 단위
- 특징
    - 고유한 식별자(ID) 보유
    - 다양한 데이터 타입 저장 가능
    - 상태 정보 포함 가능 (방문 여부 등)

<br>

#### **엣지(Edge)**
- 노드 간의 연결을 나타냄
- 특징
    - 두 노드를 연결
    - 방향성 가능 (단방향/양방향)
    - 가중치 가능(거리, 비용 등)

<br>

### BFS (Breadth-First Search, 너비 우선 탐색)
- BFS는 그래프 탐색 알고리즘의 하나로, 시작 정점에서 가까운 정점부터 순차적으로 방문하는 방식
- 같은 레벨의 노드들을 먼저 방문
- 자식 노드들을 순차적으로 방문
- Queue(큐) 자료구조를 사용

<br>

- 시작 노드를 큐에 넣고 방문 표시
- 큐에서 노드를 꺼내서 처리
- 해당 노드의 인접 노드들을 큐에 추가
- 큐가 빌 때 까지 2-3 반복

<br>

 #### 장점
 - 최단 경로 보장(가중치 없는 그래프에서)
 - 리프 노드까지의 경로 탐색에 유리
 - 레벨 단위의 탐색이 필요할 때 적합

<br>

 #### 단점
- 메모리 사용량이 많음 (모든 레벨의 노드를 저장)
- 깊이가 깊은 그래프에서는 비효율적
- 재귀적 구현이 어려움

<br>

#### BFS vs DFS

|BFS|DFS|
|-|-|
|레벨 단위 탐색|깊이 단위 탐색|
|큐 사용|스택 사용|
|최단 경로 보장|경로 탐색에 유리|
|더 많은 메모리 사용|적은 메모리 사용|

<br>

### 다익스트라 
- 가중치가 있는 그래프에서 한 정점으로 부터 다른 모든 정점까지의 최단 경로를 찾는 알고리즘

<br>

#### 장점
- 가중치가 있는 그래프에서 최단 경로 보장
- 한 번의 실행으로 모든 노드까지의 최단 거리 계산
- 게임에서 실제 거리, 비용, 시간 등을 고려한 경로 탐색 가능

<br>

#### 제한사항
- 음의 가중치를 처리할 수 없음
- 모든 노드를 방문해야 하므로 BFS보다 느림
- 메모리 사용량이 많음


<br>

#### 게임에서의 활용

1. **네비게이션 시스템**
- 유닛의 최적 경로 찾기
- 지형 비용을 고려한 이동
- 목록

<br>

2. **자원 수집 경로**
- 여러 자원을 효율적으로 수집하는 경로
- 비용 대비 효율 계산
- 목록

<br>

3. **AI 행동 결정**
- NPC의 최적 이동 경로
- 전략적 위치 선정

<br>

#### BFS와의 차이점

|BFS|Dijkstra|
|-|-|
|모든 엣지의 가중치가 동일할 때 사용|가중치가 다른 엣지들을 처리|
|단순 거리 기반 최단 경로|실제 비용 기반 최단 경로|
|더 빠른 실행 속도|더 정확한 경로 계산|

<br>