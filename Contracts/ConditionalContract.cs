using ContractFormalLanguage.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace ContractFormalLanguage
{
    public class ConditionalContract : Contract
    {
        private ComplexAction condition;
        private Contract contract;

        private string text;

        public ConditionalContract(ComplexAction condition, Contract contract)
        {
            if (condition == null)
            {
                condition = new EmptyAction();
            }
            this.condition = condition;
            this.contract = contract;

            this.text = "[" + condition.ToString() + "](" + contract.ToString() + ")";
        }

        public Contract Transition(List<Action> actionSet)
        {
            ComplexAction residual = condition.Residual(actionSet);

            Contract newContract;

            if (condition.containsEmpty())
            {
                if (condition.RemoveEmpty().GetType().Equals(typeof(EmptyAction)))
                {
                    return contract.Transition(actionSet);
                }

                newContract = ContractFactory.CreateConcurrent(contract.Transition(actionSet), new ConditionalContract(residual, contract));

                if (newContract.isViolated())
                {
                    newContract = new ViolatedContract();
                }
                else if (newContract.isSatisfied())
                {
                    newContract = new SatisfiedContract();
                }
                else if (newContract.isUnknown())
                {
                    newContract = new ViolatedContract();
                }
            }
            else
            {
                newContract = new ConditionalContract(residual, contract);
            }

            return newContract;
        }

        public bool EquivalentWith(Contract otherContract)
        {
            if (otherContract.GetType().Equals(typeof(UnknownContract)))
            {
                if (condition.containsUnknownPath())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
            ComplexAction conditionWithNoEmpty = condition.RemoveEmpty();

            if (conditionWithNoEmpty.GetType().Equals(typeof(EmptyAction)))
            {
                return contract.Reduction();
            }
            else if (condition.containsEmpty())
            {
                return new ConcurrentContract(contract.Reduction(), this.RemoveEmpty());
            }
            //else if (condition.GetType().Equals(typeof(ChoiceAction)))
            //{
            //    ComplexAction first = ((ChoiceAction)condition).getFirstAction();
            //    ComplexAction second = ((ChoiceAction)condition).getSecondAction();

            //    return new ConcurrentContract(new ConditionalContract(first, first).Reduction(), new ConditionalContract(second, first).Reduction());
            //}
            else
            {
                return new ConditionalContract(condition, contract.Reduction());
            }
        }

        private Contract RemoveEmpty()
        {
            Contract newContract;

            ComplexAction newCondition = condition.RemoveEmpty();

            if(newCondition.GetType().Equals(typeof(EmptyAction)))
            {
                newContract =  contract;
            }
            else
            {
                newContract = new ConditionalContract(newCondition, contract);
            }

            return newContract;
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

            List<Contract> argStep = contract.oneStepAwayContracts();

            foreach (Contract argContract in argStep)
            {
                contracts.Add(new ConcurrentContract(argContract, this));
            }

            return contracts;
        }

        public ComplexAction getCondition()
        {
            return condition;
        }

        public Contract getContract()
        {
            return contract;
        }

        public List<Action> getAllActions()
        {
            return condition.getAllActions().Concat(contract.getAllActions()).ToList();
        }

        public List<List<Action>> stepActionSets()
        {
            List<List<Action>> stepActionSetsList = new List<List<Action>>();

            List<List<Action>> contractStep = new List<List<Action>>();

            if (condition.containsEmpty())
            {
                contractStep.AddRange(contract.stepActionSets());
            }

            if (!condition.RemoveEmpty().GetType().Equals(typeof(EmptyAction)))
            {
                List<Action> conditionSteps = condition.firstActions();

                List<List<Action>> conditionStepActionSets = Action.permutation(conditionSteps);

                if (contractStep.Count != 0)
                {
                    for (int i = 0; i < contractStep.Count; i++)
                    {
                        for (int j = 0; j < conditionStepActionSets.Count; j++)
                        {
                            stepActionSetsList.Add(contractStep[i].Concat(conditionStepActionSets[j]).ToList());
                        }
                    }
                }
                else
                {
                    stepActionSetsList.AddRange(conditionStepActionSets);
                }
            }

            return stepActionSetsList;
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