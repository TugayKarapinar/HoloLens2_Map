using Geocoding;
using Geocoding.Utilities;
using UnityEngine;
using TMPro;

public class LineManager : MonoBehaviour
{
    public GameObject pointer1, pointer2;
    public LineRenderer lineRenderer;
    public TMP_Text distanceText;
    
    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, pointer1.transform.position);
        lineRenderer.SetPosition(1, pointer2.transform.position);

        distanceText.transform.position = Vector3.Lerp(pointer1.transform.position, pointer2.transform.position, 0.5f);
        distanceText.transform.position = new Vector3(distanceText.transform.position.x,
            distanceText.transform.position.y + 0.1f , distanceText.transform.position.z);

        var vec1 = MapManager.Instance.map.WorldToGeoPosition(pointer1.transform.position);
        var vec2 = MapManager.Instance.map.WorldToGeoPosition(pointer2.transform.position);

        var geoCoordinate1 = new GeoCoordinate(vec1.x, vec1.y);
        var geoCoordinate2 = new GeoCoordinate(vec2.x, vec2.y);

        var distance = GeoCoordinate.Distance(geoCoordinate1, geoCoordinate2,DistanceUnit.Kilometer);

        distanceText.text = $"{distance:F} km";
    }
}
