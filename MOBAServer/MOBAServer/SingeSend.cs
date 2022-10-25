using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOBAServer
{
    /// <summary>
    /// 单发消息
    /// </summary>
    public class SingeSend
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="retCode"></param>
        /// <param name="mess"></param>
        /// <param name="parameters"></param>
        public virtual void Send(MobaClient client, byte opCode, byte subCode, short retCode, string mess, params object[] parameters)
        {
            OperationResponse response = new OperationResponse();
            response.OperationCode = opCode;
            response.Parameters = new Dictionary<byte, object>();
            response[80] = subCode;
            for (int i = 0; i < parameters.Length; i++)
                response[(byte)i] = parameters[i];

            response.ReturnCode = retCode;
            response.DebugMessage = mess;

            client.SendOperationResponse(response, new SendParameters());
        }
    }
}
