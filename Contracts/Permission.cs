using System.Collections.Generic;

namespace ContractFormalLanguage
{
    public class Permission : Contract
    {
        protected Party party;
        protected Action action;

        private string text;

        public Permission(Action action, Party party)
        {
            this.party = party;
            this.action = action;

            this.text = "P[" + party.text + "](" + action.text + ")";
        }

        public Contract Transition(List<Action> actionSet)
        {
            if (action.GetType().Equals(typeof(UnknownAction)))
            {
                return new UnknownContract();
            }

            if (actionSet.Find(i => i.text == action.text) != null)
            {
                return new SatisfiedContract();
            }
            else
            {
                return new UnknownContract();
            }
        }

        public bool EquivalentWith(Contract otherContract)
        {
            if (otherContract.GetType().Equals(this.GetType()))
            {
                if (otherContract.ToString() == this.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public bool Equals(Contract otherContract)
        {
            if (otherContract.ToString().Equals(this.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return this.text;
        }

        public Contract SyntacticReduction()
        {
            return this;
        }

        public Contract Reduction()
        {
            return this.SyntacticReduction();
        }

        public Contract StartsWith()
        {
            return this;
        }

        public Action GetAction()
        {
            return this.action;
        }

        public List<Contract> oneStepAwayContracts()
        {
            List<Contract> contracts = new List<Contract>();

            contracts.Add(new SatisfiedContract());
            contracts.Add(new UnknownContract());

            return contracts;
        }

        public List<Action> getAllActions()
        {
            return new List<Action>() { action };
        }

        public List<List<Action>> stepActionSets()
        {
            return new List<List<Action>>() { new List<Action>() { this.action }, new List<Action>() { Action.empty } };
        }

        public bool isViolated()
        {
            return false;
        }

        public bool isSatisfied()
        {
            return false;
        }

        public bool isUnknown()
        {
            return false;
        }
    }
}