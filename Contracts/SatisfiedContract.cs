using System.Collections.Generic;

namespace ContractFormalLanguage
{
    public class SatisfiedContract : Contract
    {
        private string text = "SAT";

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
            else if (otherContract.Equals(new ViolatedContract())
                || otherContract.Equals(new UnknownContract()))
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
            return true;
        }

        public bool isUnknown()
        {
            return false;
        }
    }
}