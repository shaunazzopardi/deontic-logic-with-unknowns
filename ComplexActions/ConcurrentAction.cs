using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractFormalLanguage
{
    public class ConcurrentAction : ComplexAction
    {
        private ComplexAction firstAction;
        private ComplexAction secondAction;

        public ConcurrentAction(ComplexAction firstAction, ComplexAction secondAction)
            : base("(" + firstAction.ToString() + ")&(" + secondAction.ToString() + ")")
        {
            this.firstAction = firstAction;
            this.secondAction = secondAction;
        }

        public override  ComplexAction Residual(List<Action> actionSet)
        {
            Type firstType = firstAction.GetType();
            Type secondType = secondAction.GetType();

            if (firstType.Equals(typeof(EmptyAction))
                && (secondType.Equals(typeof(EmptyAction))))
            {
                return null;
            }
            else if (secondType.Equals(typeof(EmptyAction)))
            {
                if (actionSet.Find(i => secondAction.ToString() == i.ToString()) != null)
                {
                    return secondAction.Residual(actionSet);
                }
                else
                {
                    return secondAction;
                }
            }
            else if (firstType.Equals(typeof(EmptyAction)))
            {
                if (actionSet.Find(i => firstAction.ToString() == i.ToString()) != null)
                {
                    return firstAction.Residual(actionSet);
                }
                else
                {
                    return firstAction;
                }
            }
            else
            {
                return new ConcurrentAction(firstAction.Residual(actionSet), secondAction.Residual(actionSet));
            }
        }

        public override  bool containsEmpty()
        {
            if (firstAction.GetType().Equals(typeof(EmptyAction)) && (secondAction.GetType().Equals(typeof(EmptyAction))))
            {
                return true;
            }
            else
            {
                return false;
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
            List<Action> stepActionSets = new List<Action>();

            stepActionSets.AddRange(firstAction.firstActions());
            stepActionSets.AddRange(secondAction.firstActions());


            //List<List<Action>> firstStepActions = firstAction.firstActions();
            //List<List<Action>> secondStepActions = secondAction.firstActions();

            //if (firstStepActions.Count > 0 && secondStepActions.Count > 0)
            //{
            //    foreach (List<Action> firstStep in firstStepActions)
            //    {
            //        foreach (List<Action> secondStep in secondStepActions)
            //        {
            //            firstActions.Add(firstStep);
            //            firstActions.Last().AddRange(secondStep);

            //            firstActions[firstActions.Count - 1] = firstActions.Last().Distinct().ToList();

            //        }
            //    }
            //}
            //else
            //{
            //    firstActions = firstStepActions.Concat(secondStepActions).ToList();
            //}

            return stepActionSets;
        }

        public override List<Action> getAllActions()
        {
            return firstAction.getAllActions().Concat(secondAction.getAllActions()).ToList();
        }

        public override ComplexAction RemoveEmpty()
        {
            ComplexAction newAction;

            if (firstAction.GetType().Equals(typeof(EmptyAction))
                && secondAction.GetType().Equals(typeof(EmptyAction)))
            {
                newAction = firstAction;
            }
            else newAction = this;

            return newAction;
        }
    }
}