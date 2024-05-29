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

    public struct _cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public Quaternion rotation;
        }

        public static unsafe _cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 16) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 16) " +
                    "for component with ID 168");
            }

            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 168");
            }

            var orig = new _cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177();

            var comp = (Interop*)data;

            orig.rotation = comp->rotation;
            orig.rotationSimulationFrame = simFrames[0].Into();

            return orig;
        }

        public static unsafe _cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177 FromInteropArchetype_cd9bcc1feead9419fac0c5981ce85c23__cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 16) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 16) " +
                    "for component with ID 200");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 200");
            }

            var orig = new _cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177();

            var comp = (Interop*)data;

            orig.rotation = comp->rotation;
            orig.rotationSimulationFrame = simFrames[0].Into();

            return orig;
        }

        public static uint rotationMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame rotationSimulationFrame;
        public Quaternion rotation;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 168;
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

            simulationFrames[0] = rotationSimulationFrame;

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

            if ((FieldsMask & rotationMask) != 0 && (min == null || rotationSimulationFrame < min))
            {
                min = rotationSimulationFrame;
            }

            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (_cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                rotationSimulationFrame = other.rotationSimulationFrame;
                rotation = other.rotation;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(_cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
                if (isRefSimFrameValid) {
                    var simFrameDelta = data.rotationSimulationFrame - referenceSimulationFrame;
                    if (simFrameDelta > byte.MaxValue) {
                        simFrameDelta = byte.MaxValue;
                    }

                    SerializeTools.WriteFieldSimFrameDelta(bitStream, (byte)simFrameDelta);
                } else {
                    SerializeTools.WriteFieldSimFrameDelta(bitStream, 0);
                }


                var fieldValue = (data.rotation.ToCoreQuaternion());



                bitStream.WriteQuaternion(fieldValue, 12);
            }

            mask >>= 1;

            return mask;
        }

        public static _cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177();
            if (bitStream.ReadMask())
            {
                val.rotationSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.rotation = bitStream.ReadQuaternion(12).ToUnityQuaternion();
                val.FieldsMask |= rotationMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public static _cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177 DeserializeArchetype_cd9bcc1feead9419fac0c5981ce85c23__cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177();
            if (bitStream.ReadMask())
            {
                val.rotationSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.rotation = bitStream.ReadQuaternion(12).ToUnityQuaternion();
                val.FieldsMask |= rotationMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public override string ToString()
        {
            return $"_cd9bcc1feead9419fac0c5981ce85c23_4582869309862079177(" +
                $" rotation: { rotation }" +
                $", rotationSimFrame: { rotationSimulationFrame }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0') })";
        }
    }


}