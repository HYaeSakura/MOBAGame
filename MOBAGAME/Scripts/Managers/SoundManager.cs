using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����������
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    /// <summary>
    /// �������ű������֣�ѭ����
    /// </summary>
    [SerializeField]
    private AudioSource bgmAudioSource;

    /// <summary>
    /// ��Ч������
    /// </summary>
    [SerializeField]
    private AudioSource effectAudioSource;

    /// <summary>
    /// �ȴ��Ķ���
    /// </summary>
    private Queue<AudioClip> acEffectQue = new Queue<AudioClip>();

    void Start()
    {
        bgmAudioSource.loop = true;
        bgmAudioSource.playOnAwake = true;

        effectAudioSource.loop = false;
        effectAudioSource.playOnAwake = false;
    }

    #region ��������

    /// <summary>
    /// �������ֵ�����
    /// </summary>
    public float BGVolume
    {
        get { return bgmAudioSource.volume; }
        set { bgmAudioSource.volume = value; }
    }

    /// <summary>
    /// ���ű�������
    /// </summary>
    public void PlayBgMusic(AudioClip clip)
    {
        if (clip == null)
            return;
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    /// <summary>
    /// ֹͣ�������ֵĲ���
    /// </summary>
    public void StopBgMusic()
    {
        bgmAudioSource.clip = null;
        bgmAudioSource.Stop();
    }

    #endregion

    #region ��Ч����

    /// <summary>
    /// ������Ч����
    /// </summary>
    public void PlayEffectMusic(AudioClip clip)
    {
        if (clip == null)
            return;
        effectAudioSource.clip = clip;
        effectAudioSource.Play();
    }

    //void Update()
    //{
    //    //��� ���as���ڲ���״̬ ���� �ȴ����ŵ���Ч�ļ���������0
    //    if (!effectAudioSource.isPlaying && acEffectQue.Count > 0)
    //    {
    //        //�ȳ���ͷ��һ���ļ�
    //        AudioClip ac = acEffectQue.Dequeue();
    //        //��ʼ����
    //        effectAudioSource.clip = ac;
    //        effectAudioSource.Play();
    //    }
    //}

    /// <summary>
    /// ֹͣ��Ч���ֵĲ���
    /// </summary>
    public void StopEffectMusic()
    {
        effectAudioSource.clip = null;
        effectAudioSource.Stop();
    }

    #endregion

}
