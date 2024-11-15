using System;
using System.Collections.Generic;
using TrigBaker.BakingLib.Computables;
using UnityEngine;
using CeLishp.Interpreter;
using CeLishp.Interpreter.Implementation;
using CeLishp.Parser.Implementation;
using static CeLishp.Interpreter.Implementation.UnaryTrig;
using static TrigBaker.BakingLib.Computables.UnityTransformProperty;

namespace TrigBaker.BakingLib
{
    public class Baker
    {
        private Animation animation;
        private readonly Dictionary<string, IInputValue> _values;
        private readonly Dictionary<string, INaryFunction> _funcs;
        private TimeKeeper time = new();
        public Baker(Transform simulating)
        {
            
            _values = new Dictionary<string, IInputValue>
            {
                { "position.x", new UnityTransformProperty(simulating, TransformProperty.PositionX) },
                { "position.y", new UnityTransformProperty(simulating, TransformProperty.PositionY) },
                { "position.z", new UnityTransformProperty(simulating, TransformProperty.PositionZ) },
                { "rotation.x", new UnityTransformProperty(simulating, TransformProperty.RotationEulerX)},
                { "rotation.y", new UnityTransformProperty(simulating, TransformProperty.RotationEulerY)},
                { "rotation.z", new UnityTransformProperty(simulating, TransformProperty.RotationEulerZ)},
                { "time", time},
                {"pi", new Constant<float>(Mathf.PI)}
            };
            _funcs = new Dictionary<string, INaryFunction>
            {
                { "sin", new UnaryTrig(UnaryTrigFunction.Sine) },
                { "cos", new UnaryTrig(UnaryTrigFunction.Cosine) },
                { "tan", new UnaryTrig(UnaryTrigFunction.Tangent) },
                { "+", new NaryAddition()},
                { "-", new NarySubtraction()},
                { "*", new NaryMultiplication()},
                { "/", new BinaryDivision()},
            };
        }

        public AnimationClip BakeAnimation(string formula, AnimationClip clip, BakerParams options, Type receiverComponent, string prop)
        {
            var syntax = new SimpleLispSyntax();
            var synTree = syntax.GenerateTree(formula);
            var exTree = syntax.ParseTree(synTree, _funcs, _values);
            Interpreter interpreter = new Interpreter(exTree);
            AnimationCurve curve = new();
            var stepLen = 1f / options.TemporalResolution;
            for (int i = 0; i < options.AnimationDuration * options.TemporalResolution; i++)
            {
                var kf = new Keyframe(time.t, interpreter.RunTree<float>())
                {
                    weightedMode = WeightedMode.None
                };
                curve.AddKey(kf);
                time.t += stepLen;
            }
            for(int i = 0; i < curve.keys.Length; i++)
                curve.SmoothTangents(i, 1f);
            clip.SetCurve("", receiverComponent, prop, curve );
            return clip;
        }
        
    }
}