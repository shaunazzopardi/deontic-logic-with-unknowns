using System.Collections.Generic;
using System.Linq;

namespace ContractFormalLanguage
{
    public interface Contract
    {
        Contract Transition(List<Action> actionSet);

        bool EquivalentWith(Contract otherContract);

        Contract Reduction();

        Contract SyntacticReduction();

        bool Equals(Contract otherContract);

        string ToString();

        Contract StartsWith();

        List<Contract> oneStepAwayContracts();

        List<Action> getAllActions();

        List<List<Action>> stepActionSets();

        bool isViolated();
        bool isSatisfied();
        bool isUnknown();
    }
}