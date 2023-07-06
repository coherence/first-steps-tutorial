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

	public struct TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619 : ICoherenceComponentData
	{
		public Vector3 position;
		public Quaternion rotation;

		public override string ToString()
		{
			return $"TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619(position: {position}, rotation: {rotation})";
		}

		public uint GetComponentType() => Definition.InternalTrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619;

		public const int order = 0;

		public uint FieldsMask => 0b00000000000000000000000000000011;

		public int GetComponentOrder() => order;
		public bool IsSendOrdered() { return false; }

		public AbsoluteSimulationFrame Frame;
	
		private static readonly float _position_Min = -100f;
		private static readonly float _position_Max = 600f;

		public void SetSimulationFrame(AbsoluteSimulationFrame frame)
		{
			Frame = frame;
		}

		public AbsoluteSimulationFrame GetSimulationFrame() => Frame;

		public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
		{
			var other = (TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619)data;
			if ((mask & 0x01) != 0)
			{
				Frame = other.Frame;
				position = other.position;
			}
			mask >>= 1;
			if ((mask & 0x01) != 0)
			{
				Frame = other.Frame;
				rotation = other.rotation;
			}
			mask >>= 1;
			return this;
		}

		public uint DiffWith(ICoherenceComponentData data)
		{
			throw new System.NotSupportedException($"{nameof(DiffWith)} is not supported in Unity");

		}

		public static uint Serialize(TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619 data, uint mask, IOutProtocolBitStream bitStream)
		{
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				Coherence.Utils.Bounds.Check(data.position.x, _position_Min, _position_Max, "TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619.position.x");
				Coherence.Utils.Bounds.Check(data.position.y, _position_Min, _position_Max, "TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619.position.y");
				Coherence.Utils.Bounds.Check(data.position.z, _position_Min, _position_Max, "TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619.position.z");
				var fieldValue = (data.position.ToCoreVector3());

				bitStream.WriteVector3(fieldValue, FloatMeta.ForFixedPoint(-100, 600, 0.01d));
			}
			mask >>= 1;
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				var fieldValue = (data.rotation.ToCoreQuaternion());

				bitStream.WriteQuaternion(fieldValue, 12);
			}
			mask >>= 1;

			return mask;
		}

		public static (TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619, uint) Deserialize(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619();
	
			if (bitStream.ReadMask())
			{
				val.position = (bitStream.ReadVector3(FloatMeta.ForFixedPoint(-100, 600, 0.01d))).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}
			if (bitStream.ReadMask())
			{
				val.rotation = (bitStream.ReadQuaternion(12)).ToUnityQuaternion();
				mask |= 0b00000000000000000000000000000010;
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
			var last = lastSent as TrainPlatform_6ba8b7030c4bf544396f864fc9dd99de_UnityEngine__char_46_Transform_6535867841057756619?;
	
		}
	}
}