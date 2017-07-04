using ContractFormalLanguage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractFormalLanguage
{
    public class ReparationContract : Contract
    {
        private Contract contract;
        private Contract reparationContract;

        private string text;

        public ReparationContract(Contract contract, Contract reparationContract)
        {
            this.contract = contract;
            this.reparationContract = reparationContract;

            this.text = "(" + contract.ToString() + ")*>(" + reparationContract.ToString() + ")";
        }

        public Contract Transition(List<Action> actionSet)
        {
            Contract newContract = contract.Transition(actionSet);

            //if (newContract.isViolated())
            //{
            //    return reparationContract;
            //}
            //else if (newContract.isUnknown())
            //{
            //    return new UnknownContract();
            //    //return this;
            //}
            //else if (newContract.isSatisfied())
            //{
            //    return new SatisfiedContract();
            //}

            return ContractFactory.CreateReparation(newContract, reparationContract);
            
        }

        public bool EquivalentWith(Contract otherContract)
        {
            if (this.Reduction().Equals(otherContract.Reduction()))
            {
                return true;
            }
            else
            {
                if (contract.Reduction().GetType().Equals(otherContract.Reduction().GetType()))
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
            Contract startsWith = contract.StartsWith();
            Type startsWithType = startsWith.GetType();

            if (startsWithType.Equals(typeof(UnknownContract)))
            {
                return new UnknownContract();
            }
            else if (startsWithType.Equals(typeof(SatisfiedContract)))
            {
                return new SatisfiedContract();
            }
            else
            {
                return this;
            }
        }

        public Contract Reduction()
        {
            return this.ReductionToConditionalOrNot().SyntacticReduction();
        }

        public Contract StartsWith()
        {
            return this.contract.StartsWith();
        }

        public List<Contract> oneStepAwayContracts()
        {
            List<Contract> contracts = new List<Contract>();

            contracts = contracts.Concat(contract.oneStepAwayContracts()).ToList();

            if (contracts.Any(c => c.GetType().Equals(typeof(ViolatedContract)) || c.GetType().Equals(typeof(UnknownContract))))
            {
                contracts.RemoveAll(c => c.GetType().Equals(typeof(ViolatedContract)));

                contracts = contracts.Concat(reparationContract.oneStepAwayContracts()).ToList();
            }

            return contracts;
        }

        private Contract ReductionToConditionalOrNot()
        {
            Contract equivalentToThis = this;

            //if (this.first.GetType().Equals(typeof(ConditionalContract)))
            //{
            //    equivalentToThis = new ConditionalContract(((ConditionalContract)first).getCondition(), new ReparationContract(((ConditionalContract)first).getContract(), reparationContract));
            //}

            return equivalentToThis;
        }

        public List<Action> getAllActions()
        {
            return contract.getAllActions().Concat(reparationContract.getAllActions()).ToList();
        }

        public List<List<Action>> stepActionSets()
        {
            return this.contract.stepActionSets();
        }

        public bool isViolated()
        {
            return false;
        }

        public bool isSatisfied()
        {
            if (contract.isSatisfied())
            {
                return true;
            }
            else return false;
        }

        public bool isUnknown()
        {
            if (contract.isUnknown())
            {
                return true;
            }
            else return false;
        }
    }
}