using System;
using System.Linq;
using CeLishp.Interpreter;

namespace TrigBaker.BakingLib.Computables
{
    public class NaryMultiplication : INaryFunction
    {
        public object Run(object[] input)
        {
            throw new System.NotImplementedException();
        }

        public object RunNary(object[] inputs)
        {
            if (inputs == null || inputs.Length < 2 || inputs.Any(x => x is not float))
                throw new ArgumentException("Multiplication needs at least two float parameters");
            if (!(inputs[0] is float f1))
                throw new ArgumentException("How did we get here");
            float acc = f1;
            foreach (var input in inputs.Skip(1))
            {
                if (!(input is float f))
                    throw new ArgumentException("How did we get here");
                acc *= f;
            }

            return acc;
        }
    }
}