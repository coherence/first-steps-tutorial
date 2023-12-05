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
    
    public struct RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676 : ICoherenceComponentData
    {
        public static uint ClawsOpenMask => 0b00000000000000000000000000000001;
        public System.Boolean ClawsOpen;
        
        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 164;
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
            var other = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676)data;

            FieldsMask |= mask;
            StoppedMask &= ~(mask);

            if ((mask & 0x01) != 0)
            {
                Frame = other.Frame;
                ClawsOpen = other.ClawsOpen;
            }
            
            mask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }
        
        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }
        
        public static uint Serialize(RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676 data, uint mask, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
            
                var fieldValue = data.ClawsOpen;
            

            
                bitStream.WriteBool(fieldValue);
            }
            
            mask >>= 1;
          
            return mask;
        }
        
        public static (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676, uint) Deserialize(InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var mask = (uint)0;
            var val = new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676();
            if (bitStream.ReadMask())
            {
                val.ClawsOpen = bitStream.ReadBool();
                mask |= ClawsOpenMask;
            }
                    
            val.FieldsMask = mask;
            val.StoppedMask = stoppedMask;

            return (val, mask);
        }
        
        public static (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676, uint) DeserializeArchetypeRobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676_LOD0(InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var mask = (uint)0;
            var val = new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676();
            if (bitStream.ReadMask())
            {
                val.ClawsOpen = bitStream.ReadBool();
                mask |= ClawsOpenMask;
            }
                        
            val.FieldsMask = mask;
            val.StoppedMask = mask;
            
            return (val, mask);
        }
        
        public void ResetByteArrays(ICoherenceComponentData lastSent, uint mask)
        {
            var last = lastSent as RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676?;
            
        }

        public override string ToString()
        {
            return $"RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_Animator_9849676(ClawsOpen: { ClawsOpen }, Mask: {System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0')}), Stopped: {System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0')})";
        }
    }
    

}