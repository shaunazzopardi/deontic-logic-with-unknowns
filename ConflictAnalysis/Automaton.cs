using ContractFormalLanguage.ConflictAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractFormalLanguage.ConflictAnalysis
{
   public class Automaton
    {
        //public List<KnownAction[]> mutuallyExclusiveActions;
        public List<Action> actions;
        Dictionary<string, State> states;
        State startState;
        Dictionary<string, Transition> transitions;

        public ConflictAnalyzer ca;

        public Automaton(Contract startContract)
        {
            ca = new ConflictAnalyzer();

            states = new Dictionary<string, State>();

            transitions = new Dictionary<string, Transition>();

            actions = extractAllActionsInContract(startContract);

            ca.setActions(actions);

            startState = new State(this, startContract);//.Reduction());

            states.Add(startState.ToString(), startState);

            startState.Explore();
        }

        public void AddTransition(Contract contract, List<Action> actionSet)
        {
            State headState;
            if (!states.TryGetValue(contract.ToString(), out headState))
            {
                headState = new State(this, contract);
                states.Add(headState.ToString(), headState);
            }

            bool exploreChild = false;
            State childState;
            Contract transitioned = contract.Transition(actionSet);//.Reduction();
            if (!states.TryGetValue(transitioned.ToString(), out childState))
            {
                childState = new State(this, transitioned);
                states.Add(childState.ToString(), childState);
                exploreChild = true;
            }

            Transition tran;
            string key = headState.ToString() + "----" + Action.actionSetToString(actionSet, actions) + "---->" + childState.ToString();
            if (!transitions.TryGetValue(key, out tran))
            {
                tran = new Transition(headState, actionSet, childState);
                transitions.Add(key, tran);
            }


            if (exploreChild)
            {
                childState.Explore();
            }
        }
        public List<Action> extractAllActionsInContract(Contract contract)
        {
            List<Action> actions = contract.getAllActions();

            HashSet<Action> uniqueActions = new HashSet<Action>(actions);

            return uniqueActions.ToList();
        }

        //public State Transition(State state, List<Action> actionSet)
        //{
        //    State nextState = transitions.Find(t => t.isHead(state)).;
        //}
    }
}
