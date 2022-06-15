using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public GameObject pointers;
    public GameObject handMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            handMenu.SetActive(!handMenu.activeSelf);
        }
    }

    public void PointersShowHide()
    {
        pointers.SetActive(!pointers.activeSelf);
    }
    
    public void ZoomIn()
    {
        Zoom(1);
    }
    
    public void ZoomOut()
    {
        Zoom(-1);
    }

    private void Zoom(int zoom)
    {
        var map = MapManager.Instance.map;
        var zoomLevel = map.AbsoluteZoom + zoom;
        map.UpdateMap(map.CenterLatitudeLongitude,zoomLevel);
    }
}
