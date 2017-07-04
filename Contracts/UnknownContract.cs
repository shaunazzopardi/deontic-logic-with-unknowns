using System.Collections.Generic;

namespace ContractFormalLanguage
{
    public class UnknownContract : Contract
    {
        protected Party party;
        protected Action action;

        protected string text = "UNK";

        public UnknownContract()
        {
        }

        public UnknownContract(Action action, Party party)
        {
            this.party = party;
            this.action = action;

            if (!party.GetType().Equals(typeof(UnknownParty)) || !action.GetType().Equals(typeof(UnknownAction)))
            {
                this.text = "?[" + party.ToString() + "](" + action.ToString() + ")";
            }

        }
        public Contract Transition(List<Action> actionSet)
        {
            return this;
        }

        public bool EquivalentWith(Contract otherContract)
        {
            if (this.Equals(otherContract))
            {
                return true;
            }
            else if (otherContract.Equals(new SatisfiedContract())
                        || otherContract.Equals(new ViolatedContract()))
            {
                return false;
            }
            else if (otherContract.EquivalentWith(this))
            {
                return true;
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

        public List<Contract> oneStepAwayContracts()
        {
            List<Contract> contracts = new List<Contract>();

            contracts.Add(this);

            return contracts;
        }

        public List<Action> getAllActions()
        {
            return new List<Action>();
        }

        public List<List<Action>> stepActionSets()
        {
            return new List<List<Action>>() { };
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
            return true;
        }
    }
}