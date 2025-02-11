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

    public struct _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public ByteArray text;
        }

        public void ResetFrame(AbsoluteSimulationFrame frame)
        {
            FieldsMask |= _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066.textMask;
            textSimulationFrame = frame;
        }

        public static unsafe _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 16) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 16) " +
                    "for component with ID 166");
            }

            if (simFramesCount != 0) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 0) " +
                    "for component with ID 166");
            }

            var orig = new _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066();

            var comp = (Interop*)data;

            orig.text = comp->text.Data != null ? System.Text.Encoding.UTF8.GetString((byte*)comp->text.Data, (int)comp->text.Length) : null;

            return orig;
        }

        public static unsafe _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066 FromInteropArchetype_cd9bcc1feead9419fac0c5981ce85c23__cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 16) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 16) " +
                    "for component with ID 196");
            }

                
            if (simFramesCount != 0) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 0) " +
                    "for component with ID 196");
            }

            var orig = new _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066();

            var comp = (Interop*)data;

            orig.text = comp->text.Data != null ? System.Text.Encoding.UTF8.GetString((byte*)comp->text.Data, (int)comp->text.Length) : null;

            return orig;
        }

        public static uint textMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame textSimulationFrame;
        public System.String text;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 166;
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


        public AbsoluteSimulationFrame? GetMinSimulationFrame()
        {
            AbsoluteSimulationFrame? min = null;


            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (_cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                this.textSimulationFrame = other.textSimulationFrame;
                this.text = other.text;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(_cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 1);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {


                var fieldValue = data.text;



                bitStream.WriteShortString(fieldValue);
            }

            mask >>= 1;

            return mask;
        }

        public static _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066();
            if (bitStream.ReadMask())
            {

                val.text = bitStream.ReadShortString();
                val.FieldsMask |= _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066.textMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public static _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066 DeserializeArchetype_cd9bcc1feead9419fac0c5981ce85c23__cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(1);
            }

            var val = new _cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066();
            if (bitStream.ReadMask())
            {

                val.text = bitStream.ReadShortString();
                val.FieldsMask |= textMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public override string ToString()
        {
            return $"_cd9bcc1feead9419fac0c5981ce85c23_2783100773886260066(" +
                $" text: { this.text }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(1, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(1, '0') })";
        }
    }

}
