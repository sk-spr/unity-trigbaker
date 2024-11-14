using CeLishp.Interpreter;

namespace TrigBaker.BakingLib.Computables
{
    public class TimeKeeper : IInputValue
    {
        public float t = 0;
        public object Run(object[] input)
        {
            throw new System.NotImplementedException();
        }

        public object GetValue()
        {
            return t;
        }
    }
}