using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] Sprite icon = null;
    [SerializeField] Color32 iconColor = Color.white;
    public Sprite Icon { get => icon; }

    [SerializeField] Vector2 offsetOnMap = Vector2.zero;
    public Vector2 OffsetOnMap { get => offsetOnMap; }

    [SerializeField] bool allowRotationOnMap = false;
    public bool AllowRotationOnMap { get => allowRotationOnMap; }

    [SerializeField] bool updatePosition = false;
    public bool UpdatePosition { get => updatePosition; }

    [SerializeField] Color32 markerColor = new Color32(255, 255, 255, 255);

    // Size on Map
    // Enum -> states (Active, disabled, new, ...)

    // Start is called before the first frame update
    void Start()
    {
        Minimap.Instance.AddObjectToMap(this);
    }

    private void OnDestroy()
    {
        Minimap.Instance.RemoveObjectFromMap(this);
    }

    public Color32 GetIconColor() => iconColor;

    public void ActivateIcon()
    {
        Minimap.Instance.AddObjectToMap(this);
    }
    public void DeactivateIcon()
    {
        Minimap.Instance.RemoveObjectFromMap(this);
    }
}
