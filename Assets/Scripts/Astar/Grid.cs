using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    private Node[,] grid;
    public Vector2 gridSize;//网格大小
    public float nodeRadius;//节点半径
    public float nodeDiameter;//节点直径

    public Transform AI;//AI位置
    public LayerMask WhatLayer;//选择是否可走

    public int gridCntX, gridCntY;

    public List<Node> path = new List<Node>();//保存最终路径

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridCntX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridCntY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        grid = new Node[gridCntX,gridCntY];
        
    }

    private void CreatGrid()
    {
        //左下角起始点
        Vector3 startPoint = AI.position - gridSize.x / 2 * Vector3.right - gridSize.y / 2 * Vector3.up;
            //GameObject.FindGameObjectWithTag("AI").GetComponent<Transform>().position- gridSize.x / 2 * Vector3.right- gridSize.y / 2 * Vector3.up;
            
        
        for (int i = 0; i < gridCntX; i++)
        {
            for (int j = 0; j < gridCntY; j++)
            {
                //节点真实位置，需要位移计算
                Vector2 worldPoint = startPoint
                +Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.up * (j * nodeDiameter + nodeRadius);
               //检测是否有碰撞
                //https://forum.unity3d.com/threads/converting-physics-checksphere-worldpoint-noderadius-unwalkablemask-into-2d.324932/#post-2107658
                //Physics.CheckSphere,Physics2D.OverlapCircle
               // bool walkable = !Physics2D.OverlapBox(worldPoint,Vector2.one,.0f);
                bool walkable = !Physics2D.OverlapCircle(worldPoint,.0f);
                    //OverlayCircle(worldPoint, nodeRadius, WhatLayer);
                grid[i, j] = new Node(walkable, worldPoint, i, j);
                Debug.Log(walkable);
            }
        }
    }

    void Update()
    {
        CreatGrid();
        //OnDrawGizmos();
    }

    public Node GetFromPostion(Vector2 postion)
    {
        
        float percentX = (postion.x-AI.position.x + gridSize.x / 2)/gridSize.x;
        float percentY = (postion.y-AI.position.y + gridSize.y / 2) / gridSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridCntX -1) * percentX);
        int y = Mathf.RoundToInt((gridCntY -1) * percentY);
        //Debug.Log(x+"---"+y);
        return grid[x, y];
             
       // int cx =Mathf.RoundToInt(gridCntX/2);
        //int cy = Mathf.RoundToInt(gridCntY/2);
       // return grid[cx, cy];
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(AI.position, new Vector3(gridSize.x, gridSize.y, -10));
            //(GameObject.FindGameObjectWithTag("AI").GetComponent<Transform>().position, new Vector3(gridSize.x, gridSize.y, -10));
        if (grid == null) return;
        foreach (var node in grid)
        {
            
            Gizmos.color = node._canWalk ? Color.white : Color.red;
            Gizmos.DrawCube(node._worldPos, Vector3.one * (nodeDiameter - .1f));
        }

        Node AINode = GetFromPostion(AI.position);
        if (AINode != null )
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(AINode._worldPos, Vector3.one * (nodeDiameter - .1f));
        }

        if (path != null) 
        {
            foreach (var node in path)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(node._worldPos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    public List<Node> GetNeibourhood(Node node)
    {
        List<Node> neibourhood = new List<Node>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int tempX = node._girdX + i;
                int tempY = node._girdY + j;
                if (tempX < gridCntX && tempX > 0 && tempY < gridCntY && tempY > 0 )
                {
                    neibourhood.Add(grid[tempX,tempY]);
                }
            }
        }

        return neibourhood;
    }
}
