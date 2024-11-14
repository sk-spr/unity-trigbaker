using System;
using CeLishp.Interpreter;
using UnityEngine;

namespace TrigBaker.BakingLib.Computables
{
    // internal class BaseProperty: IInputValue
    // {
    //     private Func<float> propertyGetter;
    //     public BaseProperty(Func<float> getter)
    //     {
    //         propertyGetter = getter;
    //     }
    //     public float Compute(IComputable[] child)
    //     {
    //         if (child != null)
    //         {
    //             throw new ArgumentException("PropertyComputable cannot have children!");
    //         }
    //
    //         return propertyGetter();
    //     }
    // }

    internal class UnityTransformProperty : IInputValue
    {
        public enum TransformProperty
        {
            PositionX,
            PositionY,
            PositionZ,
            RotationEulerX,
            RotationEulerY,
            RotationEulerZ,
            LocalScaleX,
            LocalScaleY,
            LocalScaleZ
        }

        private TransformProperty _prop;
        private Transform _transform;

        public UnityTransformProperty(Transform transform, TransformProperty prop)
        {
            _transform = transform;
            _prop = prop;
        }
        public object Run(object[] input)
        {
            throw new NotImplementedException();
        }

        public object GetValue()
        {
            return _prop switch
            {
                TransformProperty.PositionX => _transform.position.x,
                TransformProperty.PositionY => _transform.position.y,
                TransformProperty.PositionZ => _transform.position.z,
                TransformProperty.RotationEulerX => _transform.rotation.eulerAngles.x,
                TransformProperty.RotationEulerY => _transform.rotation.eulerAngles.y,
                TransformProperty.RotationEulerZ => _transform.rotation.eulerAngles.z,
                TransformProperty.LocalScaleX => _transform.localScale.x,
                TransformProperty.LocalScaleY => _transform.localScale.y,
                TransformProperty.LocalScaleZ => _transform.localScale.z,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}