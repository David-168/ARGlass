using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.ARFoundation
{

    public class SetMeshMatriixTexture : MonoBehaviour
{

        const string k_DisplayTransformName = "_UnityDisplayTransform";
        /// <summary>
        /// Property ID for the shader parameter for the display transform matrix.
        /// </summary>
        static readonly int k_DisplayTransformId = Shader.PropertyToID(k_DisplayTransformName);

        const string k_ViewProjectTransformName = "_ViewProjectionTransform";

        /// <summary>
        /// Property ID for the shader parameter for the display transform matrix.
        /// </summary>
        static readonly int k_ViewProjectTransformId = Shader.PropertyToID(k_ViewProjectTransformName);


        const string k_UVObjectToClipTransform = "_UVObjectToClipTransform";

        /// <summary>
        /// Property ID for the shader parameter for the display transform matrix.
        /// </summary>
        static readonly int k_UVObjectToClipTransformId = Shader.PropertyToID(k_UVObjectToClipTransform);


        public GameObject objmaincamera;
        Matrix4x4 transMatrix = new Matrix4x4();
        // Start is called before the first frame update
        void Start()
    {
            objmaincamera = GameObject.FindWithTag("MainCamera");
            if (objmaincamera != null)
            {
                Debug.Log("XXXXXXUnity SetMeshTexture Start tag0");
                Debug.Log("Object Name " + objmaincamera.name);
            }

        }

        // Update is called once per frame
        void Update()
    {
            Camera comMainCamera = objmaincamera.GetComponent<Camera>();

            Renderer renderertarget = GetComponent<Renderer>();
        if (renderertarget != null)
        {

                GetMatrixTexture comGetMatrixtexture = objmaincamera.GetComponent<GetMatrixTexture>();
                Debug.Log("XXXXXXUnity SetMeshTexture Update tag0.0.1");
            if (comGetMatrixtexture != null)
            {
                List<GetMatrixTexture.TextureListType> textureList = comGetMatrixtexture.textureList;
                    comGetMatrixtexture.getDisplayTransform(ref transMatrix);

                    GameObject objfacemesh = GameObject.FindWithTag("FaceMesh");

                    EyeLocator meyelocator = objfacemesh.GetComponent<EyeLocator>();
                    Matrix4x4 objviewprojectMatrix = new Matrix4x4(new Vector4(), new Vector4(), new Vector4(), new Vector4());
                    meyelocator.getViewProjectMatrix(ref objviewprojectMatrix);

                    StoreNSetMVP comStoreNSetMVP = objmaincamera.GetComponent<StoreNSetMVP>();
                    Matrix4x4 uvobjviewprojectMatrix = new Matrix4x4(new Vector4(), new Vector4(), new Vector4(), new Vector4());
                    comStoreNSetMVP.getProjectionViewMatrix(ref uvobjviewprojectMatrix);

                    {
                        //Debug.Log("XXXXXXUnity SetMeshTexture Update tag0.0.3");
                        //                                renderertarget.material.SetTexture(0, textureList.);
                        //foreach (var ids in textureList)
                        {
                        //var ids = textureIDs[1];
                        //Debug.Log("XXXXXXUnity SetMeshTexture Update tag0.0.4" + textureList[0].id+ "-" + textureList[0].texture.graphicsFormat);
                        //Debug.Log("XXXXXXUnity SetMeshTexture Update tag0.0.4" + textureList[0].id + "-" + textureList[1].texture.graphicsFormat);
                        //Debug.Log("Matrix -" + transMatrix);
                        renderertarget.material.SetTexture("_MainTex", textureList[0].texture);
                        renderertarget.material.SetTexture("_SecondTex", textureList[1].texture);
                        renderertarget.material.SetMatrix(k_DisplayTransformId, transMatrix );
                        renderertarget.material.SetMatrix(k_ViewProjectTransformId,  objviewprojectMatrix * this.transform.localToWorldMatrix);
                        renderertarget.material.SetMatrix(k_UVObjectToClipTransformId, uvobjviewprojectMatrix * this.transform.localToWorldMatrix);
                        Debug.Log("Mesh uvobjviewprojectMatrix" + uvobjviewprojectMatrix);

                        }
                    }

            }
        }

    }
}
}
