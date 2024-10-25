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

    public struct _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341 : ICoherenceComponentData
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Interop
        {
            [FieldOffset(0)]
            public System.Byte isCarryingObject;
            [FieldOffset(1)]
            public Entity grabbableObject;
        }

        public void ResetFrame(AbsoluteSimulationFrame frame)
        {
            FieldsMask |= _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341.isCarryingObjectMask;
            isCarryingObjectSimulationFrame = frame;
            FieldsMask |= _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341.grabbableObjectMask;
            grabbableObjectSimulationFrame = frame;
        }

        public static unsafe _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341 FromInterop(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 5) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 5) " +
                    "for component with ID 160");
            }

            if (simFramesCount != 2) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 2) " +
                    "for component with ID 160");
            }

            var orig = new _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341();

            var comp = (Interop*)data;

            orig.isCarryingObject = comp->isCarryingObject != 0;
            orig.isCarryingObjectSimulationFrame = simFrames[0].Into();
            orig.grabbableObject = comp->grabbableObject;
            orig.grabbableObjectSimulationFrame = simFrames[1].Into();

            return orig;
        }

        public static unsafe _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341 FromInteropArchetype_aa7ea8e0044f0964eb9c782a689ca1b1__aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341_LOD0(IntPtr data, Int32 dataSize, InteropAbsoluteSimulationFrame* simFrames, Int32 simFramesCount)
        {
            if (dataSize != 5) {
                throw new Exception($"Given data size is not equal to the struct size. ({dataSize} != 5) " +
                    "for component with ID 197");
            }

                
            if (simFramesCount != 2) {
                throw new Exception($"Given simFrames size is not equal to the expected length. ({simFramesCount} != 2) " +
                    "for component with ID 197");
            }

            var orig = new _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341();

            var comp = (Interop*)data;

            orig.isCarryingObject = comp->isCarryingObject != 0;
            orig.isCarryingObjectSimulationFrame = simFrames[0].Into();
            orig.grabbableObject = comp->grabbableObject;
            orig.grabbableObjectSimulationFrame = simFrames[1].Into();

            return orig;
        }

        public static uint isCarryingObjectMask => 0b00000000000000000000000000000001;
        public AbsoluteSimulationFrame isCarryingObjectSimulationFrame;
        public System.Boolean isCarryingObject;
        public static uint grabbableObjectMask => 0b00000000000000000000000000000010;
        public AbsoluteSimulationFrame grabbableObjectSimulationFrame;
        public Entity grabbableObject;

        public uint FieldsMask { get; set; }
        public uint StoppedMask { get; set; }
        public uint GetComponentType() => 160;
        public int PriorityLevel() => 100;
        public const int order = 0;
        public uint InitialFieldsMask() => 0b00000000000000000000000000000011;
        public bool HasFields() => true;
        public bool HasRefFields() => true;

        private long[] simulationFrames;

        public long[] GetSimulationFrames() {
            if (simulationFrames == null)
            {
                simulationFrames = new long[2];
            }

            simulationFrames[0] = isCarryingObjectSimulationFrame;
            simulationFrames[1] = grabbableObjectSimulationFrame;

            return simulationFrames;
        }

        public int GetFieldCount() => 2;


        
        public HashSet<Entity> GetEntityRefs()
        {
            return new HashSet<Entity>()
            {
                this.grabbableObject,
            };
        }

        public uint ReplaceReferences(Entity fromEntity, Entity toEntity)
        {
            uint refsMask = 0;

            if (this.grabbableObject == fromEntity)
            {
                this.grabbableObject = toEntity;
                refsMask |= 1 << 1;
            }

            FieldsMask |= refsMask;

            return refsMask;
        }
        
        public IEntityMapper.Error MapToAbsolute(IEntityMapper mapper)
        {
            Entity absoluteEntity;
            IEntityMapper.Error err;
            err = mapper.MapToAbsoluteEntity(this.grabbableObject, false, out absoluteEntity);

            if (err != IEntityMapper.Error.None)
            {
                return err;
            }

            this.grabbableObject = absoluteEntity;
            return IEntityMapper.Error.None;
        }

        public IEntityMapper.Error MapToRelative(IEntityMapper mapper)
        {
            Entity relativeEntity;
            IEntityMapper.Error err;
            // We assume that the inConnection held changes with unresolved references, so the 'createMapping=true' is
            // there only because there's a chance that the parent creation change will be processed after this one
            // meaning there's no mapping for the parent yet. This wouldn't be necessary if mapping creation would happen
            // in the clientWorld via create/destroy requests while here we would only check whether mapping exists or not.
            var createParentMapping_grabbableObject = true;
            err = mapper.MapToRelativeEntity(this.grabbableObject, createParentMapping_grabbableObject,
             out relativeEntity);

            if (err != IEntityMapper.Error.None)
            {
                return err;
            }

            this.grabbableObject = relativeEntity;
            return IEntityMapper.Error.None;
        }

        public ICoherenceComponentData Clone() => this;
        public int GetComponentOrder() => order;
        public bool IsSendOrdered() => false;


        public AbsoluteSimulationFrame? GetMinSimulationFrame()
        {
            AbsoluteSimulationFrame? min = null;

            if ((FieldsMask & _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341.isCarryingObjectMask) != 0 && (min == null || this.isCarryingObjectSimulationFrame < min))
            {
                min = this.isCarryingObjectSimulationFrame;
            }
            if ((FieldsMask & _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341.grabbableObjectMask) != 0 && (min == null || this.grabbableObjectSimulationFrame < min))
            {
                min = this.grabbableObjectSimulationFrame;
            }

            return min;
        }

        public ICoherenceComponentData MergeWith(ICoherenceComponentData data)
        {
            var other = (_aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341)data;
            var otherMask = other.FieldsMask;

            FieldsMask |= otherMask;
            StoppedMask &= ~(otherMask);

            if ((otherMask & 0x01) != 0)
            {
                this.isCarryingObjectSimulationFrame = other.isCarryingObjectSimulationFrame;
                this.isCarryingObject = other.isCarryingObject;
            }

            otherMask >>= 1;
            if ((otherMask & 0x01) != 0)
            {
                this.grabbableObjectSimulationFrame = other.grabbableObjectSimulationFrame;
                this.grabbableObject = other.grabbableObject;
            }

            otherMask >>= 1;
            StoppedMask |= other.StoppedMask;

            return this;
        }

        public uint DiffWith(ICoherenceComponentData data)
        {
            throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");
        }

        public static uint Serialize(_aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341 data, bool isRefSimFrameValid, AbsoluteSimulationFrame referenceSimulationFrame, IOutProtocolBitStream bitStream, Logger logger)
        {
            if (bitStream.WriteMask(data.StoppedMask != 0))
            {
                bitStream.WriteMaskBits(data.StoppedMask, 2);
            }

            var mask = data.FieldsMask;

            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
                if (isRefSimFrameValid) {
                    var simFrameDelta = data.isCarryingObjectSimulationFrame - referenceSimulationFrame;
                    if (simFrameDelta > byte.MaxValue) {
                        simFrameDelta = byte.MaxValue;
                    }

                    SerializeTools.WriteFieldSimFrameDelta(bitStream, (byte)simFrameDelta);
                } else {
                    SerializeTools.WriteFieldSimFrameDelta(bitStream, 0);
                }


                var fieldValue = data.isCarryingObject;



                bitStream.WriteBool(fieldValue);
            }

            mask >>= 1;
            if (bitStream.WriteMask((mask & 0x01) != 0))
            {
                if (isRefSimFrameValid) {
                    var simFrameDelta = data.grabbableObjectSimulationFrame - referenceSimulationFrame;
                    if (simFrameDelta > byte.MaxValue) {
                        simFrameDelta = byte.MaxValue;
                    }

                    SerializeTools.WriteFieldSimFrameDelta(bitStream, (byte)simFrameDelta);
                } else {
                    SerializeTools.WriteFieldSimFrameDelta(bitStream, 0);
                }


                var fieldValue = data.grabbableObject;



                bitStream.WriteEntity(fieldValue);
            }

            mask >>= 1;

            return mask;
        }

        public static _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341 Deserialize(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(2);
            }

            var val = new _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341();
            if (bitStream.ReadMask())
            {
                val.isCarryingObjectSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.isCarryingObject = bitStream.ReadBool();
                val.FieldsMask |= _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341.isCarryingObjectMask;
            }
            if (bitStream.ReadMask())
            {
                val.grabbableObjectSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.grabbableObject = bitStream.ReadEntity();
                val.FieldsMask |= _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341.grabbableObjectMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public static _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341 DeserializeArchetype_aa7ea8e0044f0964eb9c782a689ca1b1__aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341_LOD0(AbsoluteSimulationFrame referenceSimulationFrame, InProtocolBitStream bitStream)
        {
            var stoppedMask = (uint)0;
            if (bitStream.ReadMask())
            {
                stoppedMask = bitStream.ReadMaskBits(2);
            }

            var val = new _aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341();
            if (bitStream.ReadMask())
            {
                val.isCarryingObjectSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.isCarryingObject = bitStream.ReadBool();
                val.FieldsMask |= isCarryingObjectMask;
            }
            if (bitStream.ReadMask())
            {
                val.grabbableObjectSimulationFrame = referenceSimulationFrame + DeserializerTools.ReadFieldSimFrameDelta(bitStream);

                val.grabbableObject = bitStream.ReadEntity();
                val.FieldsMask |= grabbableObjectMask;
            }

            val.StoppedMask = stoppedMask;

            return val;
        }

        public override string ToString()
        {
            return $"_aa7ea8e0044f0964eb9c782a689ca1b1_4031727028522489341(" +
                $" isCarryingObject: { this.isCarryingObject }" +
                $", isCarryingObjectSimFrame: { this.isCarryingObjectSimulationFrame }" +
                $" grabbableObject: { this.grabbableObject }" +
                $", grabbableObjectSimFrame: { this.grabbableObjectSimulationFrame }" +
                $" Mask: { System.Convert.ToString(FieldsMask, 2).PadLeft(2, '0') }, " +
                $"Stopped: { System.Convert.ToString(StoppedMask, 2).PadLeft(2, '0') })";
        }
    }


}