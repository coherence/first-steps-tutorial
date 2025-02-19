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
    using Coherence.Entities;
    using Coherence.ProtocolDef;
    using Coherence.Brook;
    using Coherence.Toolkit.Bindings.ValueBindings;
    using Coherence.Toolkit.Bindings.TransformBindings;
    using Coherence.Connection;
    using Coherence.SimulationFrame;
    using Coherence.Interpolation;
    using Coherence.Log;
    using Logger = Coherence.Log.Logger;
    using UnityEngine.Scripting;
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_c898195464f346dfa4262f1c647fa024 : PositionBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(WorldPosition);
        public override string CoherenceComponentName => "WorldPosition";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Vector3 Value
        {
            get { return (UnityEngine.Vector3)(coherenceSync.coherencePosition); }
            set { coherenceSync.coherencePosition = (UnityEngine.Vector3)(value); }
        }

        protected override (UnityEngine.Vector3 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((WorldPosition)coherenceComponent).value;
            if (!coherenceSync.HasParentWithCoherenceSync) { value += floatingOriginDelta; }

            var simFrame = ((WorldPosition)coherenceComponent).valueSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (WorldPosition)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.value = Value;
            }
            else
            {
                update.value = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.valueSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new WorldPosition();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_b0541bd4db944c98aae3912bfbdb0f6d : RotationBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(WorldOrientation);
        public override string CoherenceComponentName => "WorldOrientation";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Quaternion Value
        {
            get { return (UnityEngine.Quaternion)(coherenceSync.coherenceRotation); }
            set { coherenceSync.coherenceRotation = (UnityEngine.Quaternion)(value); }
        }

        protected override (UnityEngine.Quaternion value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((WorldOrientation)coherenceComponent).value;

            var simFrame = ((WorldOrientation)coherenceComponent).valueSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (WorldOrientation)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.value = Value;
            }
            else
            {
                update.value = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.valueSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new WorldOrientation();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_0675d43c80e54d9e9e07fd195a2e5ff3 : ScaleBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(GenericScale);
        public override string CoherenceComponentName => "GenericScale";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Vector3 Value
        {
            get { return (UnityEngine.Vector3)(coherenceSync.coherenceLocalScale); }
            set { coherenceSync.coherenceLocalScale = (UnityEngine.Vector3)(value); }
        }

        protected override (UnityEngine.Vector3 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((GenericScale)coherenceComponent).value;

            var simFrame = ((GenericScale)coherenceComponent).valueSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (GenericScale)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.value = Value;
            }
            else
            {
                update.value = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.valueSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new GenericScale();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_8761ffc7494f4fada36741f5669ef1b9 : StringBinding
    {   
        private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410);
        public override string CoherenceComponentName => "_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.String Value
        {
            get { return (System.String)(CastedUnityComponent.path); }
            set { CastedUnityComponent.path = (System.String)(value); }
        }

        protected override (System.String value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410)coherenceComponent).path;

            var simFrame = ((_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410)coherenceComponent).pathSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.path = Value;
            }
            else
            {
                update.path = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.pathSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_8fb659bfe821436e963e8810ade5244e : IntBinding
    {   
        private global::Coherence.Toolkit.CoherenceNode CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Coherence.Toolkit.CoherenceNode)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410);
        public override string CoherenceComponentName => "_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410";
        public override uint FieldMask => 0b00000000000000000000000000000010;

        public override System.Int32 Value
        {
            get { return (System.Int32)(CastedUnityComponent.pathDirtyCounter); }
            set { CastedUnityComponent.pathDirtyCounter = (System.Int32)(value); }
        }

        protected override (System.Int32 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410)coherenceComponent).pathDirtyCounter;

            var simFrame = ((_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410)coherenceComponent).pathDirtyCounterSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.pathDirtyCounter = Value;
            }
            else
            {
                update.pathDirtyCounter = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.pathDirtyCounterSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _27f1ac5097d4ee4409fbb87ad14f76c2_4939570989761225410();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_27f1ac5097d4ee4409fbb87ad14f76c2_ccbac1d4f31d4cea96ad45265c6a5613 : BoolBinding
    {   
        private global::Coherence.FirstSteps.Grabbable CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Coherence.FirstSteps.Grabbable)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121);
        public override string CoherenceComponentName => "_27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.isBeingCarried); }
            set { CastedUnityComponent.isBeingCarried = (System.Boolean)(value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121)coherenceComponent).isBeingCarried;

            var simFrame = ((_27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121)coherenceComponent).isBeingCarriedSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.isBeingCarried = Value;
            }
            else
            {
                update.isBeingCarried = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.isBeingCarriedSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _27f1ac5097d4ee4409fbb87ad14f76c2_6525610836190113121();
        }    
    }

    [UnityEngine.Scripting.Preserve]
    public class CoherenceSync_27f1ac5097d4ee4409fbb87ad14f76c2 : CoherenceSyncBaked
    {
        private Entity entityId;
        private Logger logger = Coherence.Log.Log.GetLogger<CoherenceSync_27f1ac5097d4ee4409fbb87ad14f76c2>();
        
        
        
        private IClient client;
        private CoherenceBridge bridge;
        
        private readonly Dictionary<string, Binding> bakedValueBindings = new Dictionary<string, Binding>()
        {
            ["c898195464f346dfa4262f1c647fa024"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_c898195464f346dfa4262f1c647fa024(),
            ["b0541bd4db944c98aae3912bfbdb0f6d"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_b0541bd4db944c98aae3912bfbdb0f6d(),
            ["0675d43c80e54d9e9e07fd195a2e5ff3"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_0675d43c80e54d9e9e07fd195a2e5ff3(),
            ["8761ffc7494f4fada36741f5669ef1b9"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_8761ffc7494f4fada36741f5669ef1b9(),
            ["8fb659bfe821436e963e8810ade5244e"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_8fb659bfe821436e963e8810ade5244e(),
            ["ccbac1d4f31d4cea96ad45265c6a5613"] = new Binding_27f1ac5097d4ee4409fbb87ad14f76c2_ccbac1d4f31d4cea96ad45265c6a5613(),
        };
        
        private Dictionary<string, Action<CommandBinding, CommandsHandler>> bakedCommandBindings = new Dictionary<string, Action<CommandBinding, CommandsHandler>>();
        
        public CoherenceSync_27f1ac5097d4ee4409fbb87ad14f76c2()
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
        
        public override void ReceiveCommand(IEntityCommand command)
        {
            switch (command)
            {
                default:
                    logger.Warning(Coherence.Log.Warning.ToolkitBakedSyncReceiveCommandUnhandled,
                        $"CoherenceSync_27f1ac5097d4ee4409fbb87ad14f76c2 Unhandled command: {command.GetType()}.");
                    break;
            }
        }
        
        public override List<ICoherenceComponentData> CreateEntity(bool usesLodsAtRuntime, string archetypeName, AbsoluteSimulationFrame simFrame)
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
                        index = archetypeIndex,
                        indexSimulationFrame = simFrame,
                        FieldsMask = 0b1
                    }
                };

                return components;
            }
    
            logger.Warning(Coherence.Log.Warning.ToolkitBakedSyncCreateEntityMissingArchetype,
                $"Unable to find archetype {archetypeName} in dictionary. Please, bake manually (coherence > Bake)");
            
            return null;
        }
        
        public override void Dispose()
        {
        }
        
        public override void Initialize(Entity entityId, CoherenceBridge bridge, IClient client, CoherenceInput input, Logger logger)
        {
            this.logger = logger.With<CoherenceSync_27f1ac5097d4ee4409fbb87ad14f76c2>();
            this.bridge = bridge;
            this.entityId = entityId;
            this.client = client;        
        }
    }
}
