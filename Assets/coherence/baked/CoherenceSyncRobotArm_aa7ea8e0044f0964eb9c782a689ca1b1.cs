// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Coherence.Toolkit;
	using Coherence.Toolkit.Bindings;
	using Coherence.Entity;
	using Coherence.ProtocolDef;
	using Coherence.Brook;
	using Coherence.Toolkit.Bindings.ValueBindings;
	using Coherence.Toolkit.Bindings.TransformBindings;
	using Coherence.Connection;
	using Coherence.Log;
	using Logger = Coherence.Log.Logger;
	using UnityEngine.Scripting;

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_f4bf5576_38d7_4d17_a530_79005e962dd7 : PositionBinding
	{
		public override string CoherenceComponentName => "WorldPosition";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override Vector3 Value
		{
			get => (Vector3)(UnityEngine.Vector3)(coherenceSync.coherencePosition);
			set => coherenceSync.coherencePosition = (UnityEngine.Vector3)(value);
		}

		protected override Vector3 ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((WorldPosition)coherenceComponent).value;
			if (!coherenceSync.HasParentWithCoherenceSync)
            {
                value += floatingOriginDelta;
            }
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (WorldPosition)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.value = Value;
			}
			else 
			{
				update.value = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new WorldPosition();
		}
	}

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_f0b6f7a4_f60f_4261_8fcd_91306e2a080c : BoolBinding
	{
		private global::RobotArmHand CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::RobotArmHand)UnityComponent;
		}
		public override string CoherenceComponentName => "RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_RobotArmHand_4031727028522489341";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override bool Value
		{
			get => (bool)(System.Boolean)(CastedUnityComponent.isCarryingObject);
			set => CastedUnityComponent.isCarryingObject = (System.Boolean)(value);
		}

		protected override bool ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_RobotArmHand_4031727028522489341)coherenceComponent).isCarryingObject;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_RobotArmHand_4031727028522489341)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.isCarryingObject = Value;
			}
			else 
			{
				update.isCarryingObject = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_RobotArmHand_4031727028522489341();
		}
	}

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_e1f85bdb_19f9_4e5b_b4ab_e734f041458e : ReferenceBinding
	{
		private global::RobotArmHand CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::RobotArmHand)UnityComponent;
		}
		public override string CoherenceComponentName => "RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_RobotArmHand_4031727028522489341";

		public override uint FieldMask => 0b00000000000000000000000000000010;

		public override SerializeEntityID Value
		{
			get => (SerializeEntityID)coherenceSync.CoherenceBridge.UnityObjectToEntityId(CastedUnityComponent.grabbableObject);
			set => CastedUnityComponent.grabbableObject = coherenceSync.CoherenceBridge.EntityIdToTransform(value);
		}

		protected override SerializeEntityID ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_RobotArmHand_4031727028522489341)coherenceComponent).grabbableObject;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_RobotArmHand_4031727028522489341)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.grabbableObject = Value;
			}
			else 
			{
				update.grabbableObject = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_RobotArmHand_4031727028522489341();
		}
	}

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_426f5b2e_aeaa_4fc9_8611_e6235ad861a0 : BoolAnimatorParameterBinding
	{
		private global::UnityEngine.Animator CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::UnityEngine.Animator)UnityComponent;
		}
		public override string CoherenceComponentName => "RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator_9849676";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override bool Value
		{
			get => (bool)(CastedUnityComponent.GetBool(CastedDescriptor.ParameterHash));
			set => CastedUnityComponent.SetBool(CastedDescriptor.ParameterHash, (value));
		}

		protected override bool ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator_9849676)coherenceComponent).ClawsOpen;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator_9849676)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.ClawsOpen = Value;
			}
			else 
			{
				update.ClawsOpen = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator_9849676();
		}
	}

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_cbae6206_d8ac_4196_92ba_095a4f325af1 : DeepRotationBinding
	{
		private global::UnityEngine.Transform CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
		}
		public override string CoherenceComponentName => "RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_3110171900669979710";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override Quaternion Value
		{
			get => (Quaternion)(UnityEngine.Quaternion)(CastedUnityComponent.localRotation);
			set => CastedUnityComponent.localRotation = (UnityEngine.Quaternion)(value);
		}

		protected override Quaternion ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_3110171900669979710)coherenceComponent).rotation;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_3110171900669979710)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.rotation = Value;
			}
			else 
			{
				update.rotation = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_3110171900669979710();
		}
	}

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_86c46aa1_94cd_456f_9435_accfc3ee162c : DeepRotationBinding
	{
		private global::UnityEngine.Transform CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
		}
		public override string CoherenceComponentName => "RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_4231088705669423063";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override Quaternion Value
		{
			get => (Quaternion)(UnityEngine.Quaternion)(CastedUnityComponent.localRotation);
			set => CastedUnityComponent.localRotation = (UnityEngine.Quaternion)(value);
		}

		protected override Quaternion ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_4231088705669423063)coherenceComponent).rotation;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_4231088705669423063)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.rotation = Value;
			}
			else 
			{
				update.rotation = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_4231088705669423063();
		}
	}

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_436cd823_fcf0_4e0b_bb18_06a6fcf85de1 : DeepPositionBinding
	{
		private global::UnityEngine.Transform CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
		}
		public override string CoherenceComponentName => "RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_511851952794508008";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override Vector3 Value
		{
			get => (Vector3)(UnityEngine.Vector3)(CastedUnityComponent.localPosition);
			set => CastedUnityComponent.localPosition = (UnityEngine.Vector3)(value);
		}

		protected override Vector3 ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_511851952794508008)coherenceComponent).position;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_511851952794508008)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.position = Value;
			}
			else 
			{
				update.position = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_511851952794508008();
		}
	}

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_8db61da3_fdb5_42de_86d0_92dcf4f2a02c : DeepPositionBinding
	{
		private global::UnityEngine.Transform CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
		}
		public override string CoherenceComponentName => "RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_6045207018197627436";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override Vector3 Value
		{
			get => (Vector3)(UnityEngine.Vector3)(CastedUnityComponent.localPosition);
			set => CastedUnityComponent.localPosition = (UnityEngine.Vector3)(value);
		}

		protected override Vector3 ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_6045207018197627436)coherenceComponent).position;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_6045207018197627436)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.position = Value;
			}
			else 
			{
				update.position = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_6045207018197627436();
		}
	}

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_c72045cb_3d2b_4ab9_a318_a1fd2ddd90e1 : DeepPositionBinding
	{
		private global::UnityEngine.Transform CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
		}
		public override string CoherenceComponentName => "RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_7591837871254792591";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override Vector3 Value
		{
			get => (Vector3)(UnityEngine.Vector3)(CastedUnityComponent.localPosition);
			set => CastedUnityComponent.localPosition = (UnityEngine.Vector3)(value);
		}

		protected override Vector3 ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_7591837871254792591)coherenceComponent).position;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_7591837871254792591)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.position = Value;
			}
			else 
			{
				update.position = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_7591837871254792591();
		}
	}

	public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_18a95da2_aa05_41df_bca2_3d72c86e037a : DeepRotationBinding
	{
		private global::UnityEngine.Transform CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
		}
		public override string CoherenceComponentName => "RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_7591837871254792591";

		public override uint FieldMask => 0b00000000000000000000000000000010;

		public override Quaternion Value
		{
			get => (Quaternion)(UnityEngine.Quaternion)(CastedUnityComponent.localRotation);
			set => CastedUnityComponent.localRotation = (UnityEngine.Quaternion)(value);
		}

		protected override Quaternion ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_7591837871254792591)coherenceComponent).rotation;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_7591837871254792591)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.rotation = Value;
			}
			else 
			{
				update.rotation = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Transform_7591837871254792591();
		}
	}


	[Preserve]
	public class CoherenceSyncRobotArm_aa7ea8e0044f0964eb9c782a689ca1b1 : CoherenceSyncBaked
	{
		private SerializeEntityID entityId;
		private Logger logger = Log.GetLogger<CoherenceSyncRobotArm_aa7ea8e0044f0964eb9c782a689ca1b1>();

		// Cached targets for commands		
		private global::UnityEngine.Animator RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7_CommandTarget;

		private IClient client;
		private CoherenceBridge bridge;

		private readonly Dictionary<string, Binding> bakedValueBindings = new Dictionary<string, Binding>()
		{
			["f4bf5576-38d7-4d17-a530-79005e962dd7"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_f4bf5576_38d7_4d17_a530_79005e962dd7(),
			["f0b6f7a4-f60f-4261-8fcd-91306e2a080c"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_f0b6f7a4_f60f_4261_8fcd_91306e2a080c(),
			["e1f85bdb-19f9-4e5b-b4ab-e734f041458e"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_e1f85bdb_19f9_4e5b_b4ab_e734f041458e(),
			["426f5b2e-aeaa-4fc9-8611-e6235ad861a0"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_426f5b2e_aeaa_4fc9_8611_e6235ad861a0(),
			["cbae6206-d8ac-4196-92ba-095a4f325af1"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_cbae6206_d8ac_4196_92ba_095a4f325af1(),
			["86c46aa1-94cd-456f-9435-accfc3ee162c"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_86c46aa1_94cd_456f_9435_accfc3ee162c(),
			["436cd823-fcf0-4e0b-bb18-06a6fcf85de1"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_436cd823_fcf0_4e0b_bb18_06a6fcf85de1(),
			["8db61da3-fdb5-42de-86d0-92dcf4f2a02c"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_8db61da3_fdb5_42de_86d0_92dcf4f2a02c(),
			["c72045cb-3d2b-4ab9-a318-a1fd2ddd90e1"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_c72045cb_3d2b_4ab9_a318_a1fd2ddd90e1(),
			["18a95da2-aa05-41df-bca2-3d72c86e037a"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_18a95da2_aa05_41df_bca2_3d72c86e037a(),
		};

		private Dictionary<string, Action<CommandBinding, CommandsHandler>> bakedCommandBindings =
			new Dictionary<string, Action<CommandBinding, CommandsHandler>>();

		public CoherenceSyncRobotArm_aa7ea8e0044f0964eb9c782a689ca1b1()
		{
			bakedCommandBindings.Add("d935ef02-354c-46c2-bdb5-dc4086cefeb7", BakeCommandBinding_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7);
		}

		public override Binding BakeValueBinding(Binding valueBinding)
		{
			if (bakedValueBindings.TryGetValue(valueBinding.guid, out var bakedBinding))
			{
				valueBinding.CloneTo(bakedBinding);
				return bakedBinding;
			}

			return null;
		}

		public override void BakeCommandBinding(CommandBinding commandBinding, CommandsHandler commandsHandler)
		{
			if (bakedCommandBindings.TryGetValue(commandBinding.guid, out var commandBindingBaker))
			{
				commandBindingBaker.Invoke(commandBinding, commandsHandler);
			}
		}
		private void BakeCommandBinding_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7(CommandBinding commandBinding, CommandsHandler commandsHandler)
		{
			RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7_CommandTarget = (global::UnityEngine.Animator)commandBinding.UnityComponent;
			commandsHandler.AddBakedCommand("UnityEngine.Animator.SetTrigger", "(System.String)",
				SendCommand_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7, ReceiveLocalCommand_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7, MessageTarget.All, RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7_CommandTarget,false);
		}

		public override List<ICoherenceComponentData> CreateEntity(bool usesLodsAtRuntime, string archetypeName)
		{
			if (!usesLodsAtRuntime)
			{
				return null;
			}

			if (Archetypes.IndexForName.TryGetValue(archetypeName, out int archetypeIndex))
			{
				var components = new List<ICoherenceComponentData>()
				{
					new ArchetypeComponent
					{
						index = archetypeIndex
					}
				};

				return components;
			}
	
			logger.Warning($"Unable to find archetype {archetypeName} in dictionary. Please, bake manually (coherence > Bake)");

			return null;
		}

		public override void Dispose()
		{
		}

		public override void Initialize(SerializeEntityID entityId, CoherenceBridge bridge, IClient client, CoherenceInput input, Logger logger)
		{
			this.logger = logger.With<CoherenceSyncRobotArm_aa7ea8e0044f0964eb9c782a689ca1b1>();
			this.bridge = bridge;
			this.entityId = entityId;
			this.client = client;
		}
		void SendCommand_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7(MessageTarget target, object[] args)
		{
			var command = new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7();
			int i = 0;
			command.name = (string)((System.String)args[i++]);
			client.SendCommand(command, target, entityId);
		}

		void ReceiveLocalCommand_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7(MessageTarget target, object[] args)
		{
			var command = new RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7();
			int i = 0;
			command.name = (string)((System.String)args[i++]);
			ReceiveCommand_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7(command);
		}

		void ReceiveCommand_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7(RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7 command)
		{
			var target = RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7_CommandTarget;
			target.SetTrigger((System.String)(command.name));
		}

		public override void ReceiveCommand(IEntityCommand command)
		{
			switch(command)
			{
				case RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7 castedCommand:
					ReceiveCommand_RobotArm_aa7ea8e0044f0964eb9c782a689ca1b1_UnityEngine__char_46_Animator__char_46_SetTrigger_d935ef02_354c_46c2_bdb5_dc4086cefeb7(castedCommand);
					break;
				default:
					logger.Warning($"[CoherenceSyncRobotArm_aa7ea8e0044f0964eb9c782a689ca1b1] Unhandled command: {command.GetType()}.");
					break;
			}
		}
	}
}
