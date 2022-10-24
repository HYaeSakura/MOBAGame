using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paths : MonoBehaviour
{

    #region ����

    /// <summary>
    /// UI������Դ·��
    /// </summary>
    public const string RES_SOUND_UI = "Sound/UI/";
    /// <summary>
    /// ѡӢ��������·��
    /// </summary>
    public const string RES_SOUND_SELECT = "Sound/Select/";

    /// <summary>
    /// ս������������·��
    /// </summary>
    public const string RES_SOUND_FIGHT = "Sound/Fight/";

    /// <summary>
    /// ��ȡUI������Դȫ·��
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetSoundFullName(string name)
    {
        return RES_SOUND_UI + name;
    }

    #endregion

    /// <summary>
    /// UI��·��
    /// </summary>
    public const string RES_UI = "UI/";

    /// <summary>
    /// ͷ���·��
    /// </summary>
    public const string RES_HEAD = "Head/";

    /// <summary>
    /// Ӣ��Ԥ���·��
    /// </summary>
    public const string RES_HERO = "Hero/";

    /// <summary>
    /// С��Ԥ���·��
    /// </summary>
    public const string RES_DOG = "Dog/";

    /// <summary>
    /// Ұ��Ԥ���·��
    /// </summary>
    public const string RES_MONSTER = "Monster/";

    /// <summary>
    /// ����Ԥ���·��
    /// </summary>
    public const string RES_SKILL = "Skill/";
}
