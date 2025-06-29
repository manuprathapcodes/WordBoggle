﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// The class is responsible for managint the user input. 
/// </summary>
public class InputController : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    List<Tile> currentPath = new List<Tile>();

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PointerEventData ped = new PointerEventData(eventSystem);
            ped.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(ped, results);

            foreach (var result in results)
            {
                Tile t = result.gameObject.GetComponentInParent<Tile>();

                if (t != null && !currentPath.Contains(t) &&
                    (currentPath.Count == 0 || GameManager.Instance.gridMgr.IsAdjacent(t, currentPath[^1])))
                {
                    t.Highlight(true);
                    currentPath.Add(t);
                    break; 
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && currentPath.Count > 0)
        {
            GameManager.Instance.OnWordComplete(currentPath);
            foreach (var t in currentPath)
                t.Highlight(false);
            currentPath.Clear();
        }
    }
}
