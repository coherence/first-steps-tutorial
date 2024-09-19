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

    public struct _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public System.Byte isBeingCarried;
        }

        public static unsafe _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 1) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 1) " +
                    "for component with ID 155");
            }

            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 155");
            }

            var orig = new _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121();

            var comp = (Interop*)data;

            orig.isBeingCarried = comp->isBeingCarried != 0;
            orig.isBeingCarriedSimulationFrame = simFrames[0].Into();

            return orig;
        }

        public static unsafe _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121 FromInteropArchetype_27f1ac5097d4ee4409fbb87ad14f76c2__27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 1) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 1) " +
                    "for component with ID 177");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 177");
            }

            var orig = new _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121();

            var comp = (Interop*)data;

            orig.isBeingCarried = comp->isBeingCarried != 0;
            orig.isBeingCarriedSimulationFrame = simFrames[0].Into();

            return orig;
        }

        public static uint isBeingCarriedMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame isBeingCarriedSimulationFrame;
        public System.Boolean isBeingCarried;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 155;
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

            simulationFrames[0] = isBeingCarriedSimulationFrame;

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

            if ((FieldsMask & _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121.isBeingCarriedMask) != 0 && (min == null || this.isBeingCarriedSimulationFrame < min))
            {
                min = this.isBeingCarriedSimulationFrame;
            }

            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (_27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                this.isBeingCarriedSimulationFrame = other.isBeingCarriedSimulationFrame;
                this.isBeingCarried = other.isBeingCarried;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(_27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
                if (isRefSimFrameValid) {
                    var simFrameDelta = data.isBeingCarriedSimulationFrame - referenceSimulationFrame;
                    if (simFrameDelta > byte.MaxValue) {
                        simFrameDelta = byte.MaxValue;
                    }

                    SerializeTools.WriteFieldSimFrameDelta(bitStream, (byte)simFrameDelta);
                } else {
                    SerializeTools.WriteFieldSimFrameDelta(bitStream, 0);
                }


                var fieldValue = data.isBeingCarried;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;

            return mask;
        }

        public static _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121();
            if (bitStream.ReadMask())
            {
                val.isBeingCarriedSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.isBeingCarried = bitStream.ReadBool();
                val.FieldsMask |= _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121.isBeingCarriedMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public static _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121 DeserializeArchetype_27f1ac5097d4ee4409fbb87ad14f76c2__27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121();
            if (bitStream.ReadMask())
            {
                val.isBeingCarriedSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.isBeingCarried = bitStream.ReadBool();
                val.FieldsMask |= isBeingCarriedMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public override string ToString()
        {
            return $"_27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121(" +
                $" isBeingCarried: { this.isBeingCarried }" +
                $", isBeingCarriedSimFrame: { this.isBeingCarriedSimulationFrame }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0') })";
        }
    }


}