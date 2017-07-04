using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractFormalLanguage.ConflictAnalysis
{
    public class Transition
    {
        State head;
        List<Action> actionSet;
        State next;
        Automaton automaton;

        public Transition(State head, List<Action> actionSet, State next)
        {
            this.automaton = head.automaton;

            this.head = head;
            this.actionSet = actionSet;
            this.next = next;
        }

        public State getHead()
        {
            return head;
        }

        public List<Action> getActionSet()
        {
            return actionSet;
        }
        public State getNext()
        {
            return next;
        }

        public override string ToString()
        {
            return head.ToString() + "----" + Action.actionSetToString(actionSet, automaton.actions) + "---->" + next.ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

    }
}
