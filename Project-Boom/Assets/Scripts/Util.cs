using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {

    public static void DisableAllMeshCollidersInChildren(GameObject obj)
    {
        //List<Transform> children = GetAllChildrenFromObject(obj);
        //foreach (Transform item in children)
        //{
        //    MeshCollider mc = item.gameObject.GetComponent<MeshCollider>();
        //    if(mc != null)
        //    {
        //        mc.enabled = false;
        //    }
        //}
        MeshCollider[] mcs = obj.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider mc in mcs)
        {
            mc.enabled = false;
        }
    }

    public static Transform getInitialParent(Transform t, string pName)
    {
        if (t.parent.name == pName)
        {
            return t;
        }
        else
        {
            return getInitialParent(t.parent, pName);
        }
    }

    public static void AddAllMeshCollidersToRenderers(GameObject obj)
    {
        //List<Transform> children = GetAllChildrenFromObject(obj);
        //foreach (Transform item in children)
        //{
        //    MeshRenderer mr = item.gameObject.GetComponent<MeshRenderer>();
        //    if (mr != null)
        //    {
        //        MeshCollider mc = mr.gameObject.GetComponent<MeshCollider>();
        //        if(mc == null)
        //        {
        //            mc.gameObject.AddComponent<MeshCollider>();
        //        }
        //    }
        //}
        MeshRenderer[] mrs = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            if (mr.gameObject.GetComponent<MeshCollider>() == null)
            {
                mr.gameObject.AddComponent<MeshCollider>();
            }
        }
    }

    public static List<Transform> GetAllChildrenFromObject(GameObject obj)
    {
        List<Transform> ts = new List<Transform>();
        ts = GetChildrenInTrans(obj.transform);

        foreach (Transform item in ts)
        {
            List<Transform> childsChildren = GetChildrenInTrans(item);
            foreach (Transform _item in childsChildren)
            {
                List<Transform> blah = GetAllChildrenFromObject(_item.gameObject);
                foreach (Transform t in blah)
                {
                    ts.Add(t);
                }

            }
        }
        return ts;
    }

    public static List<Transform> GetChildrenInTrans(Transform t)
    {
        List<Transform> _t = new List<Transform>();

        foreach (Transform child in t)
        {
            _t.Add(child);
        }
        return _t;
    }

    public static void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }
}
