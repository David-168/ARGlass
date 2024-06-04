using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class BackgroudSceneTeture : MonoBehaviour
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

        const string k_ViewProjectTransformName = "_ViewProjectionTransform";

        /// <summary>
        /// Property ID for the shader parameter for the display transform matrix.
        /// </summary>
        static readonly int k_ViewProjectTransformId = Shader.PropertyToID(k_ViewProjectTransformName);

        static int s_CurrentMaterialIndex;
        static Dictionary<TrackableId, Material> s_FaceTracker = new Dictionary<TrackableId, Material>();
        public static List<GameObject> objMeshList = new List<GameObject>();

        GameObject obCamera;
        EyeLocator m_Eyelocator;

        public List<GameObject>getObjects()
        {
            return objMeshList;
        }
        void Update()
        {
            MeshRenderer m_meshrender = GetComponent<MeshRenderer>();
            if (m_meshrender != null)
            {
                if (m_meshrender.material != null)
                {
                    List<string> namelist =new List<string> { };
                    m_meshrender.material.GetTexturePropertyNames(namelist);
                    foreach (var ids in namelist)
                    {
                        //Debug.Log("XXXXXXUnity BackgoundSceneTexture Tag0.3"+ ids);
                    }
                    //m_meshrender.material.SetMatrix(k_ViewProjectTransformId, m_Eyelocator.getViewProjectMatrix()); ;
                }
            }
            var meshFilter = GetComponent<MeshFilter>();
            if (m_meshrender != null)
            {
                Mesh mesh = meshFilter.mesh;
                //Debug.Log("XXXXXXUnity BackgoundSceneTexture Tag0.4" + mesh.vertices[0]);
            }
            //Debug.Log("XXXXXXUnity BackgoundSceneTexture Update Tag1.0");

        }
        void Start()
        {
            Debug.Log("XXXXXXUnity BackgroudSceneTeture Start tag0");
            objMeshList.Add(gameObject);
            ARCameraBackground bkground = GetComponent<UnityEngine.XR.ARFoundation.ARCameraBackground>();
            if (bkground != null)
            {
                GetComponent<MeshRenderer>().material = bkground.material;
                //Debug.Log("Unity Start MeshRender ok");

            }
            else
                Debug.Log("Unity Start MeshRender null");

            obCamera = GameObject.FindWithTag("FaceMesh");
            m_Eyelocator = obCamera.GetComponent<EyeLocator>();
            Debug.Log("XXXXXXUnity BackgroudSceneTeture Start tag1");

        }
        void OnDestroy()
        {
            objMeshList.Remove(gameObject);
        }
    }
}