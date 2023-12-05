// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
    using Coherence.ProtocolDef;
    using Coherence.Serializer;
    using Coherence.Brook;
    using Coherence.Entities;
    using Coherence.Log;
    using System.Collections.Generic;
    using UnityEngine;

    public struct RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02_354c_46c2_bdb5_dc4086cefeb7 : IEntityCommand
    {
        public System.String name;
        
        public Entity Entity { get; set; }
        public MessageTarget Routing { get; set; }
        public uint Sender { get; set; }
        public uint GetComponentType() => 9;
        
        public IEntityMessage Clone()
        {
            // This is a struct, so we can safely return
            // a struct copy.
            return this;
        }
        
        public IEntityMapper.Error MapToAbsolute(IEntityMapper mapper, Coherence.Log.Logger logger)
        {
            var err = mapper.MapToAbsoluteEntity(Entity, false, out var absoluteEntity);
            if (err != IEntityMapper.Error.None)
            {
                return err;
            }
            Entity = absoluteEntity;
            return IEntityMapper.Error.None;
        }
        
        public IEntityMapper.Error MapToRelative(IEntityMapper mapper, Coherence.Log.Logger logger)
        {
            var err = mapper.MapToRelativeEntity(Entity, false, out var relativeEntity);
            if (err != IEntityMapper.Error.None)
            {
                return err;
            }
            Entity = relativeEntity;
            return IEntityMapper.Error.None;
        }

        public HashSet<Entity> GetEntityRefs() {
            return default;
        }

        public void NullEntityRefs(Entity entity) {
        }
        
        public RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02_354c_46c2_bdb5_dc4086cefeb7(
        Entity entity,
        System.String name
)
        {
            Entity = entity;
            Routing = MessageTarget.All;
            Sender = 0;
            
            this.name = name; 
        }
        
        public static void Serialize(RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02_354c_46c2_bdb5_dc4086cefeb7 commandData, IOutProtocolBitStream bitStream)
        {
            bitStream.WriteShortString(commandData.name);
        }
        
        public static RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02_354c_46c2_bdb5_dc4086cefeb7 Deserialize(IInProtocolBitStream bitStream, Entity entity, MessageTarget target)
        {
            var dataname = bitStream.ReadShortString();
    
            return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02_354c_46c2_bdb5_dc4086cefeb7()
            {
                Entity = entity,
                Routing = target,
                name = dataname
            };   
        }
    }

}