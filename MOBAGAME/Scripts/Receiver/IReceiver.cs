using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �յ���������Ӧ�Ľ��ܽӿ�
/// </summary>
public interface IReceiver
{
    /// <summary>
    /// �յ���������Ӧ�Ľ��ܽӿ�
    /// </summary>
    /// <param name="subCode">�Ӳ���</param>
    /// <param name="response">��Ӧ����Ӧ</param>
    void OnReceive(byte subCode, OperationResponse response);
}
