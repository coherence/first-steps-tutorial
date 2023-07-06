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

	public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_c8981954_64f3_46df_a426_2f1c647fa024 : PositionBinding
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

	public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_b0541bd4_db94_4c98_aae3_912bfbdb0f6d : RotationBinding
	{
		public override string CoherenceComponentName => "WorldOrientation";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override Quaternion Value
		{
			get => (Quaternion)(UnityEngine.Quaternion)(coherenceSync.coherenceRotation);
			set => coherenceSync.coherenceRotation = (UnityEngine.Quaternion)(value);
		}

		protected override Quaternion ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((WorldOrientation)coherenceComponent).value;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (WorldOrientation)coherenceComponent;
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
			return new WorldOrientation();
		}
	}

	public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_8761ffc7_494f_4fad_a367_41f5669ef1b9 : StringBinding
	{
		private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
		}
		public override string CoherenceComponentName => "Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Coherence__char_46_Toolkit__char_46_CoherenceNode_4939570989761225410";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override string Value
		{
			get => (string)(System.String)(CastedUnityComponent.path);
			set => CastedUnityComponent.path = (System.String)(value);
		}

		protected override string ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Coherence__char_46_Toolkit__char_46_CoherenceNode_4939570989761225410)coherenceComponent).path;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Coherence__char_46_Toolkit__char_46_CoherenceNode_4939570989761225410)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.path = Value;
			}
			else 
			{
				update.path = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Coherence__char_46_Toolkit__char_46_CoherenceNode_4939570989761225410();
		}
	}

	public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_8fb659bf_e821_436e_963e_8810ade5244e : IntBinding
	{
		private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
		}
		public override string CoherenceComponentName => "Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Coherence__char_46_Toolkit__char_46_CoherenceNode_4939570989761225410";

		public override uint FieldMask => 0b00000000000000000000000000000010;

		public override int Value
		{
			get => (int)(System.Int32)(CastedUnityComponent.pathDirtyCounter);
			set => CastedUnityComponent.pathDirtyCounter = (System.Int32)(value);
		}

		protected override int ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Coherence__char_46_Toolkit__char_46_CoherenceNode_4939570989761225410)coherenceComponent).pathDirtyCounter;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Coherence__char_46_Toolkit__char_46_CoherenceNode_4939570989761225410)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.pathDirtyCounter = Value;
			}
			else 
			{
				update.pathDirtyCounter = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Coherence__char_46_Toolkit__char_46_CoherenceNode_4939570989761225410();
		}
	}

	public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_7adfb1f9_df9d_4d92_8091_2a9e293b28d5 : BoolBinding
	{
		private global::Grabbable CastedUnityComponent;

		protected override void OnBindingCloned()
		{
			CastedUnityComponent = (global::Grabbable)UnityComponent;
		}
		public override string CoherenceComponentName => "Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Grabbable_6525610836190113121";

		public override uint FieldMask => 0b00000000000000000000000000000001;

		public override bool Value
		{
			get => (bool)(System.Boolean)(CastedUnityComponent.isBeingCarried);
			set => CastedUnityComponent.isBeingCarried = (System.Boolean)(value);
		}

		protected override bool ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
		{
			var value = ((Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Grabbable_6525610836190113121)coherenceComponent).isBeingCarried;
			return value;
		}
		
		public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, double time)
		{
			var update = (Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Grabbable_6525610836190113121)coherenceComponent;
			if (RuntimeInterpolationSettings.IsInterpolationNone) 
			{
				update.isBeingCarried = Value;
			}
			else 
			{
				update.isBeingCarried = GetInterpolatedAt(time);
			}
			return update;
		}

		public override ICoherenceComponentData CreateComponentData()
		{
			return new Crate_27f1ac5097d4ee4409fbb87ad14f76c2_Grabbable_6525610836190113121();
		}
	}


	[Preserve]
	public class CoherenceSyncCrate_27f1ac5097d4ee4409fbb87ad14f76c2 : CoherenceSyncBaked
	{
		private SerializeEntityID entityId;
		private Logger logger = Log.GetLogger<CoherenceSyncCrate_27f1ac5097d4ee4409fbb87ad14f76c2>();

		// Cached targets for commands

		private IClient client;
		private CoherenceBridge bridge;

		private readonly Dictionary<string, Binding> bakedValueBindings = new Dictionary<string, Binding>()
		{
			["c8981954-64f3-46df-a426-2f1c647fa024"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_c8981954_64f3_46df_a426_2f1c647fa024(),
			["b0541bd4-db94-4c98-aae3-912bfbdb0f6d"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_b0541bd4_db94_4c98_aae3_912bfbdb0f6d(),
			["8761ffc7-494f-4fad-a367-41f5669ef1b9"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_8761ffc7_494f_4fad_a367_41f5669ef1b9(),
			["8fb659bf-e821-436e-963e-8810ade5244e"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_8fb659bf_e821_436e_963e_8810ade5244e(),
			["7adfb1f9-df9d-4d92-8091-2a9e293b28d5"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_7adfb1f9_df9d_4d92_8091_2a9e293b28d5(),
		};

		private Dictionary<string, Action<CommandBinding, CommandsHandler>> bakedCommandBindings =
			new Dictionary<string, Action<CommandBinding, CommandsHandler>>();

		public CoherenceSyncCrate_27f1ac5097d4ee4409fbb87ad14f76c2()
		{
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
			this.logger = logger.With<CoherenceSyncCrate_27f1ac5097d4ee4409fbb87ad14f76c2>();
			this.bridge = bridge;
			this.entityId = entityId;
			this.client = client;
		}

		public override void ReceiveCommand(IEntityCommand command)
		{
			switch(command)
			{
				default:
					logger.Warning($"[CoherenceSyncCrate_27f1ac5097d4ee4409fbb87ad14f76c2] Unhandled command: {command.GetType()}.");
					break;
			}
		}
	}
}
