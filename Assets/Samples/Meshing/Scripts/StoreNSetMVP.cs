using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreNSetMVP : MonoBehaviour
{

    GameObject objFaceMesh;
    UnityEngine.XR.ARFoundation.EyeLocator m_Eyelocator;

    //GameObject objMainCamera;
    Camera comCamera;
    Matrix4x4 matOrgProjection = new Matrix4x4(new Vector4(), new Vector4(), new Vector4(), new Vector4());
    Matrix4x4 matOrgView = new Matrix4x4(new Vector4(), new Vector4(), new Vector4(), new Vector4());

    void copyMatrix(ref Matrix4x4 target, Matrix4x4 source)
    {
        target.m00 = source.m00;
        target.m01 = source.m01;
        target.m02 = source.m02;
        target.m03 = source.m03;
        target.m10 = source.m10;
        target.m11 = source.m11;
        target.m12 = source.m12;
        target.m13 = source.m13;
        target.m20 = source.m20;
        target.m21 = source.m21;
        target.m22 = source.m22;
        target.m23 = source.m23;
        target.m30 = source.m30;
        target.m31 = source.m31;
        target.m32 = source.m32;
        target.m33 = source.m33;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void OnPreRender()
    {
        comCamera = GetComponent<Camera>();
        copyMatrix(ref matOrgProjection, comCamera.projectionMatrix);
        copyMatrix(ref matOrgView, comCamera.worldToCameraMatrix);

        
        objFaceMesh = GameObject.FindWithTag("FaceMesh");
        m_Eyelocator = objFaceMesh.GetComponent<UnityEngine.XR.ARFoundation.EyeLocator>();

        Matrix4x4 matView = new Matrix4x4(new Vector4(), new Vector4(), new Vector4(), new Vector4());
        m_Eyelocator.getViewMatrix(ref matView);
        comCamera.worldToCameraMatrix = matView;
        GL.modelview = matView;

        Matrix4x4 matProj = new Matrix4x4(new Vector4(), new Vector4(), new Vector4(), new Vector4());

        m_Eyelocator.getProjectMatrix(ref matProj);
        Matrix4x4 matUnityFlipping = GL.GetGPUProjectionMatrix(Matrix4x4.identity, false).inverse;

        comCamera.projectionMatrix = matUnityFlipping * matProj;
        GL.LoadProjectionMatrix(matUnityFlipping * matProj);

        

    }

    public void getProjectionViewMatrix(ref Matrix4x4 target)
    {
        target = matOrgProjection * matOrgView;
        //        target = GL.GetGPUProjectionMatrix(matOrgProjection, false) * matOrgView;
        Debug.Log("StoreNSetMVP matOrgProjection" + matOrgProjection);
        Debug.Log("StoreNSetMVP matOrgView" + matOrgView);
        Debug.Log("StoreNSetMVP GL.GetGPUProjectionMatrix(matOrgProjection, false)" + GL.GetGPUProjectionMatrix(matOrgProjection, false));
    }
    void OnPostRender()
    {
        comCamera.ResetWorldToCameraMatrix();
        comCamera.ResetProjectionMatrix();
    }

}
