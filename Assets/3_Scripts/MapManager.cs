using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Geocoding.Utilities;
using Mapbox.Unity.Map;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class MapManager : Singleton<MapManager>,IMixedRealityPointerHandler
{
    public AbstractMap map;
    public  List<MeshFilter> tileMeshFilterList;

    protected override void Awake()
    {
        Invoke("CreateTileList", 1f);
    }

    private void Start()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
         
        tileMeshFilterList = new List<MeshFilter>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(tileMeshFilterList.Count);
        }
        
        if (Input.GetKeyDown(KeyCode.V))
        {
            float zoom = map.AbsoluteZoom + 1;
            map.UpdateMap(map.CenterLatitudeLongitude,zoom);
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            float zoom = map.AbsoluteZoom - 1;
            map.UpdateMap(map.CenterLatitudeLongitude,zoom);
        }

        if (_isManipulationStart)
        {
            // _dummyMapCenter = map.GeoToWorldPosition(map.CenterLatitudeLongitude, true);
            //
            // var mapCenter = map.GeoToWorldPosition(map.CenterLatitudeLongitude);
            //
            // var latLon = map.WorldToGeoPosition(new Vector3(_dummyMapCenter.x,
            //     mapCenter.y, _dummyMapCenter.z));
            //
            // map.UpdateMap(latLon, map.Zoom);
            // map.gameObject.transform.localPosition = Vector3.zero;
        }
    }

    public void CreateTileList()
    {
        for (int i = 0; i < map.transform.childCount; i++)
        {
            if (map.transform.GetChild(i).GetComponent<MeshFilter>() != null)
            {
                tileMeshFilterList.Add(map.transform.GetChild(i).GetComponent<MeshFilter>());
            }
        }
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Debug.Log("Change Map Position");

         // if (CoreServices.InputSystem.FocusProvider.PrimaryPointer.Result.Details.Object.layer != 6)
         // {
         //     return;
         // }

        var a = map.WorldToGeoPosition(eventData.Pointer.BaseCursor.Position);
        map.SetCenterLatitudeLongitude(a);
        map.UpdateMap();
    }

    
    private Vector3 _dummyMapCenter;

    private bool _isManipulationStart;

    public void OnManipulationStart()
    {
        _isManipulationStart = true;
        _dummyMapCenter = map.GeoToWorldPosition(map.CenterLatitudeLongitude, true);
    }
    
    public void OnManipulationEnded()
    {
        _isManipulationStart = false;
        
        var mapCenter = map.GeoToWorldPosition(map.CenterLatitudeLongitude);

        var latLon = map.WorldToGeoPosition(new Vector3(_dummyMapCenter.x,
            mapCenter.y, _dummyMapCenter.z));
        
        map.UpdateMap(latLon, map.Zoom);
        map.gameObject.transform.localPosition = Vector3.zero;
    }
}
