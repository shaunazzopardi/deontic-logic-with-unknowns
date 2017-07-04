using System.Collections.Generic;

namespace ContractFormalLanguage
{
    public class EmptyAction : ComplexAction
    {
        string text = "0";
        public override ComplexAction Residual(List<Action> actionSet)
        {
            return null;
        }

        public override bool containsEmpty()
        {
            return true;
        }

        public override List<Action> firstActions()
        {
            return new List<Action>() { new UnknownAction("") };
        }

        public override List<Action> getAllActions()
        {
            return new List<Action>();
        }

        public override ComplexAction RemoveEmpty()
        {
            return this;
        }

        public override string ToString()
        {
            return text;
        }
    }
}