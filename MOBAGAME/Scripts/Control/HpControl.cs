using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpControl : MonoBehaviour
{
    /// <summary>
    /// Ѫ��
    /// </summary>
    [SerializeField]
    private Slider barHp;
    /// <summary>
    /// ������ɫ
    /// </summary>
    [SerializeField]
    private Image imgFill;

    /// <summary>
    /// ������ɫ
    /// </summary>
    public void SetColor(bool friend)
    {
        imgFill.color = friend ? Color.green : Color.red;
    }

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    /// <param name="value">Ѫ���İ׷ֱ�</param>
    public void SetHp(float value)
    {
        barHp.value = value;
    }

    void LateUpdate()
    {
        //Ѫ��ʱ���������
        transform.forward = Camera.main.transform.forward;
    }

}
