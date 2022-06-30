﻿#if !UNITY
namespace ET.Server
{
    [ChildType(typeof(DBComponent))]
    public class DBManagerComponent: Entity, IAwake, IDestroy
    {
        public static DBManagerComponent Instance;
        
        public DBComponent[] DBComponents = new DBComponent[IdGenerater.MaxZone];
    }
}
#endif