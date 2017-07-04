using ContractFormalLanguage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractFormalLanguage
{
    public class ComposedContract : Contract
    {
        private Contract first;
        private Contract second;

        private string text;

        public ComposedContract(Contract first, Contract second)
        {
            this.first = first;
            this.second = second;

            this.text = "(" + first.ToString() + ")>(" + second.ToString() + ")";
        }

        public Contract Transition(List<Action> actionSet)
        {
            Contract newfirst = first.Transition(actionSet);

            //if (newfirst.isSatisfied())
            //{
            //    return second;
            //}
            //else if (newfirst.isUnknown())
            //{
            //    return new UnknownContract();
            //    //return this;
            //}
            //else if (newfirst.isViolated())
            //{
            //    return new ViolatedContract();
            //}

            return ContractFactory.CreateComposed(newfirst, second);
        }

        public bool EquivalentWith(Contract otherContract)
        {
            if (this.Equals(otherContract))
            {
                return true;
            }
            else
            {
                //if (first.Reduction().GetType().Equals(second.Reduction().GetType()))
                if (first.GetType().Equals(otherContract.GetType()))
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
            Contract firstReduced = first.Reduction();
            Type firstReducedType = firstReduced.GetType();

            if (firstReducedType.Equals(typeof(UnknownContract)))
            {
                return new UnknownContract();
            }
            else if (firstReducedType.Equals(typeof(ViolatedContract)))
            {
                return new ViolatedContract();
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

        private Contract ReductionToConditionalOrNot()
        {
            Contract equivalentToThis = this;

            //if (this.first.GetType().Equals(typeof(ConditionalContract)))
            //{
            //    equivalentToThis = new ConditionalContract(((ConditionalContract)first).getCondition(), new ComposedContract(((ConditionalContract)first).getContract(), second));
            //}

            return equivalentToThis;
        }

        public Contract StartsWith()
        {
            return first.StartsWith();
        }

        public List<Contract> oneStepAwayContracts()
        {
            List<Contract> contracts = new List<Contract>();

            contracts = contracts.Concat(first.oneStepAwayContracts()).ToList();

            if (contracts.Any(c => c.GetType().Equals(typeof(SatisfiedContract)) || c.GetType().Equals(typeof(UnknownContract))))
            {
                contracts.RemoveAll(contract => contract.GetType().Equals(typeof(SatisfiedContract)));

                contracts = contracts.Concat(second.oneStepAwayContracts()).ToList();
            }

            return contracts;
        }

        public List<Action> getAllActions()
        {
            return first.getAllActions().Concat(second.getAllActions()).ToList();
        }

        public List<List<Action>> stepActionSets()
        {
            return this.first.stepActionSets();
        }

        public bool isViolated()
        {
            if (first.isViolated())
            {
                return true;
            }
            else return false;
        }

        public bool isSatisfied()
        {
            return false;
        }

        public bool isUnknown()
        {
            if (first.isUnknown())
            {
                return true;
            }
            else return false;
        }
    }
}