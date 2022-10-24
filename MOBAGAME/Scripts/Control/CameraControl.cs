using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    //������Զ��ı߽�
    [SerializeField]
    private float X_MIN;
    [SerializeField]
    private float X_MAX;
    [SerializeField]
    private float Z_MIN;
    [SerializeField]
    private float Z_MAX;

    /// <summary>
    /// ����ƶ��ٶ�
    /// </summary>
    [SerializeField]
    private float speed = 13f;
    /// <summary>
    /// ��������
    /// </summary>
    private float area = 0.1f;

    /// <summary>
    /// �Ƿ��ǽ���
    /// </summary>
    public static bool IsFocus = true;

    void OnApplicationFocus(bool focus)
    {
        //ָ���Ƿ��ǽ���
        IsFocus = focus;
    }

    void Awake()
    {
        //�����������Ļ����
        Cursor.lockState = CursorLockMode.Confined;
    }

    void LateUpdate()
    {
        if (!IsFocus)
            return;

        //Ŀ���
        Vector3 target = Vector3.zero;

        //������������
        Vector3 mousePos = Input.mousePosition;
        float x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        float y = Mathf.Clamp(mousePos.y, 0, Screen.height);
        //����ϱ�
        if (y > Screen.height * (1 - area))
        {
            //�������ϱ����еĵط�
            target.z = 1;
        }
        else if (y < Screen.height * area)
        {
            target.z = -1;
        }
        //�������
        if (x > Screen.width * (1 - area))
        {
            target.x = 1;
        }
        else if (x < Screen.width * area)
        {
            target.x = -1;
        }

        //��ʼ�ƶ�
        //���xy ����ȣ��Ҷ���Ϊ0  ��ô���Ǿ�Ҫ�����ǵ��ٶ�ͬ��һ�� 
        if (target.x != 0 && target.z != 0)
        {
            target = target.normalized * Mathf.Max(Mathf.Abs(target.x), Mathf.Abs(target.z));
        }

        //��ʼ�ƶ�
        transform.position += target * Time.deltaTime * speed;

        //�޶�����ķ�Χ
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, X_MIN, X_MAX),
            transform.position.y,
            Mathf.Clamp(transform.position.z, Z_MIN, Z_MAX));
    }

    /// <summary>
    /// ���㵽�Լ���Ӣ��
    /// </summary>
    public void FocusOn()
    {
        if (GameData.MyControl == null)
            return;

        Vector3 hero = GameData.MyControl.transform.position;
        transform.position = new Vector3(hero.x, transform.position.y, hero.z - 5);
    }

}
