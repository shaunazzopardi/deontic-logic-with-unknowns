using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractFormalLanguage.Contracts
{
    public class ContractFactory
    {
        public static Contract CreateConcurrent(Contract contract, Contract otherContract)
        {
            Type contractType = contract.GetType();
            Type otherContractType = otherContract.GetType();

            if (contract.Equals(otherContract))
            {
                return contract;
            }
            else if (contractType.Equals(typeof(ViolatedContract))
                || otherContractType.Equals(typeof(ViolatedContract)))
            {
                return new ViolatedContract();
            }
            else if (contractType.Equals(typeof(UnknownContract))
                || otherContractType.Equals(typeof(UnknownContract)))
            {
                return new UnknownContract();
            }
            else if (contractType.Equals(typeof(SatisfiedContract))
                && otherContractType.Equals(typeof(SatisfiedContract)))
            {
                return otherContract;
            }
            else if (contractType.Equals(typeof(SatisfiedContract)))
            {
                return otherContract;
            }
            else if (otherContractType.Equals(typeof(SatisfiedContract)))
            {
                return contract;
            }
            else
            {
                int i = 0;
                HashSet<Contract> allContracts = new HashSet<Contract>();

                if (contractType.Equals(typeof(ConcurrentContract)))
                {
                    allContracts.UnionWith(new HashSet<Contract>(((ConcurrentContract)contract).setOfApplicableNonConcurrentContracts()));

                    i = allContracts.Count;
                }
                else
                {
                    allContracts.Add(contract);
                }

                if (otherContractType.Equals(typeof(ConcurrentContract)))
                {
                    HashSet<Contract> otherContracts = new HashSet<Contract>(((ConcurrentContract)otherContract).setOfApplicableNonConcurrentContracts());

                    allContracts.UnionWith(otherContracts);

                    if (i != 0)
                    {
                        int j = otherContracts.Count;

                        if (i + j == allContracts.Count)
                        {
                            return new ConcurrentContract(contract, otherContract);
                        }
                        else
                        {
                            List<Contract> allToList = allContracts.ToList().OrderByDescending(c => c).ToList();

                            Contract toReturn = allToList[0];

                            for (int k = 1; k < allContracts.Count; k++)
                            {
                                toReturn = new ConcurrentContract(toReturn, allToList[k]);
                            }
                        }
                    }
                }
                else
                {
                    allContracts.Add(otherContract);
                }
                        
                return new ConcurrentContract(contract, otherContract);
            }
        }
        public static Contract CreateComposed(Contract first, Contract second)
        {
            Type firstType = first.GetType();
            Type secondType = first.GetType();

            if (firstType.Equals(typeof(ViolatedContract)))
            {
                return new ViolatedContract();
            }
            else if (firstType.Equals(typeof(UnknownContract)))
            {
                return new UnknownContract();
            }
            else return new ComposedContract(first, second);
        }

        public static Contract CreateReparation(Contract first, Contract second)
        {
            Type firstType = first.GetType();
            Type secondType = first.GetType();

            //if (firstType.Equals(typeof(ViolatedContract)))
            //{
            //    return new ViolatedContract();
            //}
            //else 
            if (firstType.Equals(typeof(UnknownContract)))
            {
                return new UnknownContract();
            }
            else if (firstType.Equals(typeof(SatisfiedContract)))
            {
                return new SatisfiedContract();
            }
            else return new ComposedContract(first, second);
        }
    }
}
