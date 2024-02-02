using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCObjectPool<T> where T : Component
{
    private List<T> m_ObjectsPool;
    private int m_CurrentElementId;

    public TCObjectPool(int elementCount, T element)
    {
        m_ObjectsPool = new List<T>();
        for (int i = 0; i < elementCount; i++)
        {
            T l_Element = GameObject.Instantiate(element);
            m_ObjectsPool.Add(l_Element);
        }
        m_CurrentElementId = 0;
    }

    public T GetNextElement()
    {
        T l_Element = m_ObjectsPool[m_CurrentElementId];
        m_CurrentElementId = (m_CurrentElementId + 1) % m_ObjectsPool.Count;
        return l_Element;
    }
}
