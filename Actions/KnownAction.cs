using System.Collections;
using System.Linq;

namespace ContractFormalLanguage
{
    public class KnownAction : Action
    {
        public KnownAction(string text)
            : base(text)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(KnownAction))
            {
                if (((KnownAction)obj).text.Equals(this.text))
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

        public override string ToString()
        {
            return this.text;
        }

    }
}