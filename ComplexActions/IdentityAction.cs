using System.Collections.Generic;

namespace ContractFormalLanguage
{
    public class IdentityAction : ComplexAction
    {
        string text = "1";
        public override ComplexAction Residual(List<Action> actionSet)
        {
            return new EmptyAction();
        }


        public override List<Action> firstActions()
        {
            return new List<Action>();
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

        public override bool containsEmpty()
        {
            return false;
        }
    }
}