using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

namespace UnityEngine.XR.ARFoundation.Samples
{

    public class CheckEyeTracking : MonoBehaviour
{
    static bool bEyeTrackingOn = false;
        static bool bBackroundRenderTurnOff = false;

        ARCameraBackground comBackground;
        ARMeshManager comMeshManager;

        // Start is called before the first frame update
        void Start()
    {

    }

        void Awake()
        {
            var comCamera = GetComponent<XROrigin>().Camera;
            comBackground = comCamera.GetComponent<ARCameraBackground>();

            comMeshManager = GetComponentInChildren < ARMeshManager>();
        }

        // Update is called once per frame
        void Update()
    {
            if (!bEyeTrackingOn)
            {
                if(comMeshManager != null)
                if (comMeshManager.meshes.Count > 0)
                    {
                        if (comBackground != null)
                        {
                            comBackground.enabled = false;
                            bBackroundRenderTurnOff = true;
                            bEyeTrackingOn = true;
                       }
                    }
            }

        }
        public bool EyeTrackingOn()
    {
        return bEyeTrackingOn;
    }

    }
}
