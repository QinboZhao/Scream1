using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node  {

    public bool _canWalk;//是否能通过
    public Vector3 _worldPos;//节点位置

    public int _girdX, _girdY;
    public int gCost;//与起始点的长度
    public int hCost;//与目标点的长度
    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node parent;

    public Node(bool CanWalk,Vector3 Postion,int x,int y)
    {
        _canWalk = CanWalk;
        _worldPos = Postion;
        _girdX = x;
        _girdY = y;
    }
   
}
