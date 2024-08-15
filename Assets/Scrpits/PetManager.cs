using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    public Camera Camera => Camera.main;

    /// <summary>
    /// 鼠标当前覆盖的Collider，你也可以使用3DCollider，修改一下下面的获取方法就行了
    /// </summary>
    public Collider2D HitCollider { get; private set; }

    private void Awake()
    {
        Application.runInBackground = true;
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        WindowsAPI.InitWindow();
#endif
    }

    private void Update()
    {

        HitCollider = Physics2D.OverlapPoint(Camera.ScreenToWorldPoint(Input.mousePosition));
        WindowsAPI.SetClickThrough(HitCollider);
    }


    public void ChangeModel()
    {
        //TODO: Complete the function.
    }
}