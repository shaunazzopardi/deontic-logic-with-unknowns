using ContractFormalLanguage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractFormalLanguage
{
    public class ConcurrentContract : Contract
    {
        private Contract contract;
        private Contract otherContract;

        private List<Contract> allContracts;

        private string stringRep;

        private string text;

        public ConcurrentContract(Contract contract, Contract otherContract)
        {
            this.contract = contract;
            this.otherContract = otherContract;

            allContracts =  this.setOfApplicableNonConcurrentContracts();

            this.text = "(" + contract.ToString() + ")&(" + otherContract.ToString() + ")";
        }

        public Contract Transition(List<Action> actionSet)
        {
            Contract newContract = this.contract.Transition(actionSet);
            Contract newOtherContract = this.otherContract.Transition(actionSet);

            Contract transitionedContract = ContractFactory.CreateConcurrent(newContract, newOtherContract);

            //if (transitionedContract.isViolated())
            //{
            //    return new ViolatedContract();
            //}
            //else if (transitionedContract.isSatisfied())
            //{
            //    return new SatisfiedContract();
            //}
            //else if (transitionedContract.isUnknown())
            //{
            //    return new UnknownContract();
            //}

            return transitionedContract;
        }

        public bool isViolated()
        {
            if (contract.isViolated() || otherContract.isViolated())
            {
                return true;
            }
            else return false;
        }

        public bool isSatisfied()
        {
            if (contract.isSatisfied() && otherContract.isSatisfied())
            {
                return true;
            }
            else return false;
        }

        public bool isUnknown()
        {
            if (contract.isUnknown() && otherContract.isUnknown())
            {
                return true;
            }
            else return false;
        }

        public bool EquivalentWith(Contract otherContract)
        {
            Contract contractSimplified = this.contract.Reduction();
            Contract otherContractSimplified = this.otherContract.Reduction();

            if (this.Equals(otherContract))
            {
                return true;
            }
            else
            {
                if (contractSimplified.EquivalentWith(new SatisfiedContract()))
                {
                    return otherContract.EquivalentWith(otherContractSimplified);
                }
                else if (otherContractSimplified.EquivalentWith(new SatisfiedContract()))
                {
                    return otherContract.EquivalentWith(contractSimplified);
                }
            }

            return false;
        }

        public bool Equals(Contract otherContract)
        {
            if (otherContract.GetType().Equals(typeof(ConcurrentContract)))
            {
                List<Contract> otherContracts = ((ConcurrentContract)otherContract).setOfApplicableNonConcurrentContracts();

                List<Contract> thisContracts = this.setOfApplicableNonConcurrentContracts();

                if (otherContracts.Count == thisContracts.Count)
                {
                    HashSet<Contract> both = new HashSet<Contract>(otherContracts);

                    both.UnionWith(thisContracts);

                    if (both.Count == thisContracts.Count) return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            if (stringRep == null || stringRep == "")
            {
                HashSet<string> strings = new HashSet<string>();

                for (int i = 0; i < allContracts.Count; i++)
                {
                    strings.Add(allContracts[i].ToString());
                }

                List<string> uniqueContractStrings = new List<string>();

                uniqueContractStrings = strings.ToList().OrderByDescending(s => s).ToList();

                stringRep = String.Join("&", uniqueContractStrings);
            }
           
            return this.stringRep;
        }

        public Contract SyntacticReduction()
        {
            Contract conReduced = contract.Reduction();
            Type conReducedType = conReduced.GetType();

            Contract othReduced = otherContract.Reduction();
            Type othReducedType = othReduced.GetType();

            if (conReducedType.Equals(typeof(ViolatedContract))
                || othReducedType.Equals(new ViolatedContract())) return new ViolatedContract();

            if (conReducedType.Equals(new UnknownContract())
                || othReducedType.Equals(new UnknownContract())) return new UnknownContract();

            if (conReducedType.Equals(typeof(SatisfiedContract)))
            {
                return othReduced;
            }
            else if (othReducedType.Equals(typeof(SatisfiedContract)))
            {
                return conReduced;
            }
            else
            {
                return new ConcurrentContract(conReduced, othReduced);
            }
        }
        public Contract Reduction()
        {
            return this.SyntacticReduction();
        }

        public Contract StartsWith()
        {
            return new ConcurrentContract(contract.StartsWith(), otherContract.StartsWith());
        }

        public List<Contract> oneStepAwayContracts()
        {
            List<Contract> contracts = new List<Contract>();

            List<Contract> firstArgStep = contract.oneStepAwayContracts();
            List<Contract> secondArgStep = otherContract.oneStepAwayContracts();

            foreach (Contract firstContract in firstArgStep)
            {
                foreach (Contract secondContract in secondArgStep)
                {
                    contracts.Add(new ConcurrentContract(firstContract, secondContract));
                }
            }

            return contracts;
        }

        public List<Contract> setOfApplicableNonConcurrentContracts()
        {
            HashSet<Contract> contracts = new HashSet<Contract>();

            if (contract.GetType().Equals(typeof(ConcurrentContract)))
            {
                contracts.UnionWith(((ConcurrentContract)contract).setOfApplicableNonConcurrentContracts());
            }
            else
            {
                contracts.Add(contract);//.Reduction());
            }

            if (otherContract.GetType().Equals(typeof(ConcurrentContract)))
            {
                contracts.UnionWith(((ConcurrentContract)otherContract).setOfApplicableNonConcurrentContracts());
            }
            else
            {
                contracts.Add(otherContract);//.Reduction());
            }

            return contracts.ToList();
        }

        public Contract getFirstArg()
        {
            return contract;
        }

        public List<Action> getArgsActions()
        {
            List<Action> actions = new List<Action>();

            Type contractType = contract.GetType();
            Type otherContractType = otherContract.GetType();

            if (!contractType.Equals(typeof(ConcurrentContract)))
            {
                actions.AddRange(((ConcurrentContract)contract).getArgsActions());
            }

            if (!otherContractType.Equals(typeof(ConcurrentContract)))
            {
                actions.AddRange(((ConcurrentContract)otherContract).getArgsActions());
            }

            if (contractType.Equals(typeof(Obligation)))
            {
                actions.Add(((Obligation)contract).GetAction());
            }
            else if (contractType.Equals(typeof(Permission)))
            {
                actions.Add(((Permission)contract).GetAction());
            }
            else if (contractType.Equals(typeof(Prohibition)))
            {
                actions.Add(((Prohibition)contract).GetAction());
            }

            if (otherContractType.Equals(typeof(Obligation)))
            {
                actions.Add(((Obligation)otherContract).GetAction());
            }
            else if (otherContractType.Equals(typeof(Permission)))
            {
                actions.Add(((Permission)otherContract).GetAction());
            }
            else if (otherContractType.Equals(typeof(Prohibition)))
            {
                actions.Add(((Prohibition)otherContract).GetAction());
            }

            return actions;
        }

        //not needed for conflict analysis
        //but purpose was to get [a]C&[a]C' into [a](C&C')
        //public Contract ReductionToConditionalOrNot()
        //{
        //    Contract equivalentToThis = this;

        //    if (this.first.GetType().Equals(typeof(ConditionalContract)))
        //    {
        //        if (this.second.GetType().Equals(typeof(ConditionalContract)))
        //        {
        //            ComplexAction contractCondition = ((ConditionalContract)first).getCondition();
        //            ComplexAction otherContractCondition = ((ConditionalContract)second).getCondition();

        //            if(contractCondition.e)

        //            equivalentToThis = new ConditionalContract(((ConditionalContract)first).getCondition(), new ComposedContract(((ConditionalContract)first).getContract(), second));
        //        }
        //    }

        //    return equivalentToThis;
        //}

        public Contract getSecondArg()
        {
            return otherContract;
        }
        public List<Action> getAllActions()
        {
            return contract.getAllActions().Concat(otherContract.getAllActions()).ToList();
        }

        public List<List<Action>> stepActionSets()
        {
            List<Contract> allContracts = this.setOfApplicableNonConcurrentContracts();

            HashSet<Action> allActions = new HashSet<Action>();

            for(int i = 0; i < allContracts.Count; i++)
            {
                List<Action> sub = allContracts[i].getAllActions();

                allActions.UnionWith(sub);
            }


            return Action.permutation(allActions.ToList());
        }
    }
}