using System.Collections.Generic;

namespace ContractFormalLanguage
{
    public class SimpleAction : ComplexAction
    {
        public Action action;

        public SimpleAction(Action action) : base(action.ToString())
        {
            this.action = action;
        }
        public override ComplexAction Residual(List<Action> actionSet)
        {
            if (actionSet.Find(i => this.ToString() == i.ToString()) != null)
            {
                return new EmptyAction();
            }
            else
            {
                return this;
            }
        }

        public override bool containsUnknownPath()
        {
            if (action.GetType().Equals(typeof(UnknownAction)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ComplexAction> getFirstAction()
        {
            List<ComplexAction> actions = new List<ComplexAction>();

            actions.Add(this);

            return actions;
        }

        public bool containsChoiceCurrently()
        {
            return false;
        }

        public override List<Action> firstActions()
        {
            if (action.GetType().Equals(typeof(KnownAction)))
                return new List<Action>() { action };
            else return new List<Action>() { };
        }

        public override List<Action> getAllActions()
        {
            return new List<Action>(){action};
        }

        public override ComplexAction RemoveEmpty()
        {
            return this;
        }

        public override bool containsEmpty()
        {
            return false;
        }
    }
}