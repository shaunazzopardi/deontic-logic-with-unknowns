using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractFormalLanguage
{
    public class ChoiceAction : ComplexAction
    {
        private ComplexAction firstAction;
        private ComplexAction secondAction;

        public ChoiceAction(ComplexAction firstAction, ComplexAction secondAction)
            : base("(" + firstAction.ToString() + ")+(" + secondAction.ToString() + ")")
        {
            this.firstAction = firstAction;
            this.secondAction = secondAction;
        }

        public ComplexAction getFirstAction()
        {
            return firstAction;
        }

        public ComplexAction getSecondAction()
        {
            return secondAction;
        }

        public override  ComplexAction Residual(List<Action> actionSet)
        {
            if (firstAction.GetType().Equals(typeof(EmptyAction)) && (secondAction.GetType().Equals(typeof(EmptyAction))))
            {
                return null;
            }
            else if (secondAction.GetType().Equals(typeof(EmptyAction)))
            {
                return firstAction.Residual(actionSet);
            }
            else if (firstAction.GetType().Equals(typeof(EmptyAction)))
            {
                return secondAction.Residual(actionSet);
            }
            else
            {
                return new ChoiceAction(firstAction.Residual(actionSet), secondAction.Residual(actionSet));
            }
        }

        public override bool containsEmpty()
        {
            if (firstAction.GetType().Equals(typeof(EmptyAction)) || (secondAction.GetType().Equals(typeof(EmptyAction))))
            {
                return true;
            }
            else
            {
                return firstAction.containsEmpty() || secondAction.containsEmpty();
            }
        }

        public override List<Action> firstActions()
        {
            List<Action> stepActionSets = new List<Action>();

            stepActionSets.AddRange(firstAction.firstActions());
            stepActionSets.AddRange(secondAction.firstActions());


            return stepActionSets;
        }

        public override List<Action> getAllActions()
        {
            return firstAction.getAllActions().Concat(secondAction.getAllActions()).ToList();
        }

        public override ComplexAction RemoveEmpty()
        {
            ComplexAction newAction;

            Type firstType = firstAction.GetType();
            Type secondType = secondAction.GetType();

            if (firstType.Equals(typeof(EmptyAction))
                && (secondType.Equals(typeof(EmptyAction))))
            {
                return firstAction;
            }
            else if (firstType.Equals(typeof(EmptyAction)))
            {
                newAction = secondAction.RemoveEmpty();
            }
            else if (secondType.Equals(typeof(EmptyAction)))
            {
                newAction = firstAction.RemoveEmpty();
            }
            else newAction = new ChoiceAction(firstAction.RemoveEmpty(), secondAction.RemoveEmpty());

            return newAction;
        }
    }
}