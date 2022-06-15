using Geocoding.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

[RequireComponent(typeof(ClippingBox))]
public class ClippingBoxController : Singleton<ClippingBoxController>
{
    public ClippingBox clippingBox;
    
    void Start()
    {
        clippingBox = gameObject.GetComponent<ClippingBox>();
        SetClippingBoxSetting();
    }

    private void SetClippingBoxSetting()
    {
        clippingBox.ClippingSide = ClippingPrimitive.Side.Outside;
    }
        
    public void AddVisible(GameObject obj)
    {
        clippingBox.AddRenderer(obj.GetComponent<Renderer>());
    }
    
    public void AddVisible(Renderer renderer)
    {
        if(clippingBox == null) return;
        clippingBox.AddRenderer(renderer);
    }
        
    public void RemoveVisible(Renderer renderer)
    {
        clippingBox.RemoveRenderer(renderer);
    }
}
