// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using Coherence.ProtocolDef;
    using Coherence.Serializer;
    using Coherence.SimulationFrame;
    using Coherence.Entities;
    using Coherence.Utils;
    using Coherence.Brook;
    using Coherence.Core;
    using Logger = Coherence.Log.Logger;
    using UnityEngine;
    using Coherence.Toolkit;

    public struct _aa7ea8e0044f0964eb9c782a689ca1b1_9849676 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public System.Byte ClawsOpen;
        }

        public void ResetFrame(AbsoluteSimulationFrame frame)
        {
            FieldsMask |= _aa7ea8e0044f0964eb9c782a689ca1b1_9849676.ClawsOpenMask;
            ClawsOpenSimulationFrame = frame;
        }

        public static unsafe _aa7ea8e0044f0964eb9c782a689ca1b1_9849676 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 1) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 1) " +
                    "for component with ID 165");
            }

            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 165");
            }

            var orig = new _aa7ea8e0044f0964eb9c782a689ca1b1_9849676();

            var comp = (Interop*)data;

            orig.ClawsOpen = comp->ClawsOpen != 0;
            orig.ClawsOpenSimulationFrame = simFrames[0].Into();

            return orig;
        }

        public static unsafe _aa7ea8e0044f0964eb9c782a689ca1b1_9849676 FromInteropArchetype_aa7ea8e0044f0964eb9c782a689ca1b1__aa7ea8e0044f0964eb9c782a689ca1b1_9849676_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 1) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 1) " +
                    "for component with ID 191");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 191");
            }

            var orig = new _aa7ea8e0044f0964eb9c782a689ca1b1_9849676();

            var comp = (Interop*)data;

            orig.ClawsOpen = comp->ClawsOpen != 0;
            orig.ClawsOpenSimulationFrame = simFrames[0].Into();

            return orig;
        }

        public static uint ClawsOpenMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame ClawsOpenSimulationFrame;
        public System.Boolean ClawsOpen;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 165;
        public int PriorityLevel() => 100;
        public const int order = 0;
        public uint InitialFieldsMask() => 0b00000000000000000000000000000001;
        public bool HasFields() => true;
        public bool HasRefFields() => false;

        private long[] simulationFrames;

        public long[] GetSimulationFrames() {
            if (simulationFrames == null)
            {
                simulationFrames = new long[1];
            }

            simulationFrames[0] = ClawsOpenSimulationFrame;

            return simulationFrames;
        }

        public int GetFieldCount() => 1;


        
        public HashSet<Entity> GetEntityRefs()
        {
            return default;
        }

        public uint ReplaceReferences(Entity fromEntity, Entity toEntity)
        {
            return 0;
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


        public AbsoluteSimulationFrame? GetMinSimulationFrame()
        {
            AbsoluteSimulationFrame? min = null;

            if ((FieldsMask & _aa7ea8e0044f0964eb9c782a689ca1b1_9849676.ClawsOpenMask) != 0 && (min == null || this.ClawsOpenSimulationFrame < min))
            {
                min = this.ClawsOpenSimulationFrame;
            }

            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (_aa7ea8e0044f0964eb9c782a689ca1b1_9849676)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                this.ClawsOpenSimulationFrame = other.ClawsOpenSimulationFrame;
                this.ClawsOpen = other.ClawsOpen;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(_aa7ea8e0044f0964eb9c782a689ca1b1_9849676 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
                if (isRefSimFrameValid) {
                    var simFrameDelta = data.ClawsOpenSimulationFrame - referenceSimulationFrame;
                    if (simFrameDelta > byte.MaxValue) {
                        simFrameDelta = byte.MaxValue;
                    }

                    SerializeTools.WriteFieldSimFrameDelta(bitStream, (byte)simFrameDelta);
                } else {
                    SerializeTools.WriteFieldSimFrameDelta(bitStream, 0);
                }


                var fieldValue = data.ClawsOpen;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;

            return mask;
        }

        public static _aa7ea8e0044f0964eb9c782a689ca1b1_9849676 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _aa7ea8e0044f0964eb9c782a689ca1b1_9849676();
            if (bitStream.ReadMask())
            {
                val.ClawsOpenSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.ClawsOpen = bitStream.ReadBool();
                val.FieldsMask |= _aa7ea8e0044f0964eb9c782a689ca1b1_9849676.ClawsOpenMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public static _aa7ea8e0044f0964eb9c782a689ca1b1_9849676 DeserializeArchetype_aa7ea8e0044f0964eb9c782a689ca1b1__aa7ea8e0044f0964eb9c782a689ca1b1_9849676_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _aa7ea8e0044f0964eb9c782a689ca1b1_9849676();
            if (bitStream.ReadMask())
            {
                val.ClawsOpenSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.ClawsOpen = bitStream.ReadBool();
                val.FieldsMask |= ClawsOpenMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public override string ToString()
        {
            return $"_aa7ea8e0044f0964eb9c782a689ca1b1_9849676(" +
                $" ClawsOpen: { this.ClawsOpen }" +
                $", ClawsOpenSimFrame: { this.ClawsOpenSimulationFrame }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0') })";
        }
    }


}