using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.iOS;


namespace UnityEngine.XR.ARFoundation
{
    public class EyeLocator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Materials to use for face meshes.")]
        Material[] m_FaceMaterials;

        /// <summary>
        /// Getter/setter for the Face Materials.
        /// </summary>
        public Material[] faceMaterials
        {
            get { return m_FaceMaterials; }
            set { m_FaceMaterials = value; }
        }

        ZenPlusPlugIn.Utils myutils;

        static int s_CurrentMaterialIndex;
        static bool bFixViewPoint = false;
        static bool bUseDLL = true;
        static Dictionary<TrackableId, Material> s_FaceTracker = new Dictionary<TrackableId, Material>();
        ARFace m_Face;
        GameObject obSubCamera;
        GameObject obMainCamera;
        GameObject objScreen;
        //GameObject objMesh;

        Camera m_Camera;
        MeshFilter m_ScreenMeshFilter;
        //MeshFilter m_BackgroundMeshFilter;
        MeshRenderer m_ScreenMeshRenderer;
        MeshRenderer m_BackgroundMeshRenderer;
        Mesh meshScreen;
        Matrix4x4 objectProjectionMatrix = new Matrix4x4(new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
        Matrix4x4 objectViewMatrix = new Matrix4x4(new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
        Matrix4x4 objectWorldtoCameraMatrix = new Matrix4x4(new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
        Matrix4x4 objectLeftHandToRightHand = new Matrix4x4(new Vector4(1.0f,0.0f,0.0f,0.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, -1.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            );


        Matrix4x4 mtranslateSubCamToMainCam = Matrix4x4.Translate(new Vector3(0.009f, 0.061f, -0.01f)
            );
        //Matrix4x4 mtranslateSubCamToMainCam = Matrix4x4.Translate(new Vector3(-0.011f, 0.022f, -0.01f)
        //    );

        Matrix4x4 mtranslateScreenCenterToMainCam = Matrix4x4.Translate(new Vector3(0.00f, 0.00f, 0.0f)
            );
        //Matrix4x4 mtranslateScreenCenterToMainCam = Matrix4x4.Translate(new Vector3(-0.02f, -0.039f, 0.0f)
        //    );

        void Start()
        {
            Debug.Log("Unity EyeLocator start tag 0");
            
            m_Face = GetComponent<ARFace>();
            m_Face.updated += OnUpdated;


            Debug.Log("Unity EyeLocator start tag 00.0");
            obSubCamera = GameObject.FindWithTag("MainCamera");
                        if (m_Face != null)
                        {

                //     Debug.Log("Unity EyeLocator tag 0.0" + m_Face.leftEye.TransformPoint(0, 0, 0));
                //Debug.Log("Unity EyeLocator tag 0.1" + m_Face.transform.TransformPoint(m_Face.leftEye.TransformPoint(0, 0, 0)));

                        }
                        obMainCamera = GameObject.FindWithTag("MainCamera");
                        Debug.Log("Unity EyeLocator start tag 01");

            myutils = new ZenPlusPlugIn.Utils(SystemInfo.deviceModel);

        }

        
        void OnDestroy()
        {
            m_Face.updated -= OnUpdated;

        }

        void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
        {
            if (m_Face != null)
            {

                objectWorldtoCameraMatrix = obMainCamera.transform.worldToLocalMatrix;
                objectWorldtoCameraMatrix = Matrix4x4.Translate(obMainCamera.transform.InverseTransformVector( m_Face.rightEye.transform.position) * -1.0f)
                    * objectWorldtoCameraMatrix;

                if (bFixViewPoint == false)
                {
                     if (bUseDLL == true)
                     {
                            Debug.Log("Unity EyeLocator tag 0.1.0");
                            myutils.CalculateMatrix(obMainCamera.transform,obSubCamera.transform, m_Face.rightEye.TransformPoint(0, 0, 0));
                            Debug.Log("Unity EyeLocator tag 0.1.1");
                            objectProjectionMatrix = myutils.getProjectionMatrix();
                            Debug.Log("Unity EyeLocator tag 0.1.2");
                            Debug.Log("Frustum" + objectProjectionMatrix);

                            objectViewMatrix = myutils.getViewMatrix();
                            Debug.Log("Unity EyeLocator tag 0.1.3");
                            Debug.Log("View" + objectViewMatrix);
                     }                   
                }
            }

        }

        public void getProjectMatrix(ref Matrix4x4 matIn)
        {
            matIn.m00 = objectProjectionMatrix.m00;
            matIn.m01 = objectProjectionMatrix.m01;
            matIn.m02 = objectProjectionMatrix.m02;
            matIn.m03 = objectProjectionMatrix.m03;
            matIn.m10 = objectProjectionMatrix.m10;
            matIn.m11 = objectProjectionMatrix.m11;
            matIn.m12 = objectProjectionMatrix.m12;
            matIn.m13 = objectProjectionMatrix.m13;
            matIn.m20 = objectProjectionMatrix.m20;
            matIn.m21 = objectProjectionMatrix.m21;
            matIn.m22 = objectProjectionMatrix.m22;
            matIn.m23 = objectProjectionMatrix.m23;
            matIn.m30 = objectProjectionMatrix.m30;
            matIn.m31 = objectProjectionMatrix.m31;
            matIn.m32 = objectProjectionMatrix.m32;
            matIn.m33 = objectProjectionMatrix.m33;

            matIn =
                GL.GetGPUProjectionMatrix(Matrix4x4.identity, false) *
                matIn;
        }
        public void getViewMatrix(ref Matrix4x4 matIn)
        {
            //Debug.Log("Unity EyeLocator tag 2.1.0");
            //Debug.Log("objectViewMatrix" + objectViewMatrix);
            // matIn = objectViewMatrix;
            matIn.m00 = objectViewMatrix.m00;
            matIn.m01 = objectViewMatrix.m01;
            matIn.m02 = objectViewMatrix.m02;
            matIn.m03 = objectViewMatrix.m03;
            matIn.m10 = objectViewMatrix.m10;
            matIn.m11 = objectViewMatrix.m11;
            matIn.m12 = objectViewMatrix.m12;
            matIn.m13 = objectViewMatrix.m13;
            matIn.m20 = objectViewMatrix.m20;
            matIn.m21 = objectViewMatrix.m21;
            matIn.m22 = objectViewMatrix.m22;
            matIn.m23 = objectViewMatrix.m23;
            matIn.m30 = objectViewMatrix.m30;
            matIn.m31 = objectViewMatrix.m31;
            matIn.m32 = objectViewMatrix.m32;
            matIn.m33 = objectViewMatrix.m33;
            //Debug.Log("Unity EyeLocator tag 2.1.1");

            matIn =  objectLeftHandToRightHand * objectWorldtoCameraMatrix;

        }
        public void getViewProjectMatrix(ref Matrix4x4 output)
        {

            // Apply the inverse Matrix might change the size of the object. Odd....
            output = GL.GetGPUProjectionMatrix(Matrix4x4.identity, false)*
            objectProjectionMatrix * objectViewMatrix * objectLeftHandToRightHand * objectWorldtoCameraMatrix;
        }
        public void getRay(ref Ray output,Vector2 tapPosition)
        {
            if (bUseDLL == true)
            {
                myutils.getRay(ref output, tapPosition, obSubCamera.transform, m_Face.rightEye);
            }
        }


        void Awake()
    {

        Debug.Log("Unity EyeLocator tag 3.1");
        m_Face = GetComponent<ARFace>();

            
            obSubCamera = GameObject.FindWithTag("SubCamera");
             Debug.Log("Unity EyeLocator tag 3.2.0");
       if (m_Face != null)
        {

                //            Debug.Log("Unity EyeLocator tag 0.0" + m_Face.leftEye.TransformPoint(0, 0, 0));
                //Debug.Log("Unity EyeLocator tag 0.1" + m_Face.transform.TransformPoint(m_Face.leftEye.TransformPoint(0, 0, 0)));

            }
            obMainCamera = GameObject.FindWithTag("MainCamera");
        Debug.Log("Unity EyeLocator tag 3.3");
            
          
    }

    void OnEnable()
    {
        Debug.Log("Unity EyeLocator tag 4");
        m_Face.updated += OnUpdated;
    }

    void OnDisable()
    {
        m_Face.updated -= OnUpdated;
    }

    }
}