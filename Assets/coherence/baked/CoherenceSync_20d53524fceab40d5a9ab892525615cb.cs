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
    public class Binding_20d53524fceab40d5a9ab892525615cb_7711bc2e3be74b03a7060692d306fce7 : IntBinding
    {   
        private global::Counter CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::Counter)UnityComponent;
        }

        public override string CoherenceComponentName => "_20d53524fceab40d5a9ab892525615cb_5847726189716557621";
        public override uint FieldMask => 0b00000000000000000000000000000001;

        public override System.Int32 Value
        {
            get { return (System.Int32)(CastedUnityComponent.count); }
            set { CastedUnityComponent.count = (System.Int32)(value); }
        }

        protected override (System.Int32 value, AbsoluteSimulationFrame simFrame) ReadComponentData(ICoherenceComponentData coherenceComponent, Vector3 floatingOriginDelta)
        {
            var value = ((_20d53524fceab40d5a9ab892525615cb_5847726189716557621)coherenceComponent).count;

            var simFrame = ((_20d53524fceab40d5a9ab892525615cb_5847726189716557621)coherenceComponent).countSimulationFrame;
            
            return (value, simFrame);
        }

        public override ICoherenceComponentData WriteComponentData(ICoherenceComponentData coherenceComponent, AbsoluteSimulationFrame simFrame)
        {
            var update = (_20d53524fceab40d5a9ab892525615cb_5847726189716557621)coherenceComponent;
            if (RuntimeInterpolationSettings.IsInterpolationNone)
            {
                update.count = Value;
            }
            else
            {
                update.count = GetInterpolatedAt(simFrame / InterpolationSettings.SimulationFramesPerSecond);
            }

            update.countSimulationFrame = simFrame;
            
            return update;
        }

        public override ICoherenceComponentData CreateComponentData()
        {
            return new _20d53524fceab40d5a9ab892525615cb_5847726189716557621();
        }    
    }
    
    [UnityEngine.Scripting.Preserve]
    public class Binding_20d53524fceab40d5a9ab892525615cb_d34657af8900466f81ebcda7b52c281f : PositionBinding
    {   
        private global::UnityEngine.Transform CastedUnityComponent;

        protected override void OnBindingCloned()
        {
    	    CastedUnityComponent = (global::UnityEngine.Transform)UnityComponent;
        }

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
            if (RuntimeInterpolationSettings.IsInterpolationNone)
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
    public class CoherenceSync_20d53524fceab40d5a9ab892525615cb : CoherenceSyncBaked
    {
        private Entity entityId;
        private Logger logger = Coherence.Log.Log.GetLogger<CoherenceSync_20d53524fceab40d5a9ab892525615cb>();
        
        private global::Counter _20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8_CommandTarget;
        private global::Counter _20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b_CommandTarget;
        
        
        private IClient client;
        private CoherenceBridge bridge;
        
        private readonly Dictionary<string, Binding> bakedValueBindings = new Dictionary<string, Binding>()
        {
            ["7711bc2e3be74b03a7060692d306fce7"] = new Binding_20d53524fceab40d5a9ab892525615cb_7711bc2e3be74b03a7060692d306fce7(),
            ["d34657af8900466f81ebcda7b52c281f"] = new Binding_20d53524fceab40d5a9ab892525615cb_d34657af8900466f81ebcda7b52c281f(),
        };
        
        private Dictionary<string, Action<CommandBinding, CommandsHandler>> bakedCommandBindings = new Dictionary<string, Action<CommandBinding, CommandsHandler>>();
        
        public CoherenceSync_20d53524fceab40d5a9ab892525615cb()
        {
            bakedCommandBindings.Add("2f75e99488a84da2aebe765c3648d9d8", BakeCommandBinding__20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8);
            bakedCommandBindings.Add("7ce8b858d0074d868da45a67d2d9a68b", BakeCommandBinding__20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b);
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
    
        private void BakeCommandBinding__20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8_CommandTarget = (global::Counter)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("Counter.ResetToZero", "()", SendCommand__20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8, ReceiveLocalCommand__20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8, MessageTarget.All, _20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8_CommandTarget, false);
        }
        
        private void SendCommand__20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8(MessageTarget target, object[] args)
        {
            var command = new _20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8();
            
        
            client.SendCommand(command, target, entityId);
        }
        
        private void ReceiveLocalCommand__20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8(MessageTarget target, object[] args)
        {
            var command = new _20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8();
            
            
            ReceiveCommand__20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8(command);
        }

        private void ReceiveCommand__20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8(_20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8 command)
        {
            var target = _20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8_CommandTarget;
            
            target.ResetToZero();
        }
    
        private void BakeCommandBinding__20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b(CommandBinding commandBinding, CommandsHandler commandsHandler)
        {
            _20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b_CommandTarget = (global::Counter)commandBinding.UnityComponent;
            commandsHandler.AddBakedCommand("Counter.AddOne", "()", SendCommand__20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b, ReceiveLocalCommand__20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b, MessageTarget.All, _20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b_CommandTarget, false);
        }
        
        private void SendCommand__20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b(MessageTarget target, object[] args)
        {
            var command = new _20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b();
            
        
            client.SendCommand(command, target, entityId);
        }
        
        private void ReceiveLocalCommand__20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b(MessageTarget target, object[] args)
        {
            var command = new _20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b();
            
            
            ReceiveCommand__20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b(command);
        }

        private void ReceiveCommand__20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b(_20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b command)
        {
            var target = _20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b_CommandTarget;
            
            target.AddOne();
        }
        
        public override void ReceiveCommand(IEntityCommand command)
        {
            switch (command)
            {
                case _20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8 castedCommand:
                    ReceiveCommand__20d53524fceab40d5a9ab892525615cb_2f75e99488a84da2aebe765c3648d9d8(castedCommand);
                    break;
                case _20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b castedCommand:
                    ReceiveCommand__20d53524fceab40d5a9ab892525615cb_7ce8b858d0074d868da45a67d2d9a68b(castedCommand);
                    break;
                default:
                    logger.Warning($"CoherenceSync_20d53524fceab40d5a9ab892525615cb Unhandled command: {command.GetType()}.");
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
    
            logger.Warning($"Unable to find archetype {archetypeName} in dictionary. Please, bake manually (coherence > Bake)");
            
            return null;
        }
        
        public override void Dispose()
        {
        }
        
        public override void Initialize(Entity entityId, CoherenceBridge bridge, IClient client, CoherenceInput input, Logger logger)
        {
            this.logger = logger.With<CoherenceSync_20d53524fceab40d5a9ab892525615cb>();
            this.bridge = bridge;
            this.entityId = entityId;
            this.client = client;        
        }
    }

}