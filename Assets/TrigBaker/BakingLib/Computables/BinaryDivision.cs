using System;
using System.Linq;
using CeLishp.Interpreter;

namespace TrigBaker.BakingLib.Computables
{
    public class BinaryDivision : INaryFunction
    {
        public object Run(object[] input)
        {
            throw new System.NotImplementedException();
        }

        public object RunNary(object[] inputs)
        {
            if (inputs == null || inputs.Length != 2 || inputs.Any(x => x is not float))
                throw new ArgumentException("Division inputs must be exactly two floats");
            var n = inputs[0] as float?;
            var d = inputs[1] as float?;
            return n! / d!;
        }
    }
}