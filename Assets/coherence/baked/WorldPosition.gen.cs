// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
	using Coherence.ProtocolDef;
	using Coherence.Serializer;
	using Coherence.SimulationFrame;
	using Coherence.Entity;
	using Coherence.Utils;
	using Coherence.Brook;
	using Coherence.Toolkit;
	using UnityEngine;

	public struct WorldPosition : ICoherenceComponentData
	{
		public Vector3 value;

		public override string ToString()
		{
			return $"WorldPosition(value: {value})";
		}

		public uint GetComponentType() => Definition.InternalWorldPosition;

		public const int order = 0;

		public uint FieldsMask => 0b00000000000000000000000000000001;

		public int GetComponentOrder() => order;
		public bool IsSendOrdered() { return false; }

		public AbsoluteSimulationFrame Frame;
	

		public void SetSimulationFrame(AbsoluteSimulationFrame frame)
		{
			Frame = frame;
		}

		public AbsoluteSimulationFrame GetSimulationFrame() => Frame;

		public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
		{
			var other = (WorldPosition)data;
			if ((mask & 0x01) != 0)
			{
				Frame = other.Frame;
				value = other.value;
			}
			mask >>= 1;
			return this;
		}

		public uint DiffWith(ICoherenceComponentData data)
		{
			throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");

		}

		public static uint Serialize(WorldPosition data, uint mask, IOutProtocolBitStream bitStream)
		{
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				var fieldValue = (data.value.ToCoreVector3());
				Coherence.Utils.Bounds.CheckPositionForNanAndInfinity(ref fieldValue);

				bitStream.WriteVector3(fieldValue, FloatMeta.NoCompression());
			}
			mask >>= 1;

			return mask;
		}

		public static (WorldPosition, uint) Deserialize(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
	
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}
			return (val, mask);
		}
		public static (WorldPosition, uint) DeserializeArchetypeCrate_27f1ac5097d4ee4409fbb87ad14f76c2_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d))).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask);
		}
		public static (WorldPosition, uint) DeserializeArchetypeElevatorPlatform_ba50eecfd968a47c38959f27b05771b6_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d))).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask);
		}
		public static (WorldPosition, uint) DeserializeArchetypeFlower_a167402e36850884aa7ce3d374cd6c77_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d))).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask);
		}
		public static (WorldPosition, uint) DeserializeArchetypeFlowersCounter_20d53524fceab40d5a9ab892525615cb_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d))).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask);
		}
		public static (WorldPosition, uint) DeserializeArchetypePlayer_cd9bcc1feead9419fac0c5981ce85c23_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d))).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask);
		}
		public static (WorldPosition, uint) DeserializeArchetypeRobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d))).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask);
		}
		public static (WorldPosition, uint) DeserializeArchetypeRobotArm_Crate_a0e6252c4d09f4fb28257804194356b6_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d))).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask);
		}
		public static (WorldPosition, uint) DeserializeArchetypeTrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d))).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask);
		}

		/// <summary>
		/// Resets byte array references to the local array instance that is kept in the lastSentData.
		/// If the array content has changed but remains of same length, the new content is copied into the local array instance.
		/// If the array length has changed, the array is cloned and overwrites the local instance.
		/// If the array has not changed, the reference is reset to the local array instance.
		/// Otherwise, changes to other fields on the component might cause the local array instance reference to become permanently lost.
		/// </summary>
		public void ResetByteArrays(ICoherenceComponentData lastSent, uint mask)
		{
			var last = lastSent as WorldPosition?;
	
		}
	}
}