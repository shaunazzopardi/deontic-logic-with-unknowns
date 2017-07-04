namespace ContractFormalLanguage
{
    public class UnknownAction : Action
    {
        public UnknownAction(string text) : base(text)
        {
        }

        public override string ToString()
        {
            return "?";
        }
    }
}