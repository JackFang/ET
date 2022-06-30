﻿using System;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectRemoveRequestHandler: AMActorRpcHandler<Scene, ObjectRemoveRequest, ObjectRemoveResponse>
    {
        protected override async ETTask Run(Scene scene, ObjectRemoveRequest request, ObjectRemoveResponse response, Action reply)
        {
            await scene.GetComponent<LocationComponent>().Remove(request.Key);

            reply();
        }
    }
}