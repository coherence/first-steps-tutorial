// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
    using System.Collections.Generic;
    using Coherence.ProtocolDef;
    using Coherence.Serializer;
    using Coherence.SimulationFrame;
    using Coherence.Entities;
    using Coherence.Utils;
    using Coherence.Brook;
    using Logger = Coherence.Log.Logger;
    using UnityEngine;
    using Coherence.Toolkit;
    
    public struct Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033 : ICoherenceComponentData
    {
        public static uint positionMask => 0b00000000000000000000000000000001;
        public Vector3 position;
        
        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 162;
        public int PriorityLevel() => 100;
        public const int order = 0;
        public uint InitialFieldsMask() => 0b00000000000000000000000000000001;
        public bool HasFields() => true;
        public bool HasRefFields() => false;
        
        public HashSet<Entity> GetEntityRefs()
        {
            return default;
        }
        
        public IEntityMapper.Error MapToAbsolute(IEntityMapper mapper)
        {
            return IEntityMapper.Error.None;  
        }
        
        public IEntityMapper.Error MapToRelative(IEntityMapper mapper)
        {
            return IEntityMapper.Error.None;   
        }
        
        public ICoherenceComponentData Clone() => this;
        public int GetComponentOrder() => order;
        public bool IsSendOrdered() => false;
        public AbsoluteSimulationFrame Frame;
        
        private static readonly float _position_Min = -100f;
        private static readonly float _position_Max = 600f;
    
        public void SetSimulationFrame(AbsoluteSimulationFrame frame)
        {
            Frame = frame;
        }
        
        public AbsoluteSimulationFrame GetSimulationFrame() => Frame;
        
        public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
        {
            var other = (Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033)data;

            FieldsMask |= mask;
            StoppedMask &= ~(mask);

            if ((mask & 0x01) != 0)
            {
                Frame = other.Frame;
                position = other.position;
            }
            
            mask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }
        
        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }
        
        public static uint Serialize(Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033 data, uint mask, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
                Coherence.Utils.Bounds.Check(data.position.x, _position_Min, _position_Max, "Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033.position.x", logger);
                Coherence.Utils.Bounds.Check(data.position.y, _position_Min, _position_Max, "Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033.position.y", logger);
                Coherence.Utils.Bounds.Check(data.position.z, _position_Min, _position_Max, "Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033.position.z", logger);
                
            
                var fieldValue = (data.position.ToCoreVector3());
            

            
                bitStream.WriteVector3(fieldValue, FloatMeta.ForFixedPoint(-100, 600, 0.01d));
            }
            
            mask >>= 1;
          
            return mask;
        }
        
        public static (Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033, uint) Deserialize(InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var mask = (uint)0;
            var val = new Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033();
            if (bitStream.ReadMask())
            {
                val.position = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                mask |= positionMask;
            }
                    
            val.FieldsMask = mask;
            val.StoppedMask = stoppedMask;

            return (val, mask);
        }
        
        public static (Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033, uint) DeserializeArchetypePlayer_cd9bcc1feead9419fac0c5981ce85c23_Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033_LOD0(InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var mask = (uint)0;
            var val = new Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033();
            if (bitStream.ReadMask())
            {
                val.position = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                mask |= positionMask;
            }
                        
            val.FieldsMask = mask;
            val.StoppedMask = mask;
            
            return (val, mask);
        }
        
        public void ResetByteArrays(ICoherenceComponentData lastSent, uint mask)
        {
            var last = lastSent as Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033?;
            
        }

        public override string ToString()
        {
            return $"Player_cd9bcc1feead9419fac0c5981ce85c23_Transform_7791709351172572033(position: { position }, Mask: {System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0')}), Stopped: {System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0')})";
        }
    }
    

}