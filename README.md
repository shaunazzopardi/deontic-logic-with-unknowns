# deontic-logic-with-unknowns

This is an implementation of an action-based deontic logic language that allows for unknowns, i.e. a partial logic of norms. The semantics are described in:

Shaun Azzopardi, Albert Gatt and Gordon Pace, [Reasoning about partial contracts](https://www.researchgate.net/publication/311651290_Reasoning_about_Partial_Contracts), Jurix 2016, Nice, France.

Usage:

Create objects for each contract (in /Contracts/), but use the ContractFactory for operators between contracts (concurrent, composed and reparation contracts) to create such contracts in their minimized equivalent state. This allows for efficient analysis. Given such a contract it can then be analysed for conflicts.
