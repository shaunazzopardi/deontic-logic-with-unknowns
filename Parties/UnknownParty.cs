namespace ContractFormalLanguage
{
    public class UnknownParty : Party
    {
        public UnknownParty(string text)
            : base(text)
        {
        }

        public override string ToString()
        {
            return "?";
        }
    }
}