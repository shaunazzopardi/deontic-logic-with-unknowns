namespace ContractFormalLanguage
{
    public class KnownParty : Party
    {
        public KnownParty(string text)
            : base(text)
        {
            this.number = Party.numberOfParties;

            numberOfParties++;
        }

        public override string ToString()
        {
            return this.text;
        }
    }
}