import { useEffect, useState } from "react";
import type { Person } from "./types/Person";
import type { Transaction } from "./types/Transaction"
import type { Summary } from "./types/Summary";
import api from "./services/api";

function App() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [summary, setSummary] = useState<Summary | null> (null);
  const [name, setName] = useState("");
  const [age, setAge] = useState(0);
  const [description, setDescription] = useState("");
  const [value, setValue] = useState(0);
  const [type, setType] = useState(0);
  const [personId, setPersonId] = useState(0);


  async function loadPersons() {
    const response = await api.get("/Person");
    setPersons(response.data);
    if (response.data.length > 0) {
      setPersonId(response.data[0].id);
    }
  }

  async function loadTransactions() {
    const response = await api.get("/Transaction");
    setTransactions(response.data);
  }

  async function loadSummary(){
    const response = await api.get("/Person/Summary")
    setSummary(response.data)
  }
  async function createPerson() {
    await api.post("/Person", { name, age });
    setName("")
    setAge(0);
    await loadPersons();
  }

  async function deletePerson(id: number) {
    await api.delete(`/Person/${id}`);
    await loadPersons();
    await loadTransactions();
    await loadSummary();
  }

  async function createTransaction() {
    await api.post("/Transaction", { description, value, type, personId });
    setDescription("");
    setValue(0);
    setType(0);
    setPersonId(0);
    await loadTransactions();
  }

  useEffect(() => {
    loadPersons();
    loadTransactions();
    loadSummary();

  }, []);
  return (
    <>
      <h1>Controle de Gastos</h1>
      <input
        value={name}
        onChange={(e) => setName(e.target.value)}
      />
      <input
        type="number"
        value={age}
        onChange={(e) => setAge(parseInt(e.target.value))}
      />
      <button onClick={createPerson}>
        Cadastrar Pessoa</button>
      {persons.map((person) => (
        <div key={person.id}>
          <p>
            {person.name} - {person.age} anos
          </p>
          <button onClick={() => deletePerson(person.id)}>Excluir Pessoa</button>
        </div>
      ))}
      <select value={personId} onChange={(e) => setPersonId(parseInt(e.target.value))}>
        {persons.map((person) => (
          <option key={person.id} value={person.id}>
            {person.name}
          </option>
        ))}
      </select>
      <input value={description} onChange={(e) => setDescription(e.target.value)} />
      <input type="number" value={value} onChange={(e) => setValue(parseFloat(e.target.value))} />
      <select value={type} onChange={(e) => setType(parseInt(e.target.value))}>
        <option value={1}>
          Receita
        </option>
        <option value={0}>
          Despesa
        </option>

      </select>
      <button onClick={createTransaction}>Cadastrar transação</button>
      {transactions.map((transaction) => {
        const person = persons.find(
          p => p.id === transaction.personId
        );  
        return (
          <div key={transaction.id}>
            {transaction.description} - {transaction.value} - {transaction.type === 0 ? "Despesa" : "Receita"} - {person?.name}
          </div>
        )
      })}
      {summary &&(
        <div>
          {summary.persons.map((person)=>
          <div key={person.name}>
            <p>{person.name}</p>
            <p>Receitas: {person.totalIncome}</p>
            <p>Despesas: {person.totalExpense}</p>
            <p> Saldo: {person.balance}</p>
            </div>
          )}
           <h2>Resumo Geral</h2>

        <p>Receitas: {summary.totalIncome}</p>
        <p>Despesas: {summary.totalExpense}</p>
        <p>Saldo: {summary.balance}</p>

        </div>
      )}
    </>
  );
}

export default App;
