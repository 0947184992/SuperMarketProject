using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapcontroller : MonoBehaviour
{
    public List<TreeController> treeControllers = new List<TreeController>();


    private void Start()
    {
        for(int i = 0; i < treeControllers.Count; i++)
        {
            treeControllers[i].InitTree();
        }
    }
}
