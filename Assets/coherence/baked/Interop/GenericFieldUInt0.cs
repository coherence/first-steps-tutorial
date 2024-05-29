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

    public struct GenericFieldUInt0 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public System.UInt32 number;
        }

        public static unsafe GenericFieldUInt0 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 4) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 4) " +
                    "for component with ID 28");
            }

            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 28");
            }

            var orig = new GenericFieldUInt0();

            var comp = (Interop*)data;

            orig.number = comp->number;
            orig.numberSimulationFrame = simFrames[0].Into();

            return orig;
        }


        public static uint numberMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame numberSimulationFrame;
        public System.UInt32 number;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 28;
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

            simulationFrames[0] = numberSimulationFrame;

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

        private static readonly System.UInt32 _number_Min = 0;
        private static readonly System.UInt32 _number_Max = 4294967295;

        public AbsoluteSimulationFrame? GetMinSimulationFrame()
        {
            AbsoluteSimulationFrame? min = null;

            if ((FieldsMask & numberMask) != 0 && (min == null || numberSimulationFrame < min))
            {
                min = numberSimulationFrame;
            }

            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (GenericFieldUInt0)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                numberSimulationFrame = other.numberSimulationFrame;
                number = other.number;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(GenericFieldUInt0 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
                if (isRefSimFrameValid) {
                    var simFrameDelta = data.numberSimulationFrame - referenceSimulationFrame;
                    if (simFrameDelta > byte.MaxValue) {
                        simFrameDelta = byte.MaxValue;
                    }

                    SerializeTools.WriteFieldSimFrameDelta(bitStream, (byte)simFrameDelta);
                } else {
                    SerializeTools.WriteFieldSimFrameDelta(bitStream, 0);
                }

                Coherence.Utils.Bounds.Check(data.number, _number_Min, _number_Max, "GenericFieldUInt0.number", logger);

                data.number = Coherence.Utils.Bounds.Clamp(data.number, _number_Min, _number_Max);

                var fieldValue = data.number;



                bitStream.WriteUIntegerRange(fieldValue, 32, 0);
            }

            mask >>= 1;

            return mask;
        }

        public static GenericFieldUInt0 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new GenericFieldUInt0();
            if (bitStream.ReadMask())
            {
                val.numberSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.number = bitStream.ReadUIntegerRange(32, 0);
                val.FieldsMask |= numberMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }


        public override string ToString()
        {
            return $"GenericFieldUInt0(" +
                $" number: { number }" +
                $", numberSimFrame: { numberSimulationFrame }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0') })";
        }
    }


}