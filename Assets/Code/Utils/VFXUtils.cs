using UnityEngine;

public class VFXUtils
{
    public static void SetVFXSortingLayer(GameObject vfx, string layerName)
    {
        foreach (var renderer in vfx.GetComponents<Renderer>())
        {
            renderer.sortingLayerName = layerName;
        }
        foreach (var renderer in vfx.GetComponentsInChildren<Renderer>())
        {
            renderer.sortingLayerName = layerName;
        }
    }
}