using UnityEngine;

public class RayTracerLab : MonoBehaviour
{
    [SerializeField] private float _downsampling = 1.0f;

    private Texture2D _canvasTex;

    // Use this for initialization
    void Awake()
    {
        _canvasTex = new Texture2D((int)(Screen.width / _downsampling), (int)(Screen.height / _downsampling));
    }

    void Start()
    {
        UpdateRaytrace();
    }

    private void UpdateRaytrace()
    {
        int newWidth = (int) (Screen.width / _downsampling);
        if (_canvasTex == null || _canvasTex.width != newWidth)
        {
            _canvasTex = new Texture2D((int)(Screen.width / _downsampling), (int)(Screen.height / _downsampling));
            _canvasTex.filterMode = FilterMode.Point;
        }

        Camera cam = Camera.main;

        int width = _canvasTex.width, height = _canvasTex.height;
        float camZ = cam.transform.position.z;
        Color black = Color.black;

        for (float i = 0; i < width; ++i)
        {
            for (float j = 0; j < height; ++j)
            {
                Vector3 pos = new Vector3(i / width, j / height, camZ);
                Ray ray = cam.ViewportPointToRay(pos);

                RaycastHit raycastHitInfo;
                if (Physics.Raycast(ray, out raycastHitInfo))
                {
                    var comp = raycastHitInfo.collider.GetComponent<RayTraceRenderer>();
                    if (comp != null)
                        _canvasTex.SetPixel((int)i, (int)j, comp.CalculateColor(raycastHitInfo));
                    else
                        _canvasTex.SetPixel((int)i, (int)j, black);
                }
                else
                {
                    _canvasTex.SetPixel((int)i, (int)j, black);
                }
            }
        }

        _canvasTex.Apply();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
            UpdateRaytrace();
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0.0f, 0.0f, Screen.width, Screen.height), _canvasTex);
    }

#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts(1)]
    private static void HandleReload()
    {
        FindObjectOfType<RayTracerLab>().UpdateRaytrace();
    }
#endif
}
