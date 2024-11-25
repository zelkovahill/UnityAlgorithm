using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphVisualizer : MonoBehaviour
{
    [Header("������")]
    [Tooltip("��� ������")]
    [SerializeField]
    private GameObject nodePrefab;

    [Tooltip("���� ������")]
    [SerializeField]
    private GameObject edgePrefab;


    [Header("���־� ����")]
    [Tooltip("��� ���� �⺻ ����")]
    [SerializeField]
    private float nodeSpacing = 2f;

    [Tooltip("�ִϸ��̼� �ӵ�")]
    [SerializeField]
    private float animationSpeed = 1f;

    [Header("UI ���۷���")]
    [Tooltip("���� ���¸� ǥ���� UI")]
    [SerializeField]
    private TextMeshProUGUI lessonText;

    private Dictionary<int, GameObject> nodes;      // ��� ID �� ��� ������Ʈ ���� �迭
    private List<GameObject> edges;                 // ������ ���� ������Ʈ ���
    private List<int>[] adjList;                    // �׷����� ���� ����Ʈ ǥ��

    private Material defaultMaterial;
    private Material visitedMaterial;
    private Material currentMaterial;
    private Coroutine currentLessonCoroutine;

    public int currentLesson;
    private bool isLessonComplete = false;

    private void Start()
    {
        // �Լ� �ʱ�ȭ
        InitializeGraph();
        StartLesson(2);
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
            if (currentLesson > 1)
            {
                StopCurrentLessonIfRunning();
                StartLesson(currentLesson - 1);
            }
        }
    }


    /// <summary>
    /// �׷��� �ʱ�ȭ
    /// </summary>
    private void InitializeGraph()
    {
        nodes = new Dictionary<int, GameObject>();
        edges = new List<GameObject>();
        CreateMaterials();
    }

    /// <summary>
    /// ������ ��� ���׸��� �� ����
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
    /// ��带 �����ϰ� ���������� ǥ��
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    private void CreateNode(int id, Vector3 position)
    {
        GameObject node = Instantiate(nodePrefab, position, Quaternion.identity);
        nodes[id] = node;

        // ��� ��ȣ ǥ��
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
    /// �� ��� ���� ������ �����ϰ� ���������� ǥ��
    /// </summary>
    /// <param name="fromId">��߳��</param>
    /// <param name="toId">���� ���</param>
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
    /// �׷����� �ʱ�ȭ�ϴ� �Լ�
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

    // �ڷ�ƾ �Լ� ���

    /// <summary>
    /// �⺻ �׷��� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson1_BasicGraph()
    {
        Vector3[] nodePositions = new Vector3[]
        {
            new Vector3(0,0,2),      // ��� 0
            new Vector3(2,0,0),      // ��� 1
            new Vector3(-2,0,0),     // ��� 2    
            new Vector3(0,0,-2),     // ��� 3
        };

        // ��� ������ ����
        for (int i = 0; i < nodePositions.Length; i++)
        {
            CreateNode(i, nodePositions[i]);
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);


        int[,] edges = new int[,]
        {
            {0,1},     // ��� 0�� 1 ����
            {1,2},     // ��� 1�� 2 ����
            {2,3},     // ��� 2�� 3 ����
            {3,0},     // ��� 3�� 0 ����
        };

        // ���� ������ ����
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            CreateEdgeBetweenNodes(edges[i, 0], edges[i, 1]);
            yield return new WaitForSeconds(1f);
        }
        isLessonComplete = true;
    }


    /// <summary>
    /// ���� �켱 Ž��
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson2_DFS()
    {
        Vector3[] nodePositions = new Vector3[]
        {
            new Vector3(0,0,0),      // ��� 0
            new Vector3(2,0,2),      // ��� 1
            new Vector3(-2,0,2),     // ��� 2    
            new Vector3(2,0,-2),     // ��� 3
            new Vector3(-2,0,-2),    // ��� 4
            new Vector3(0,0,-4),     // ��� 5
        };

        // ��� ������ ����
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

        // ���� ������ ����
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            CreateEdgeBetweenNodes(edges[i, 0], edges[i, 1]);
            yield return new WaitForSeconds(1f);
        }

        // ���� ����Ʈ ����
        adjList = new List<int>[nodePositions.Length];
        for (int i = 0; i < nodePositions.Length; i++)
        {
            adjList[i] = new List<int>();
        }

        // ����� ���� �߰�
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            int from = edges[i, 0];
            int to = edges[i, 1];
            adjList[from].Add(to);
            adjList[to].Add(from);
        }

        yield return new WaitForSeconds(1f);


        // DFS ����
        bool[] visited = new bool[nodePositions.Length];
        Stack<int> stack = new Stack<int>();

        // ���� ��� 0��
        stack.Push(0);

        while (stack.Count > 0)
        {
            int current = stack.Pop();

            if (!visited[current])
            {
                visited[current] = true;
                nodes[current].GetComponent<MeshRenderer>().material = currentMaterial;
                yield return new WaitForSeconds(1f);

                // �湮 �Ϸ� ǥ��
                nodes[current].GetComponent<MeshRenderer>().material = visitedMaterial;

                // ���� ������ ���ؿ� �߰� (�������� �߰��Ͽ� ���� ��ȣ���� �湮
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
    /// �ʺ� �켱 Ž��
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson3_BFS()
    {
        yield return null;
    }

    /// <summary>
    /// ���ͽ�Ʈ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson4_Dijkstra()
    {
        yield return null;
    }

    /// <summary>
    /// ���̽�Ÿ
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson5_AStar()
    {
        yield return null;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lesson6_Prim()
    {
        yield return null;
    }


    /// <summary>
    /// Ư�� �ڷ�ƾ ����
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
    /// ���� ���� �ڷ�ƾ ����
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
    /// ��ũ��Ʈ�� ��Ȱ��ȭ �� �� ���� ���� �ڷ�ƾ ����
    /// </summary>
    private void OnDisable()
    {
        StopCurrentLessonIfRunning();
    }
}
