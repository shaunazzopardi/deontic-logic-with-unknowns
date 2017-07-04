using ContractFormalLanguage.ConflictAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContractFormalLanguage
{
    public class ConflictAnalyzer
    {
        Automaton automaton;

        public List<KnownAction[]> mutuallyExclusiveActions;
        public List<Action> actions;

        public void extractAllActionsInContract(Contract contract)
        {
            actions = contract.getAllActions();

            for (int i = 0; i < actions.Count; i++)
            {
                Action current = actions[i];
                

                    for (int j = 1; j < actions.Count; j++)
                    {
                        if (i != j && i < actions.Count && j < actions.Count)
                        {
                            if (actions[j].Equals(current))
                            {
                                actions.RemoveAt(j);
                                j--;
                            }
                        }
                    }
            }
        }
        public bool MayConflict(Contract firstContract, Contract secondContract)
        {
            //var firstStart = firstContract.StartsWith();
            //var secondStart = secondContract.StartsWith();

            Type firstType = firstContract.GetType();
            Type secondType = secondContract.GetType();

            //Second Axiom
            if (MustConflict(firstContract, secondContract))
            {
                return true;
            }
            else
            {
                //First Axiom & Third Axiom
                if (firstType.Equals(typeof(UnknownAction))
                    || secondType.Equals(typeof(UnknownAction)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool MustConflict(Contract first, Contract second)
        {
            bool mustConflict = false;

            Contract firstContract = first.StartsWith();
            Contract secondContract = second.StartsWith();

            Type firstType = firstContract.GetType();
            Type secondType = secondContract.GetType();

            if (firstType.Equals(typeof(ConcurrentContract)) && secondType.Equals(typeof(ConcurrentContract)))
            {
                List<Contract> firstContracts = ((ConcurrentContract)firstContract).setOfApplicableNonConcurrentContracts();

                List<Contract> secondContracts = ((ConcurrentContract)secondContract).setOfApplicableNonConcurrentContracts();

                foreach (Contract firstCon in firstContracts)
                {
                    foreach (Contract secondCon in secondContracts)
                    {
                        mustConflict = MustConflict(firstCon, secondCon);

                        if (mustConflict) return mustConflict;
                    }
                }
            }
            else
                if (firstType.Equals(typeof(ConcurrentContract)))
                {
                    List<Contract> firstContracts = ((ConcurrentContract)firstContract).setOfApplicableNonConcurrentContracts();

                    foreach (Contract firstCon in firstContracts)
                    {
                        mustConflict = MustConflict(firstCon, secondContract);

                        if (mustConflict) return mustConflict;
                    }
                }
                else if (secondType.Equals(typeof(ConcurrentContract)))
                {
                    List<Contract> secondContracts = ((ConcurrentContract)secondContract).setOfApplicableNonConcurrentContracts();

                    foreach (Contract secondCon in secondContracts)
                    {
                        mustConflict = MustConflict(firstContract, secondCon);

                        if (mustConflict) return mustConflict;
                    }
                }
                else if (firstType.Equals(typeof(ConditionalContract)))
                {
                    //types that can come here are [a]O(a), [a]P(a), [a]F(a)
                    //[a]C>C' and [a]C*>C' are resolved to [a](C>C') and [a](C*>C')
                    //in these cases first = firstReduced
                    //([a]C)&C' is resolved earlier when checking for concurrent contracts
                    //so 

                    bool containsEmpty = ((ConditionalContract)first).getCondition().containsEmpty();

                    if (containsEmpty)
                    {
                        mustConflict = MustConflict(((ConditionalContract)first).getContract(), secondContract);

                        if (mustConflict) return mustConflict;
                    }
                }
                else if (secondType.Equals(typeof(ConditionalContract)))
                {
                    //types that can come here are [a]O(a), [a]P(a), [a]F(a)
                    //[a]C>C' and [a]C*>C' are resolved to [a](C>C') and [a](C*>C')
                    //in these cases first = firstReduced
                    //([a]C)&C' is resolved earlier when checking for concurrent contracts
                    //so 

                    bool containsEmpty = ((ConditionalContract)second).getCondition().containsEmpty();

                    if (containsEmpty)
                    {
                        mustConflict = MustConflict(((ConditionalContract)second).getContract(), firstContract);

                        if (mustConflict) return mustConflict;
                    }
                }
                else
                {
                    if (firstType.Equals(typeof(SatisfiedContract)) || firstType.Equals(typeof(ViolatedContract)) || firstType.Equals(typeof(UnknownContract))
                        || secondType.Equals(typeof(SatisfiedContract)) || secondType.Equals(typeof(ViolatedContract)) || secondType.Equals(typeof(UnknownContract)))
                    {
                        mustConflict = false;

                        return mustConflict;
                    }

                    //First Axiom
                    else if (firstType.Equals(typeof(Permission)))
                    {
                        if (secondType.Equals(typeof(Prohibition)))
                        {
                            mustConflict = ((Permission)firstContract).GetAction().Equals(((Prohibition)secondContract).GetAction());

                            if (mustConflict) return mustConflict;
                        }
                    }
                    else if (firstType.Equals(typeof(Prohibition)))
                    {
                        if (secondType.Equals(typeof(Permission)))
                        {
                            mustConflict = ((Prohibition)firstContract).GetAction().Equals(((Permission)secondContract).GetAction());

                            if (mustConflict) return mustConflict;
                        }
                    }

                    //Second Axiom
                    else if (firstType.Equals(typeof(Obligation)))
                    {
                        if (secondType.Equals(typeof(Prohibition)))
                        {
                            mustConflict = ((Obligation)firstContract).GetAction().Equals(((Prohibition)secondContract).GetAction());

                            if (mustConflict) return mustConflict;
                        }
                    }
                    else if (firstType.Equals(typeof(Prohibition)))
                    {
                        if (secondType.Equals(typeof(Obligation)))
                        {
                            mustConflict = ((Prohibition)firstContract).GetAction().Equals(((Obligation)secondContract).GetAction());
                        }

                        if (mustConflict) return mustConflict;
                    }

                    //Third Axiom
                    else if (firstType.Equals(typeof(Obligation)))
                    {
                        if (secondType.Equals(typeof(Obligation)))
                        {
                            mustConflict = MutuallyExclusive(((Obligation)firstContract).GetAction(), ((Obligation)secondContract).GetAction());
                        }
                        if (mustConflict) return mustConflict;
                    }

                    //Fourth Axiom
                    else if (firstType.Equals(typeof(Obligation)))
                    {
                        if (secondType.Equals(typeof(Permission)))
                        {
                            mustConflict = MutuallyExclusive(((Obligation)firstContract).GetAction(), ((Permission)secondContract).GetAction());

                            if (mustConflict) return mustConflict;
                        }
                    }
                    else if (firstType.Equals(typeof(Permission)))
                    {
                        if (secondType.Equals(typeof(Obligation)))
                        {
                            mustConflict = MutuallyExclusive(((Permission)firstContract).GetAction(), ((Obligation)secondContract).GetAction());
                        }

                        if (mustConflict) return mustConflict;
                    }

                    //Fifth Axiom is symmetry, it is included in each axiom

                    //Sixth Axiom - Syntactic equivalence
                    //Contract firstReduced = firstContract.Reduction();
                    //Contract secondReduced = secondContract.Reduction();

                    //if (!second.Equals(secondContract))
                    //{
                    //    mustConflict = this.MustConflict(firstContract, second);
                    //}
                    //else if (!first.Equals(firstContract))
                    //{
                    //    mustConflict = this.MustConflict(first, secondContract);
                    //}

                    if (mustConflict) return mustConflict;

                }

            return mustConflict;
        }
        public bool MutuallyExclusive(List<Action> actions)
        {
            bool mutuallyExclusive = false;

            for (int i = 0; i < actions.Count - 1; i++)
            {
                for (int j = 0; j < actions.Count - 1; j++)
                {
                    if (i != j)
                    {
                        mutuallyExclusive = MutuallyExclusive(actions[i], actions[j]);
                        if (mutuallyExclusive == true) return true;
                    }
                }
            }

            return mutuallyExclusive;
        }
        public bool MutuallyExclusive(Action firstAction, Action secondAction)
        {
            if (firstAction.GetType().Equals(typeof(UnknownAction)) || secondAction.GetType().Equals(typeof(UnknownAction)))
            {
                return false;
            }

            if (mutuallyExclusiveActions == null) return false;

            bool mutuallyExclusive = mutuallyExclusiveActions.Any(actions => actions.Contains(firstAction) && actions.Contains(secondAction));

            return mutuallyExclusive;
        }
        public void setActions(List<Action> actions)
        {
            this.actions = actions;
        }

    }
}