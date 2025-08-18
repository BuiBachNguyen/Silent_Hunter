using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class EmptyBlock : Block, IPointerEnterHandler, IPointerExitHandler
{
    public Action<Vector3> OnHoverEnter;
    public Action<Vector3> OnHoverExit;

    private void Start()
    {
        // Tự đăng ký vào PathFinder nếu có
        if (PathFinder.Instance != null)
        {
            //Auto submit
            OnHoverEnter += PathFinder.Instance.HandleObjectHovering;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter?.Invoke(transform.position);
        Debug.Log("Hover ENTER: " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit?.Invoke(transform.position);
        Debug.Log("Hover EXIT: " + gameObject.name);
    }
}
