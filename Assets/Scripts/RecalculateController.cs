using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecalculateController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] MeshFilter filter;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }

    void Update()
    {
        filter.mesh.RecalculateBounds();
        filter.mesh.RecalculateNormals();
    }
}
