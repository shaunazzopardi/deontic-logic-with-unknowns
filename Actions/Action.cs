using System.Collections.Generic;
using System.Linq;

namespace ContractFormalLanguage
{
    abstract public class Action
    {
        public static Action empty = new KnownAction("");

        public string text { get; set; }

        public Action(string text)
        {
            this.text = text;
        }

        public override string ToString()
        {
            return this.text;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(typeof(Action)))
            {
                return this.text == ((Action)obj).text;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        //from http://rosettacode.org/wiki/Power_set#C.23
        public static List<List<Action>> permutation(List<Action> actions)
        {
            IEnumerable<IEnumerable<Action>> powerSet = (IEnumerable<IEnumerable<Action>>)(GetPowerSet<Action>(actions));

            List<List<Action>> permutations = new List<List<Action>>();

            foreach (IEnumerable<Action> set in powerSet)
            {
                permutations.Add(set.ToList());
            }

            return permutations;
        }
        //from http://rosettacode.org/wiki/Power_set#C.23
        public static IEnumerable<IEnumerable<T>> GetPowerSet<T>(List<T> list)
        {
            return from m in Enumerable.Range(0, 1 << list.Count)
                   select
                       from i in Enumerable.Range(0, list.Count)
                       where (m & (1 << i)) != 0
                       select list[i];
        }

        public static string actionSetToString(List<Action> actionSet, List<Action> allActions)
        {
            if (allActions == null) allActions = new List<Action>();

            if (actionSet.Count == 0)
            {
                return "{}";
            }
            else
            {
                string setString = "";

                
                for (int i = 0; i < actionSet.Count; i++)
                {
                    string rep = "" + allActions.IndexOf(actionSet[i]);

                    if (rep == "-1")
                    {
                        rep = actionSet[i].ToString();
                    }

                    setString += rep + ",";
                }

                return "{" + setString.Substring(0, setString.Count() - 1) + "}";
            }
        }

    }
}