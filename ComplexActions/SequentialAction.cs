using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractFormalLanguage
{
    public class SequentialAction : ComplexAction
    {
        private ComplexAction firstAction;
        private ComplexAction secondAction;

        public SequentialAction(ComplexAction firstAction, ComplexAction secondAction)
            : base("(" + firstAction.ToString() + ").(" + secondAction.ToString() + ")")
        {
            this.firstAction = firstAction;
            this.secondAction = secondAction;
        }

        public override  ComplexAction Residual(List<Action> actionSet)
        {
            Type firstType = firstAction.GetType();
            Type secondType = secondAction.GetType();

            if (firstType.Equals(typeof(EmptyAction)))
            {
                return secondAction;
            }

            if (firstType.Equals(typeof(SimpleAction)))
            {
                if (actionSet.Find(i => firstAction.ToString() == i.ToString()) != null)
                {
                    return secondAction;
                }
                else
                {
                    return this;
                }
            }
            else
            {
                return new SequentialAction(firstAction.Residual(actionSet), secondAction);
            }
        }

        public override  bool containsUnknownPath()
        {
            if (firstAction.containsUnknownPath()
                || secondAction.containsUnknownPath())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override List<Action> firstActions()
        {
            return firstAction.firstActions();
        }

        public override List<Action> getAllActions()
        {
            return firstAction.getAllActions().Concat(secondAction.getAllActions()).ToList();
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