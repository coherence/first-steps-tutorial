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

    public struct _20d53524fceab40d5a9ab892525615cb_5847726189716557621 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public System.Int32 count;
        }

        public void ResetFrame(AbsoluteSimulationFrame frame)
        {
            FieldsMask |= _20d53524fceab40d5a9ab892525615cb_5847726189716557621.countMask;
            countSimulationFrame = frame;
        }

        public static unsafe _20d53524fceab40d5a9ab892525615cb_5847726189716557621 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 4) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 4) " +
                    "for component with ID 153");
            }

            if (simFramesCount != 0) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 0) " +
                    "for component with ID 153");
            }

            var orig = new _20d53524fceab40d5a9ab892525615cb_5847726189716557621();

            var comp = (Interop*)data;

            orig.count = comp->count;

            return orig;
        }

        public static unsafe _20d53524fceab40d5a9ab892525615cb_5847726189716557621 FromInteropArchetype_20d53524fceab40d5a9ab892525615cb__20d53524fceab40d5a9ab892525615cb_5847726189716557621_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 4) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 4) " +
                    "for component with ID 172");
            }

                
            if (simFramesCount != 0) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 0) " +
                    "for component with ID 172");
            }

            var orig = new _20d53524fceab40d5a9ab892525615cb_5847726189716557621();

            var comp = (Interop*)data;

            orig.count = comp->count;

            return orig;
        }

        public static uint countMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame countSimulationFrame;
        public System.Int32 count;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 153;
        public int PriorityLevel() => 100;
        public const int order = 0;
        public uint InitialFieldsMask() => 0b00000000000000000000000000000001;
        public bool HasFields() => true;
        public bool HasRefFields() => false;


        public long[] GetSimulationFrames() {
            return null;
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

        private static readonly System.Int32 _count_Min = -2147483648;
        private static readonly System.Int32 _count_Max = 2147483647;

        public AbsoluteSimulationFrame? GetMinSimulationFrame()
        {
            AbsoluteSimulationFrame? min = null;


            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (_20d53524fceab40d5a9ab892525615cb_5847726189716557621)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                this.countSimulationFrame = other.countSimulationFrame;
                this.count = other.count;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(_20d53524fceab40d5a9ab892525615cb_5847726189716557621 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {

                Coherence.Utils.Bounds.Check(data.count, _count_Min, _count_Max, "_20d53524fceab40d5a9ab892525615cb_5847726189716557621.count", logger);

                data.count = Coherence.Utils.Bounds.Clamp(data.count, _count_Min, _count_Max);

                var fieldValue = data.count;



                bitStream.WriteIntegerRange(fieldValue, 32, -2147483648);
            }

            mask >>= 1;

            return mask;
        }

        public static _20d53524fceab40d5a9ab892525615cb_5847726189716557621 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _20d53524fceab40d5a9ab892525615cb_5847726189716557621();
            if (bitStream.ReadMask())
            {

                val.count = bitStream.ReadIntegerRange(32, -2147483648);
                val.FieldsMask |= _20d53524fceab40d5a9ab892525615cb_5847726189716557621.countMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public static _20d53524fceab40d5a9ab892525615cb_5847726189716557621 DeserializeArchetype_20d53524fceab40d5a9ab892525615cb__20d53524fceab40d5a9ab892525615cb_5847726189716557621_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _20d53524fceab40d5a9ab892525615cb_5847726189716557621();
            if (bitStream.ReadMask())
            {

                val.count = bitStream.ReadIntegerRange(32, -2147483648);
                val.FieldsMask |= countMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public override string ToString()
        {
            return $"_20d53524fceab40d5a9ab892525615cb_5847726189716557621(" +
                $" count: { this.count }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0') })";
        }
    }

}
