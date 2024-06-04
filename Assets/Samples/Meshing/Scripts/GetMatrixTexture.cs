using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.ARFoundation
{

    public class GetMatrixTexture : MonoBehaviour
    {

        public class TextureListType : IEquatable<TextureListType>
        {
            public Texture texture { get; set; }

            public int id { get; set; }

            public bool Equals(TextureListType other)
            {
                if (other == null) return false;
                return (this.id.Equals(other.id));
            }
        }
        public List<TextureListType> textureList = new List<TextureListType>();

        /// <summary>
        /// The camera manager from which frame information is pulled.
        /// </summary>
        ARCameraManager m_CameraManager;

        /// <summary>
        /// The camera manager from which frame information is pulled.
        /// </summary>
        /// <value>
        /// The camera manager from which frame information is pulled.
        /// </value>
        protected ARCameraManager cameraManager => m_CameraManager;

        public static Matrix4x4 transformM = new Matrix4x4();


        void Awake()
        {
            m_CameraManager = GetComponent<ARCameraManager>();
        }

        void OnEnable()
        {
            cameraManager.frameReceived += OnCameraFrameReceived;
        }

        void OnDisable()
        {
            cameraManager.frameReceived -= OnCameraFrameReceived;
        }


        void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            if (eventArgs.displayMatrix.HasValue)
            {
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        transformM[i, j] = eventArgs.displayMatrix.Value[i, j];
                    }
                }
                Debug.Log("Unity GetMatrixTexture Background displayMatrix" + transformM
                    );
            }

            // Enable background rendering when first frame is received.
            textureList.Clear();
            var count = eventArgs.textures.Count;
            for (int i = 0; i < count; ++i)
            {
                textureList.Add(new TextureListType() { texture = eventArgs.textures[i], id = eventArgs.propertyNameIds[i] });
            }

        }

        public void getDisplayTransform(ref Matrix4x4 inmatrix)
        {
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    inmatrix[i, j] = transformM[i, j];
                }
            }

            //Debug.Log("Unity ARCameraBackgroundModifed Background getDisplayTransform displayMatrix" + transformM
            //    );
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
