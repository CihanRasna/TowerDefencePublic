using UnityEngine;
using Vanta.Core;



namespace Vanta.UI
{

    public class SafeArea : VantaBehaviour
    {

        public delegate void SafeAreaEvent(SafeArea safeArea);
        public event SafeAreaEvent didApplySafeArea;
        
        
        
    #region Simulation
        public enum SimulationDevice
        {
            None,
            iPhoneX,
            iPhoneXsMax,
            Pixel3XL_LSL,
            Pixel3XL_LSR
        }
        
        public static SimulationDevice simulationDevice = SimulationDevice.None;

        
        private static Rect[] NSA_iPhoneX = new Rect[]
        {
            new Rect (0f, 102f / 2436f, 1f, 2202f / 2436f),  // Portrait
            new Rect (132f / 2436f, 63f / 1125f, 2172f / 2436f, 1062f / 1125f)  // Landscape
        };
        
        private static Rect[] NSA_iPhoneXsMax = new Rect[]
        {
            new Rect (0f, 102f / 2688f, 1f, 2454f / 2688f),  // Portrait
            new Rect (132f / 2688f, 63f / 1242f, 2424f / 2688f, 1179f / 1242f)  // Landscape
        };
        
        private static Rect[] NSA_Pixel3XL_LSL = new Rect[]
        {
            new Rect (0f, 0f, 1f, 2789f / 2960f),  // Portrait
            new Rect (0f, 0f, 2789f / 2960f, 1f)  // Landscape
        };

        private static Rect[] NSA_Pixel3XL_LSR = new Rect[]
        {
            new Rect (0f, 0f, 1f, 2789f / 2960f),  // Portrait
            new Rect (171f / 2960f, 0f, 2789f / 2960f, 1f)  // Landscape
        };
        
    #endregion

        
        
        private RectTransform rectTransform;
        private Rect _lastSafeArea = new Rect (0, 0, 0, 0);
        private Rect _appliedSafeArea = new Rect(0, 0, 0, 0);
        Vector2Int _lastScreenSize = new Vector2Int (0, 0);
        ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;
        
        public Rect appliedSafeArea => _appliedSafeArea;
        public Rect actualSafeArea => _lastSafeArea;
        
        [SerializeField] private bool ConformLeft = true;
        [SerializeField] private bool ConformTop = true;
        [SerializeField] private bool ConformRight = true;
        [SerializeField] private bool ConformBottom = true;
        [SerializeField] private bool Logging = false;

        
        
    #region Life Cycle
            
        private void Start()
        {
            rectTransform = GetComponent<RectTransform> ();

            if (rectTransform == null)
            {
                Debug.LogError ("Cannot apply safe area - no RectTransform found on " + name);
                Destroy(gameObject);
                return;
            }
            
            Refresh();
        }

        private void Update()
        {
            Refresh();
        }
        
    #endregion



    #region Metrics
        
        private void Refresh()
        {
            Rect safeArea = GetSafeArea();

            if (safeArea != _lastSafeArea
                || Screen.width != _lastScreenSize.x
                || Screen.height != _lastScreenSize.y
                || Screen.orientation != _lastOrientation)
            {
                _lastScreenSize.x = Screen.width;
                _lastScreenSize.y = Screen.height;
                _lastOrientation = Screen.orientation;

                ApplySafeArea(safeArea);
            }
        }

        private Rect GetSafeArea()
        {
            Rect safeArea = Screen.safeArea;

            if (Application.isEditor && simulationDevice != SimulationDevice.None)
            {
                Rect nsa = new Rect (0, 0, Screen.width, Screen.height);

                switch (simulationDevice)
                {
                    case SimulationDevice.iPhoneX:
                        if (Screen.height > Screen.width)  // Portrait
                            nsa = NSA_iPhoneX[0];
                        else  // Landscape
                            nsa = NSA_iPhoneX[1];
                        break;
                    case SimulationDevice.iPhoneXsMax:
                        if (Screen.height > Screen.width)  // Portrait
                            nsa = NSA_iPhoneXsMax[0];
                        else  // Landscape
                            nsa = NSA_iPhoneXsMax[1];
                        break;
                    case SimulationDevice.Pixel3XL_LSL:
                        if (Screen.height > Screen.width)  // Portrait
                            nsa = NSA_Pixel3XL_LSL[0];
                        else  // Landscape
                            nsa = NSA_Pixel3XL_LSL[1];
                        break;
                    case SimulationDevice.Pixel3XL_LSR:
                        if (Screen.height > Screen.width)  // Portrait
                            nsa = NSA_Pixel3XL_LSR[0];
                        else  // Landscape
                            nsa = NSA_Pixel3XL_LSR[1];
                        break;
                    default:
                        break;
                }

                safeArea = new Rect(Screen.width * nsa.x, Screen.height * nsa.y, Screen.width * nsa.width, Screen.height * nsa.height);
            }

            return safeArea;
        }

        private void ApplySafeArea(Rect r)
        {
            _lastSafeArea = r;

            if (!ConformLeft)
            {
                r.width += r.xMin;
                r.x = 0;
            }

            if (!ConformRight)
            {
                r.width += (Screen.width - r.xMax);
            }

            if (!ConformBottom)
            {
                r.height += r.yMin;
                r.y = 0;
            }

            if (!ConformTop)
            {
                r.height += (Screen.height - r.yMax);
            }
            

            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;

            _appliedSafeArea = r;

            if (Logging)
            {
                Debug.LogFormat ("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                    name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
            }
            
            didApplySafeArea?.Invoke(this);
        }
        
    #endregion
        
    }
}