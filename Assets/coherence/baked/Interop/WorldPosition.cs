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

    public struct WorldPosition : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public Vector3 value;
        }

        public void ResetFrame(AbsoluteSimulationFrame frame)
        {
            FieldsMask |= WorldPosition.valueMask;
            valueSimulationFrame = frame;
        }

        public static unsafe WorldPosition FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 12) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for component with ID 0");
            }

            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 0");
            }

            var orig = new WorldPosition();

            var comp = (Interop*)data;

            orig.value = comp->value;
            orig.valueSimulationFrame = simFrames[0].Into();

            return orig;
        }

        public static unsafe WorldPosition FromInteropArchetype_20d53524fceab40d5a9ab892525615cb_WorldPosition_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 12) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for component with ID 172");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 172");
            }

            var orig = new WorldPosition();

            var comp = (Interop*)data;

            orig.value = comp->value;
            orig.valueSimulationFrame = simFrames[0].Into();

            return orig;
        }
        public static unsafe WorldPosition FromInteropArchetype_27f1ac5097d4ee4409fbb87ad14f76c2_WorldPosition_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 12) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for component with ID 174");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 174");
            }

            var orig = new WorldPosition();

            var comp = (Interop*)data;

            orig.value = comp->value;
            orig.valueSimulationFrame = simFrames[0].Into();

            return orig;
        }
        public static unsafe WorldPosition FromInteropArchetype_6ba8b7030c4bf544396f864fc9dd99de_WorldPosition_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 12) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for component with ID 179");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 179");
            }

            var orig = new WorldPosition();

            var comp = (Interop*)data;

            orig.value = comp->value;
            orig.valueSimulationFrame = simFrames[0].Into();

            return orig;
        }
        public static unsafe WorldPosition FromInteropArchetype_a0e6252c4d09f4fb28257804194356b6_WorldPosition_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 12) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for component with ID 182");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 182");
            }

            var orig = new WorldPosition();

            var comp = (Interop*)data;

            orig.value = comp->value;
            orig.valueSimulationFrame = simFrames[0].Into();

            return orig;
        }
        public static unsafe WorldPosition FromInteropArchetype_a167402e36850884aa7ce3d374cd6c77_WorldPosition_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 12) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for component with ID 185");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 185");
            }

            var orig = new WorldPosition();

            var comp = (Interop*)data;

            orig.value = comp->value;
            orig.valueSimulationFrame = simFrames[0].Into();

            return orig;
        }
        public static unsafe WorldPosition FromInteropArchetype_aa7ea8e0044f0964eb9c782a689ca1b1_WorldPosition_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 12) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for component with ID 188");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 188");
            }

            var orig = new WorldPosition();

            var comp = (Interop*)data;

            orig.value = comp->value;
            orig.valueSimulationFrame = simFrames[0].Into();

            return orig;
        }
        public static unsafe WorldPosition FromInteropArchetype_ba50eecfd968a47c38959f27b05771b6_WorldPosition_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 12) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for component with ID 196");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 196");
            }

            var orig = new WorldPosition();

            var comp = (Interop*)data;

            orig.value = comp->value;
            orig.valueSimulationFrame = simFrames[0].Into();

            return orig;
        }
        public static unsafe WorldPosition FromInteropArchetype_cd9bcc1feead9419fac0c5981ce85c23_WorldPosition_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 12) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 12) " +
                    "for component with ID 198");
            }

                
            if (simFramesCount != 1) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 1) " +
                    "for component with ID 198");
            }

            var orig = new WorldPosition();

            var comp = (Interop*)data;

            orig.value = comp->value;
            orig.valueSimulationFrame = simFrames[0].Into();

            return orig;
        }

        public static uint valueMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame valueSimulationFrame;
        public Vector3 value;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 0;
        public int PriorityLevel() => 1000;
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

            simulationFrames[0] = valueSimulationFrame;

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

            if ((FieldsMask & WorldPosition.valueMask) != 0 && (min == null || this.valueSimulationFrame < min))
            {
                min = this.valueSimulationFrame;
            }

            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (WorldPosition)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                this.valueSimulationFrame = other.valueSimulationFrame;
                this.value = other.value;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(WorldPosition data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
                if (isRefSimFrameValid) {
                    var simFrameDelta = data.valueSimulationFrame - referenceSimulationFrame;
                    if (simFrameDelta > byte.MaxValue) {
                        simFrameDelta = byte.MaxValue;
                    }

                    SerializeTools.WriteFieldSimFrameDelta(bitStream, (byte)simFrameDelta);
                } else {
                    SerializeTools.WriteFieldSimFrameDelta(bitStream, 0);
                }


                var fieldValue = (data.value.ToCoreVector3());

                Coherence.Utils.Bounds.CheckPositionForNanAndInfinity(ref fieldValue, logger);

                bitStream.WriteVector3(fieldValue, FloatMeta.NoCompression());
            }

            mask >>= 1;

            return mask;
        }

        public static WorldPosition Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new WorldPosition();
            if (bitStream.ReadMask())
            {
                val.valueSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.value = bitStream.ReadVector3(FloatMeta.NoCompression()).ToUnityVector3();
                val.FieldsMask |= WorldPosition.valueMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public static WorldPosition DeserializeArchetype_20d53524fceab40d5a9ab892525615cb_WorldPosition_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new WorldPosition();
            if (bitStream.ReadMask())
            {
                val.valueSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.value = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                val.FieldsMask |= valueMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }
        public static WorldPosition DeserializeArchetype_27f1ac5097d4ee4409fbb87ad14f76c2_WorldPosition_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new WorldPosition();
            if (bitStream.ReadMask())
            {
                val.valueSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.value = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                val.FieldsMask |= valueMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }
        public static WorldPosition DeserializeArchetype_6ba8b7030c4bf544396f864fc9dd99de_WorldPosition_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new WorldPosition();
            if (bitStream.ReadMask())
            {
                val.valueSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.value = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                val.FieldsMask |= valueMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }
        public static WorldPosition DeserializeArchetype_a0e6252c4d09f4fb28257804194356b6_WorldPosition_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new WorldPosition();
            if (bitStream.ReadMask())
            {
                val.valueSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.value = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                val.FieldsMask |= valueMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }
        public static WorldPosition DeserializeArchetype_a167402e36850884aa7ce3d374cd6c77_WorldPosition_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new WorldPosition();
            if (bitStream.ReadMask())
            {
                val.valueSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.value = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                val.FieldsMask |= valueMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }
        public static WorldPosition DeserializeArchetype_aa7ea8e0044f0964eb9c782a689ca1b1_WorldPosition_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new WorldPosition();
            if (bitStream.ReadMask())
            {
                val.valueSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.value = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                val.FieldsMask |= valueMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }
        public static WorldPosition DeserializeArchetype_ba50eecfd968a47c38959f27b05771b6_WorldPosition_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new WorldPosition();
            if (bitStream.ReadMask())
            {
                val.valueSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.value = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                val.FieldsMask |= valueMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }
        public static WorldPosition DeserializeArchetype_cd9bcc1feead9419fac0c5981ce85c23_WorldPosition_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new WorldPosition();
            if (bitStream.ReadMask())
            {
                val.valueSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.value = bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d)).ToUnityVector3();
                val.FieldsMask |= valueMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public override string ToString()
        {
            return $"WorldPosition(" +
                $" value: { this.value }" +
                $", valueSimFrame: { this.valueSimulationFrame }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0') })";
        }
    }


}