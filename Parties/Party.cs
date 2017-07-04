namespace ContractFormalLanguage
{
    abstract public class Party
    {
        public string text;
        protected int number;

        protected static int numberOfParties = 0;

        public Party(string text)
        {
            this.text = text;
        }
    }
}