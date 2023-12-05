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
    
    public struct ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579 : ICoherenceComponentData
    {
        public static uint timerMask => 0b00000000000000000000000000000001;
        public System.Single timer;
        
        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 155;
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
        
    
        public void SetSimulationFrame(AbsoluteSimulationFrame frame)
        {
            Frame = frame;
        }
        
        public AbsoluteSimulationFrame GetSimulationFrame() => Frame;
        
        public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
        {
            var other = (ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579)data;

            FieldsMask |= mask;
            StoppedMask &= ~(mask);

            if ((mask & 0x01) != 0)
            {
                Frame = other.Frame;
                timer = other.timer;
            }
            
            mask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }
        
        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }
        
        public static uint Serialize(ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579 data, uint mask, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
            
                var fieldValue = data.timer;
            

            
                bitStream.WriteFloat(fieldValue, FloatMeta.NoCompression());
            }
            
            mask >>= 1;
          
            return mask;
        }
        
        public static (ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579, uint) Deserialize(InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var mask = (uint)0;
            var val = new ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579();
            if (bitStream.ReadMask())
            {
                val.timer = bitStream.ReadFloat(FloatMeta.NoCompression());
                mask |= timerMask;
            }
                    
            val.FieldsMask = mask;
            val.StoppedMask = stoppedMask;

            return (val, mask);
        }
        
        public static (ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579, uint) DeserializeArchetypeElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579_LOD0(InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var mask = (uint)0;
            var val = new ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579();
            if (bitStream.ReadMask())
            {
                val.timer = bitStream.ReadFloat(FloatMeta.NoCompression());
                mask |= timerMask;
            }
                        
            val.FieldsMask = mask;
            val.StoppedMask = mask;
            
            return (val, mask);
        }
        
        public void ResetByteArrays(ICoherenceComponentData lastSent, uint mask)
        {
            var last = lastSent as ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579?;
            
        }

        public override string ToString()
        {
            return $"ElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_FloatingPlatform_5459872012036489579(timer: { timer }, Mask: {System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0')}), Stopped: {System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0')})";
        }
    }
    

}