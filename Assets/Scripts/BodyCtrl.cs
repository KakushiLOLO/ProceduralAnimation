using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCtrl : MonoBehaviour
{
    public List<LegCtrl> legs;
    public float stepSize = 0.5f;
    public float stepInterval =0.2f;
    public float rotateInterval = 30.0f;
    public float hightChangeSpeed = 0.5f;
    public float highestSize = 1f;
    private Vector3 midBody = Vector3.zero;
    private Vector3 midRotate = Vector3.zero;
    private float lastHightChangeTime = 0;
    private float bodyTargetY = 1;
    // Start is called before the first frame update
    void Start()
    {
        lastHightChangeTime = Time.time;
        midBody = transform.position;
        midRotate = transform.forward;
        bodyTargetY = midBody.y;
    }

    // Update is called once per frame
    void Update()
    {
        //控制腿
        float rotateAngle = Quaternion.FromToRotation(midRotate,transform.forward).eulerAngles.magnitude;
        if(Vector3.Distance(midBody,transform.position)>stepSize||(rotateAngle>rotateInterval&&rotateAngle<360-rotateInterval))
        {
            Debug.Log(Quaternion.FromToRotation(midRotate,transform.forward).eulerAngles.magnitude);
            Vector3 moveDirecte = transform.position - midBody;
            if(Vector3.Dot(moveDirecte,transform.forward)>0)
            {
                for(int i = 0;i<legs.Count;i++)
                {
                    legs[i].TryStep(i*stepInterval);
                }
            }
            else
            {
                for(int i = legs.Count-1;i>=0;i--)
                {
                    legs[i].TryStep((legs.Count-i-1)*stepInterval);
                }
            }
            midBody = transform.position;
            midRotate = transform.forward;
        }
        //控制身體的侧歪
        float highestPos = legs[0].worldTarget.y;
        for(int i =1;i<legs.Count;i++)
        {
            if(legs[i].worldTarget.y>highestPos)
                highestPos = legs[i].worldTarget.y;
        }
        if(highestPos >= bodyTargetY)
        {
            bodyTargetY += hightChangeSpeed * Time.deltaTime;
            UpdateBodyHight();
        }
        else if(bodyTargetY - highestPos > highestSize)
        {
            bodyTargetY -= hightChangeSpeed * Time.deltaTime;
            UpdateBodyHight();
        }
    }
    /// <summary>
    /// 更新身体高度
    /// </summary>
    private void UpdateBodyHight()
    {
        Vector3 interPosition = transform.position;
        interPosition.y = bodyTargetY;
        transform.position = interPosition;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,transform.forward);
    }
}
