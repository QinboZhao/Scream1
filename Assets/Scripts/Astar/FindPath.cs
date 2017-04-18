using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour {

    private Grid _grid;

    public Transform AI;
    public Transform Endpoint; 
    
    List<Node> openSet = new List<Node>();
    HashSet<Node> closeSet = new HashSet<Node>();

	// Use this for initialization
	void Start () {
        _grid = this.GetComponent<Grid>();
	}
	
	// Update is called once per frame
	void Update () {
        
        FindingPath(AI.position, Endpoint.position);
	}

    void FindingPath(Vector3 StartPos, Vector3 EndPos)
    {
        
        Node startNode = _grid.GetFromPostion(StartPos);
        Node endNode = _grid.GetFromPostion(EndPos);
        //Debug.Log("startNode._worldPos" + startNode==null);
        //Debug.Log("endNode._worldPos" + EndPos);
        //open,close表
       
        openSet.Add(startNode);

        //Debug.Log("endNode._worldPos" + startNode._girdX);
        while (openSet.Count > 0)
        {
            //Debug.Log("endNode._worldPos" + openSet[0].fCost);
            //Debug.Log("endNode._worldPos" + openSet[0]._girdX);
            Node currentNode = openSet[0];
            //Debug.Log("hc"+currentNode.hCost);
            for (int i = 0; i < openSet.Count; i++)
            { //Debug.Log(openSet[i]);
                if (true
                    //openSet[i].fCost < currentNode.fCost || 
                    //openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost
                    )
                {
                    currentNode = openSet[i];
                }
            }
            
            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if (currentNode == endNode)
            {
                GeneratePath(startNode,endNode);
                return;
            }

            foreach (var node in _grid.GetNeibourhood(currentNode))
            {
                if (!node._canWalk || closeSet.Contains(node)) continue;
                int newCost = currentNode.gCost + GetDistanceNodes(currentNode, node);
                if (newCost < node.gCost || !openSet.Contains(node))
                {
                    node.gCost = newCost;
                    node.hCost = GetDistanceNodes(node, endNode);
                    node.parent = currentNode;
                    if (!openSet.Contains(node))
                    {
                        openSet.Add(node);
                    }
                }
            }
        }
    }

    private void GeneratePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node temp = endNode;

        while (temp != startNode)
        {
            path.Add(temp);
            temp = temp.parent;
        }
        path.Reverse();
        _grid.path = path;
    }

    int GetDistanceNodes(Node a, Node b)
    {
        int cntX = Mathf.Abs(a._girdX - b._girdX);
        int cntY = Mathf.Abs(a._girdY - b._girdY);

        if (cntX > cntY)
        { 
            return 14 * cntY + 10 * (cntX - cntY); 
        }
        else
        {
            return 14 * cntX + 10 * (cntY - cntX);
        }
    }

    
}
