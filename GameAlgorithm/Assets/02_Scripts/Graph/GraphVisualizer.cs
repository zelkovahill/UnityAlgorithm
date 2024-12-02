using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphVisualizer : MonoBehaviour
{
    [Header("프리팹")]
    [Tooltip("노드 프리팹")]
    [SerializeField]
    private GameObject nodePrefab;

    [Tooltip("엣지 프리팹")]
    [SerializeField]
    private GameObject edgePrefab;


    [Header("비주얼 세팅")]
    [Tooltip("노드 간의 기본 간격")]
    [SerializeField]
    private float nodeSpacing = 2f;

    [Tooltip("애니메이션 속도")]
    [SerializeField]
    private float animationSpeed = 1f;

    [Header("UI 레퍼런스")]
    [Tooltip("현재 상태를 표시할 UI")]
    [SerializeField]
    private TextMeshProUGUI lessonText;

    private Dictionary<int, GameObject> nodes;      // 노드 ID 와 노드 오브젝트 간의 배열
    private List<GameObject> edges;                 // 생성된 엣지 오브젝트 등록
    private List<int>[] adjList;                    // 그래프의 인접 리스트 표현

    private Material defaultMaterial;
    private Material visitedMaterial;
    private Material currentMaterial;
    private Coroutine currentLessonCoroutine;

    public int currentLesson;
    private bool isLessonComplete = false;

    private void Start()
    {
        // 함수 초기화
        InitializeGraph();
        StartLesson(currentLesson);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isLessonComplete && currentLesson < 6)
            {
                StopCurrentLessonIfRunning();
                StartLesson(currentLesson + 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (/*isLessonComplete &&*/ currentLesson > 1)
            {
                StopCurrentLessonIfRunning();
                StartLesson(currentLesson - 1);
            }
        }
    }


    /// <summary>
    /// 그래프 초기화
    /// </summary>
    private void InitializeGraph()
    {
        nodes = new Dictionary<int, GameObject>();
        edges = new List<GameObject>();
        CreateMaterials();
    }

    /// <summary>
    /// 보여진 노드 메테리얼 색 설정
    /// </summary>
    private void CreateMaterials()
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        defaultMaterial = new Material(shader);
        defaultMaterial.SetColor("_BaseColor", Color.white);

        visitedMaterial = new Material(shader);
        visitedMaterial.SetColor("_BaseColor", Color.green);

        currentMaterial = new Material(shader);
        currentMaterial.SetColor("_BaseColor", Color.red);
    }

    /// <summary>
    /// 노드를 생성하고 시작적으로 표시
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    private void CreateNode(int id, Vector3 position)
    {
        GameObject node = Instantiate(nodePrefab, position, Quaternion.identity);
        nodes[id] = node;

        // 노드 번호 표시
        GameObject textObj = new GameObject($"NodeText_{id}");
        textObj.transform.SetParent(node.transform);
        textObj.transform.localPosition = new Vector3(0, 0.6f, 0);

        TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
        tmp.text = id.ToString();
        tmp.fontSize = 10;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.black;
    }


    /// <summary>
    /// 두 노드 간의 엣지를 생성하고 시작적으로 표시
    /// </summary>
    /// <param name="fromId">출발노드</param>
    /// <param name="toId">도착 노드</param>
    private void CreateEdgeBetweenNodes(int fromId, int toId)
    {
        if (nodes.ContainsKey(fromId) && nodes.ContainsKey(toId))
        {
            GameObject edge = Instantiate(edgePrefab);
            LineRenderer line = edge.GetComponent<LineRenderer>();

            line.SetPosition(0, nodes[fromId].transform.position);
            line.SetPosition(1, nodes[toId].transform.position);


            edges.Add(edge);
        }
    }


    /// <summary>
    /// 그래프를 초기화하는 함수
    /// </summary>
    private void ClearGraph()
    {
        foreach (var node in nodes.Values)
        {
            Destroy(node);
        }

        foreach (var edge in edges)
        {
            Destroy(edge);
        }

        nodes.Clear();
        edges.Clear();
    }

    // 코루틴 함수 등록

    /// <summary>
    /// 기본 그래프 구조
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson1_BasicGraph()
    {
        Vector3[] nodePositions = new Vector3[]
        {
            new Vector3(0,0,2),      // 노드 0
            new Vector3(2,0,0),      // 노드 1
            new Vector3(-2,0,0),     // 노드 2    
            new Vector3(0,0,-2),     // 노드 3
        };

        // 노드 순차적 생성
        for (int i = 0; i < nodePositions.Length; i++)
        {
            CreateNode(i, nodePositions[i]);
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);


        int[,] edges = new int[,]
        {
            {0,1},     // 노드 0과 1 연결
            {1,2},     // 노드 1과 2 연결
            {2,3},     // 노드 2과 3 연결
            {3,0},     // 노드 3과 0 연결
        };

        // 엣지 순차적 생성
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            CreateEdgeBetweenNodes(edges[i, 0], edges[i, 1]);
            yield return new WaitForSeconds(1f);
        }
        isLessonComplete = true;
    }


    /// <summary>
    /// 깊이 우선 탐색
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson2_DFS()
    {
        Vector3[] nodePositions = new Vector3[]
        {
            new Vector3(0,0,0),      // 노드 0
            new Vector3(2,0,2),      // 노드 1
            new Vector3(-2,0,2),     // 노드 2    
            new Vector3(2,0,-2),     // 노드 3
            new Vector3(-2,0,-2),    // 노드 4
            new Vector3(0,0,-4),     // 노드 5
        };

        // 노드 순차적 생성
        for (int i = 0; i < nodePositions.Length; i++)
        {
            CreateNode(i, nodePositions[i]);
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);


        int[,] edges = new int[,]
        {
            {0,1}, {2,4},
            {0,2}, {3,4},
            {1,3}, {3,5},
            {2,3}, {4,5},
        };

        // 엣지 순차적 생성
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            CreateEdgeBetweenNodes(edges[i, 0], edges[i, 1]);
            yield return new WaitForSeconds(1f);
        }

        // 인접 리스트 설정
        adjList = new List<int>[nodePositions.Length];
        for (int i = 0; i < nodePositions.Length; i++)
        {
            adjList[i] = new List<int>();
        }

        // 양방향 엣지 추가
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            int from = edges[i, 0];
            int to = edges[i, 1];
            adjList[from].Add(to);
            adjList[to].Add(from);
        }

        yield return new WaitForSeconds(1f);


        // DFS 실행
        bool[] visited = new bool[nodePositions.Length];
        Stack<int> stack = new Stack<int>();

        // 시작 노드 0번
        stack.Push(0);

        while (stack.Count > 0)
        {
            int current = stack.Pop();

            if (!visited[current])
            {
                visited[current] = true;
                nodes[current].GetComponent<MeshRenderer>().material = currentMaterial;
                yield return new WaitForSeconds(1f);

                // 방문 완료 표시
                nodes[current].GetComponent<MeshRenderer>().material = visitedMaterial;

                // 인접 노드들을 스텍에 추가 (역순으로 추가하여 작은 번호부터 방문
                for (int i = adjList[current].Count - 1; i >= 0; i--)
                {
                    int neighbor = adjList[current][i];

                    if (!visited[neighbor])
                    {
                        stack.Push(neighbor);
                    }
                }
            }
        }
        isLessonComplete = true;
    }

    /// <summary>
    /// 너비 우선 탐색
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson3_BFS()
    {
        Vector3[] nodePositions = new Vector3[]
        {
            new Vector3(0,0,0),      // 노드 0
            new Vector3(2,0,2),      // 노드 1
            new Vector3(-2,0,2),     // 노드 2    
            new Vector3(2,0,-2),     // 노드 3
            new Vector3(-2,0,-2),    // 노드 4
            new Vector3(0,0,-4),     // 노드 5
        };


        // 노드 순차적 생성
        for (int i = 0; i < nodePositions.Length; i++)
        {
            CreateNode(i, nodePositions[i]);
            yield return new WaitForEndOfFrame();
        }

        int[,] edges = new int[,]
        {
            {0,1}, {2,4},
            {0,2}, {3,4},
            {1,3}, {3,5},
            {2,3}, {4,5},
         };

        // 엣지 순차적 생성
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            CreateEdgeBetweenNodes(edges[i, 0], edges[i, 1]);
            yield return new WaitForEndOfFrame();
        }

        // 인접 리스트 설정
        adjList = new List<int>[nodePositions.Length];
        for (int i = 0; i < nodePositions.Length; i++)
        {
            adjList[i] = new List<int>();
        }

        // 양방향 엣지 추가
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            int from = edges[i, 0];
            int to = edges[i, 1];
            adjList[from].Add(to);
            adjList[to].Add(from);
        }

        yield return null;

        // BFS
        bool[] visited = new bool[nodePositions.Length];
        Queue<int> queue = new Queue<int>();

        // 시작 노드
        queue.Enqueue(0);
        visited[0] = true;

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();

            // 현재 노드 처리
            nodes[current].GetComponent<MeshRenderer>().material = currentMaterial;
            yield return new WaitForSeconds(0.5f);
            nodes[current].GetComponent<MeshRenderer>().material = visitedMaterial;

            // 인접 노드들을 큐에 추가
            foreach (int neighbor in adjList[current])
            {
                // 방문하지 않은 노드만 큐에 추가
                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    queue.Enqueue(neighbor);
                    nodes[neighbor].GetComponent<MeshRenderer>().material.color = Color.yellow;   // 큐에 추가된 노드는 노란색 표시
                }
            }

            yield return new WaitForEndOfFrame();
        }

        isLessonComplete = true;
    }

    /// <summary>
    /// 다익스트라
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson4_Dijkstra()
    {

        Vector3[] nodePositions = new Vector3[]
        {
            new Vector3(0,0,0),      // 노드 0
            new Vector3(2,0,2),      // 노드 1
            new Vector3(-2,0,2),     // 노드 2    
            new Vector3(2,0,-2),     // 노드 3
            new Vector3(-2,0,-2),    // 노드 4
            new Vector3(0,0,-4),     // 노드 5
        };

        // 노드 순차적 생성
        for (int i = 0; i < nodePositions.Length; i++)
        {
            CreateNode(i, nodePositions[i]);
            yield return new WaitForEndOfFrame();
        }

        int[,] edges = new int[,]
        {
            {0,1}, {2,4},
            {0,2}, {3,4},
            {1,3}, {3,5},
            {2,3}, {4,5},
         };

        // 엣지 순차적 생성
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            CreateEdgeBetweenNodes(edges[i, 0], edges[i, 1]);
            yield return new WaitForEndOfFrame();
        }

        // 다익스트라 알고리즘 실행
        int startNode = 0;
        int nodeCount = nodePositions.Length;
        float[] distances = new float[nodeCount];
        bool[] visited = new bool[nodeCount];
        int[] previous = new int[nodeCount];

        // 초기화
        for (int i = 0; i < nodeCount; i++)
        {
            distances[i] = float.MaxValue;
            previous[i] = -1;
        }

        distances[startNode] = 0;

        // 메인 알고리즘
        for (int i = 0; i < nodeCount; i++)
        {
            // 최소 거리를 가진 미방문 노드 찾기
            float minDistance = float.MaxValue;
            int currentNode = -1;

            for (int j = 0; j < nodeCount; j++)
            {
                if (!visited[j] && distances[j] < minDistance)
                {
                    minDistance = distances[j];
                    currentNode = j;
                }
            }

            if (currentNode == -1)
            {
                break;
            }

            // 현재 노드 처리
            visited[currentNode] = true;
            nodes[currentNode].GetComponent<MeshRenderer>().material = currentMaterial;
            yield return new WaitForSeconds(1f / animationSpeed);
            nodes[currentNode].GetComponent<MeshRenderer>().material = visitedMaterial;


            // 이웃 노드들의 거리 업데이트
            for (int j = 0; j < nodeCount; j++)
            {
                if (IsConnected(currentNode, j))
                {
                    float weight = Vector3.Distance(nodes[currentNode].transform.position, nodes[j].transform.position);
                    float newDistance = distances[currentNode] + weight;

                    if (newDistance < distances[j])
                    {
                        distances[j] = newDistance;
                        previous[j] = currentNode;

                        // 거리가 업데이트된 노드는 파란색으로 표시
                        if (visited[j])
                        {
                            nodes[j].GetComponent<MeshRenderer>().material.color = Color.blue;
                        }
                    }
                }
            }
        }

        // 최단 경로 시각화 (도착점부터 역추적)
        int current = nodeCount - 1; // 도착점

        while (current != -1 && current != startNode)
        {
            nodes[current].GetComponent<MeshRenderer>().material.color = Color.yellow;
            current = previous[current];
            yield return new WaitForSeconds(0.3f);
        }

        if (current == startNode)
        {
            nodes[startNode].GetComponent<MeshRenderer>().material.color = Color.yellow;
        }

        yield return null;
        isLessonComplete = true;
    }


    private bool IsConnected(int node1, int node2)
    {
        // 인접 리스트 사용
        if (adjList != null && adjList[node1] != null)
        {
            return adjList[node1].Contains(node2);
        }

        // 엣지 리스트로 연결 확인
        foreach (GameObject edge in edges)
        {
            LineRenderer line = edge.GetComponent<LineRenderer>();
            Vector3 start = line.GetPosition(0);
            Vector3 end = line.GetPosition(1);

            if ((Vector3.Distance(start, nodes[node1].transform.position) < 0.1f &&
     Vector3.Distance(end, nodes[node2].transform.position) < 0.1f) ||
     (Vector3.Distance(start, nodes[node2].transform.position) < 0.1f &&
     Vector3.Distance(end, nodes[node1].transform.position) < 0.1f))
            {
                return true;
            }

        }
        return false;
    }

    /// <summary>
    /// 에이스타
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson5_AStar()
    {
        yield return null;
    }

    /// <summary>
    /// 프림
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson6_Prim()
    {
        yield return null;
    }


    /// <summary>
    /// 특정 코루틴 실행
    /// </summary>
    /// <param name="lessonNumber"></param>
    private void StartLesson(int lessonNumber)
    {
        ClearGraph();
        currentLesson = lessonNumber;
        isLessonComplete = false;

        switch (lessonNumber)
        {
            case 1:
                currentLessonCoroutine = StartCoroutine(Lesson1_BasicGraph());
                break;
            case 2:
                currentLessonCoroutine = StartCoroutine(Lesson2_DFS());
                break;
            case 3:
                currentLessonCoroutine = StartCoroutine(Lesson3_BFS());
                break;
            case 4:
                currentLessonCoroutine = StartCoroutine(Lesson4_Dijkstra());
                break;
            case 5:
                currentLessonCoroutine = StartCoroutine(Lesson5_AStar());
                break;
            case 6:
                currentLessonCoroutine = StartCoroutine(Lesson6_Prim());
                break;
        }
    }

    /// <summary>
    /// 실행 중인 코루틴 중지
    /// </summary>
    private void StopCurrentLessonIfRunning()
    {
        if (currentLessonCoroutine != null)
        {
            StopCoroutine(currentLessonCoroutine);
            currentLessonCoroutine = null;
        }
    }

    /// <summary>
    /// 스크립트가 비활성화 될 때 실행 중인 코루틴 중지
    /// </summary>
    private void OnDisable()
    {
        StopCurrentLessonIfRunning();
    }
}
