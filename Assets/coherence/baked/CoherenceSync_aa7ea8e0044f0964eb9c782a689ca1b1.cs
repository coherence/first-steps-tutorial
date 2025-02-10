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
    public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_f4bf557638d74d17a53079005e962dd7 : PositionBinding
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
    public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_86c46aa194cd456f9435accfc3ee162c : DeepRotationBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_aa7ea8e0044f0964eb9c782a689ca1b1_4231088705669423063);
        public override string CoherenceComponentName => "_aa7ea8e0044f0964eb9c782a689ca1b1_4231088705669423063";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Quaternion Value
        {
            get { return (UnityEngine.Quaternion)(CastedUnityComponent.localRotation); }
            set { CastedUnityComponent.localRotation = (UnityEngine.Quaternion)(value); }
        }

        protected override (UnityEngine.Quaternion value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_aa7ea8e0044f0964eb9c782a689ca1b1_4231088705669423063)coherenceComponent).rotation;

            var simFrame = ((_aa7ea8e0044f0964eb9c782a689ca1b1_4231088705669423063)coherenceComponent).rotationSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_aa7ea8e0044f0964eb9c782a689ca1b1_4231088705669423063)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.rotation = Value;
            }
            else
            {
                update.rotation = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.rotationSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _aa7ea8e0044f0964eb9c782a689ca1b1_4231088705669423063();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_cbae6206d8ac419692ba095a4f325af1 : DeepRotationBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_aa7ea8e0044f0964eb9c782a689ca1b1_3110171900669979710);
        public override string CoherenceComponentName => "_aa7ea8e0044f0964eb9c782a689ca1b1_3110171900669979710";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Quaternion Value
        {
            get { return (UnityEngine.Quaternion)(CastedUnityComponent.localRotation); }
            set { CastedUnityComponent.localRotation = (UnityEngine.Quaternion)(value); }
        }

        protected override (UnityEngine.Quaternion value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_aa7ea8e0044f0964eb9c782a689ca1b1_3110171900669979710)coherenceComponent).rotation;

            var simFrame = ((_aa7ea8e0044f0964eb9c782a689ca1b1_3110171900669979710)coherenceComponent).rotationSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_aa7ea8e0044f0964eb9c782a689ca1b1_3110171900669979710)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.rotation = Value;
            }
            else
            {
                update.rotation = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.rotationSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _aa7ea8e0044f0964eb9c782a689ca1b1_3110171900669979710();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_8db61da3fdb542de86d092dcf4f2a02c : DeepPositionBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_aa7ea8e0044f0964eb9c782a689ca1b1_6045207018197627436);
        public override string CoherenceComponentName => "_aa7ea8e0044f0964eb9c782a689ca1b1_6045207018197627436";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Vector3 Value
        {
            get { return (UnityEngine.Vector3)(CastedUnityComponent.localPosition); }
            set { CastedUnityComponent.localPosition = (UnityEngine.Vector3)(value); }
        }

        protected override (UnityEngine.Vector3 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_aa7ea8e0044f0964eb9c782a689ca1b1_6045207018197627436)coherenceComponent).position;

            var simFrame = ((_aa7ea8e0044f0964eb9c782a689ca1b1_6045207018197627436)coherenceComponent).positionSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_aa7ea8e0044f0964eb9c782a689ca1b1_6045207018197627436)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.position = Value;
            }
            else
            {
                update.position = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.positionSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _aa7ea8e0044f0964eb9c782a689ca1b1_6045207018197627436();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_c72045cb3d2b4ab9a318a1fd2ddd90e1 : DeepPositionBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591);
        public override string CoherenceComponentName => "_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Vector3 Value
        {
            get { return (UnityEngine.Vector3)(CastedUnityComponent.localPosition); }
            set { CastedUnityComponent.localPosition = (UnityEngine.Vector3)(value); }
        }

        protected override (UnityEngine.Vector3 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591)coherenceComponent).position;

            var simFrame = ((_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591)coherenceComponent).positionSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.position = Value;
            }
            else
            {
                update.position = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.positionSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_18a95da2aa0541dfbca23d72c86e037a : DeepRotationBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591);
        public override string CoherenceComponentName => "_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591";
        public override uint FieldMask => 0b00000000000000000000000000000010;

        public override UnityEngine.Quaternion Value
        {
            get { return (UnityEngine.Quaternion)(CastedUnityComponent.localRotation); }
            set { CastedUnityComponent.localRotation = (UnityEngine.Quaternion)(value); }
        }

        protected override (UnityEngine.Quaternion value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591)coherenceComponent).rotation;

            var simFrame = ((_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591)coherenceComponent).rotationSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.rotation = Value;
            }
            else
            {
                update.rotation = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.rotationSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _aa7ea8e0044f0964eb9c782a689ca1b1_7591837871254792591();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_436cd823fcf04e0bbb1806a6fcf85de1 : DeepPositionBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_aa7ea8e0044f0964eb9c782a689ca1b1_511851952794508008);
        public override string CoherenceComponentName => "_aa7ea8e0044f0964eb9c782a689ca1b1_511851952794508008";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override UnityEngine.Vector3 Value
        {
            get { return (UnityEngine.Vector3)(CastedUnityComponent.localPosition); }
            set { CastedUnityComponent.localPosition = (UnityEngine.Vector3)(value); }
        }

        protected override (UnityEngine.Vector3 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_aa7ea8e0044f0964eb9c782a689ca1b1_511851952794508008)coherenceComponent).position;

            var simFrame = ((_aa7ea8e0044f0964eb9c782a689ca1b1_511851952794508008)coherenceComponent).positionSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_aa7ea8e0044f0964eb9c782a689ca1b1_511851952794508008)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.position = Value;
            }
            else
            {
                update.position = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.positionSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _aa7ea8e0044f0964eb9c782a689ca1b1_511851952794508008();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_aa7ea8e0044f0964eb9c782a689ca1b1_426f5b2eaeaa4fc98611e6235ad861a0 : BoolAnimatorParameterBinding
    {   
        private global::UnityEngine.Animator CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Animator)UnityComponent;
        }

        public override global::System.Type CoherenceComponentType => typeof(_aa7ea8e0044f0964eb9c782a689ca1b1_9849676);
        public override string CoherenceComponentName => "_aa7ea8e0044f0964eb9c782a689ca1b1_9849676";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Boolean Value
        {
            get { return (System.Boolean)(CastedUnityComponent.GetBool(CastedDescriptor.ParameterHash)); }
            set { CastedUnityComponent.SetBool(CastedDescriptor.ParameterHash, value); }
        }

        protected override (System.Boolean value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_aa7ea8e0044f0964eb9c782a689ca1b1_9849676)coherenceComponent).ClawsOpen;

            var simFrame = ((_aa7ea8e0044f0964eb9c782a689ca1b1_9849676)coherenceComponent).ClawsOpenSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_aa7ea8e0044f0964eb9c782a689ca1b1_9849676)coherenceComponent;
            if (Interpolator.IsInterpolationNone)
            {
                update.ClawsOpen = Value;
            }
            else
            {
                update.ClawsOpen = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.ClawsOpenSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _aa7ea8e0044f0964eb9c782a689ca1b1_9849676();
        }    
    }

    [UnityEngine.Scripting.Preserve]
    public class CoherenceSync_aa7ea8e0044f0964eb9c782a689ca1b1 : CoherenceSyncBaked
    {
        private Entity entityId;
        private Logger logger = Coherence.Log.Log.GetLogger<CoherenceSync_aa7ea8e0044f0964eb9c782a689ca1b1>();
        
        private global::UnityEngine.Animator _aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7_CommandTarget;
        
        
        private IClient client;
        private CoherenceBridge bridge;
        
        private readonly Dictionary<string, Binding> bakedValueBindings = new Dictionary<string, Binding>()
        {
            ["f4bf557638d74d17a53079005e962dd7"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_f4bf557638d74d17a53079005e962dd7(),
            ["86c46aa194cd456f9435accfc3ee162c"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_86c46aa194cd456f9435accfc3ee162c(),
            ["cbae6206d8ac419692ba095a4f325af1"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_cbae6206d8ac419692ba095a4f325af1(),
            ["8db61da3fdb542de86d092dcf4f2a02c"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_8db61da3fdb542de86d092dcf4f2a02c(),
            ["c72045cb3d2b4ab9a318a1fd2ddd90e1"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_c72045cb3d2b4ab9a318a1fd2ddd90e1(),
            ["18a95da2aa0541dfbca23d72c86e037a"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_18a95da2aa0541dfbca23d72c86e037a(),
            ["436cd823fcf04e0bbb1806a6fcf85de1"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_436cd823fcf04e0bbb1806a6fcf85de1(),
            ["426f5b2eaeaa4fc98611e6235ad861a0"] = new Binding_aa7ea8e0044f0964eb9c782a689ca1b1_426f5b2eaeaa4fc98611e6235ad861a0(),
        };
        
        private Dictionary<string, Action<CommandBinding, CommandsHandler>> bakedCommandBindings = new Dictionary<string, Action<CommandBinding, CommandsHandler>>();
        
        public CoherenceSync_aa7ea8e0044f0964eb9c782a689ca1b1()
        {
            bakedCommandBindings.Add("d935ef02354c46c2bdb5dc4086cefeb7", BakeCommandBinding__aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7);
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
    
        private void BakeCommandBinding__aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7_CommandTarget = (global::UnityEngine.Animator)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("UnityEngine.Animator.SetTrigger", "(System.String)", SendCommand__aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7, ReceiveLocalCommand__aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7, MessageTarget.All, _aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7_CommandTarget, false);
        }
        
        private void SendCommand__aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7(MessageTarget target, ChannelID channelID, object[] args)
        {
            var command = new _aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7();
            
            int i = 0;
            command.name = (System.String)args[i++];
        
            client.SendCommand(command, target, entityId, channelID);
        }
        
        private void ReceiveLocalCommand__aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7(MessageTarget target, ChannelID _, object[] args)
        {
            var command = new _aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7();
            
            int i = 0;
            command.name = (System.String)args[i++];
            
            ReceiveCommand__aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7(command);
        }

        private void ReceiveCommand__aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7(_aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7 command)
        {
            var target = _aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7_CommandTarget;
            
            target.SetTrigger((System.String)(command.name));
        }
        
        public override void ReceiveCommand(IEntityCommand command)
        {
            switch (command)
            {
                case _aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7 castedCommand:
                    ReceiveCommand__aa7ea8e0044f0964eb9c782a689ca1b1_d935ef02354c46c2bdb5dc4086cefeb7(castedCommand);
                    break;
                default:
                    logger.Warning(Coherence.Log.Warning.ToolkitBakedSyncReceiveCommandUnhandled,
                        $"CoherenceSync_aa7ea8e0044f0964eb9c782a689ca1b1 Unhandled command: {command.GetType()}.");
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
            this.logger = logger.With<CoherenceSync_aa7ea8e0044f0964eb9c782a689ca1b1>();
            this.bridge = bridge;
            this.entityId = entityId;
            this.client = client;        
        }
    }
}
