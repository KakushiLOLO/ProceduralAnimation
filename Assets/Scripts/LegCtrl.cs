using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegCtrl : MonoBehaviour
{ 
    //确定Hits射出射线
    public Vector3 targetsObjPos = Vector3.zero;
    public Vector3 restingPosition
    {
        get
        {
            //将点从本地坐标转换到世界坐标
            return transform.TransformPoint(targetsObjPos);
        }
    }

    public Vector3 worldTarget = Vector3.zero;
    public Transform ikTarget;
    public Transform ikHits;

    public LayerMask solidLayer;
    public float stepRadius = 0.25f;
    public AnimationCurve stepHeightCurve;
    public float stepHeightMultiplier = 0.25f;
    public float stepCooldown = 1f;
    public float stepDuration = 0.5f;
    public float lastStep = 0;
    public float OffsetPlaneDistance=1;
    void Start()
    {
        lastStep = Time.time + stepCooldown;
        //Debug.Log(lastStep + "__0");
        Step();
    }
    void Update()
    {
        UpdateIkTarget();
        
    }
    /// <summary>
    /// 插值更新IKTarget位置
    /// </summary>
    public void UpdateIkTarget()
    {
        float percent = Mathf.Clamp01((Time.time - lastStep) / stepDuration);
        //Debug.Log(Time.time + "__2");
        ikTarget.position = Vector3.Lerp(ikTarget.position, worldTarget, percent) + Vector3.up * stepHeightCurve.Evaluate(percent) * stepHeightMultiplier;
        //Debug.Log(percent + "__C:"+count++);
    }
    public void TryStep(float interval)
    {
        Invoke("Step",interval);
    }
    /// <summary>
    /// 腿移动
    /// </summary>
    public void Step()
    {
        Vector3 direction = restingPosition - ikHits.position;
        RaycastHit hit;
        if (Physics.SphereCast(ikHits.position, stepRadius, direction, out hit, direction.magnitude * 2f, solidLayer))
        {
            worldTarget = hit.point + new Vector3(0, OffsetPlaneDistance, 0);
        }
        else
        {
            worldTarget = restingPosition + new Vector3(0, OffsetPlaneDistance, 0);
        }
        lastStep = Time.time;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(ikHits.position, 0.1f);

        Vector3 direction = restingPosition - ikHits.position;
        RaycastHit hitInfo;
        if (Physics.Raycast(ikHits.position, direction, out hitInfo, direction.magnitude * 2f, solidLayer))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ikHits.position, hitInfo.point);
            Gizmos.DrawSphere(hitInfo.point, 0.1f);
        }
    }
}