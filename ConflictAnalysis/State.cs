using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractFormalLanguage.ConflictAnalysis
{
    public class State
    {
        public Automaton automaton;
        Contract contract;
        Contract startsWith;

        List<Contract> allIndividualConcurrentContracts;

        ConflictAnalyzer ca;

        bool mustConflict;
        bool mayConflict;

        public State(Automaton automaton, Contract contract)
        {
            this.automaton = automaton;

            ca = automaton.ca;

            this.contract = contract;//.Reduction();

            allIndividualConcurrentContracts = new List<Contract>();

            startsWith = this.contract.StartsWith();

            if (startsWith.GetType().Equals(typeof(ConcurrentContract)))
            {
                allIndividualConcurrentContracts = ((ConcurrentContract)startsWith).setOfApplicableNonConcurrentContracts();
            }
            else
            {
                allIndividualConcurrentContracts.Add(startsWith);
            }

            mustConflictAnalysis();
            mayConflictAnalysis();

        }

        public Contract getContract()
        {
            return contract;
        }

        public void Explore()
        {
            List<State> newStates = new List<State>();

            Type contractType = contract.GetType();

            if (contractType.Equals(typeof(ViolatedContract))
                || contractType.Equals(typeof(SatisfiedContract))
                || contractType.Equals(typeof(UnknownContract)))
            {
                return;
            }

            List<List<Action>> actions = contract.stepActionSets();

            if (allIndividualConcurrentContracts.All(c => c.GetType().Equals(typeof(ConditionalContract))))
            {
                //otherwise we get stuck, since the empty trace does not change a conditional first
                actions.RemoveAll(A => A.Count == 1 && A[0] == Action.empty);
            }


            for(int i = 0; i < actions.Count; i++)
            {
                //Contract nextContract = contractReduced.Transition(actions[i]);

                automaton.AddTransition(contract, actions[i]);
            }

        }

        public override string ToString()
        {
            return contract.ToString();
        }

        public void mustConflictAnalysis()
        {
            for (int i = 0; i < allIndividualConcurrentContracts.Count(); i++)
            {
                for (int j = 1; j < allIndividualConcurrentContracts.Count(); j++)
                {
                    if (ca.MustConflict(allIndividualConcurrentContracts[i], allIndividualConcurrentContracts[j]))
                    {
                        mustConflict = true;
                        return;
                    }
                }
            }

            mustConflict = false;
        }

        public void mayConflictAnalysis()
        {
            for (int i = 0; i < allIndividualConcurrentContracts.Count(); i++)
            {
                for (int j = 1; j < allIndividualConcurrentContracts.Count(); j++)
                {
                    if (ca.MayConflict(allIndividualConcurrentContracts[i], allIndividualConcurrentContracts[j]))
                    {
                        mayConflict = true;
                        return;
                    }
                }
            }

            mayConflict = false;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(typeof(State)))
            {
                if (((State)obj).getContract().Equals(this.contract))
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

        public override int GetHashCode()
        {
            return contract.ToString().GetHashCode();
        }
    }
}
