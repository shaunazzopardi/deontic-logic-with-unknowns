using System.Collections.Generic;

namespace ContractFormalLanguage
{
    public abstract class ComplexAction
    {
        string text;

        public ComplexAction()
        {
            this.text = "";
        }

        public ComplexAction(string text)
        {
            this.text = text;
        }

        public virtual ComplexAction Residual(List<Action> actionSet)
        {
            return this;
        }

        public abstract bool containsEmpty();

        public virtual bool containsUnknownPath()
        {
            return false;
        }

        public override string ToString()
        {
            return this.text;
        }

        public abstract List<Action> firstActions();

        public abstract List<Action> getAllActions();

        public abstract ComplexAction RemoveEmpty();
    }
}