import { useEffect, useState } from "react";
import "./App.css"
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
    loadSummary();
  }

  useEffect(() => {
    loadPersons();
    loadTransactions();
    loadSummary();

  }, []);
  return (
    <div className="container">
  <h1>Controle de Gastos</h1>

  <div className="section">
    <h2>Cadastrar Pessoa</h2>

    <input
      placeholder="Nome"
      value={name}
      onChange={(e) => setName(e.target.value)}
    />
    <label>Idade:</label>
    <input
      type="number"
      placeholder="Idade"
      value={age}
      onChange={(e) => setAge(parseInt(e.target.value))}
    />

    <button onClick={createPerson}>
      Cadastrar Pessoa
    </button>
  </div>


  <div className="section">
    <h2>Pessoas cadastradas</h2>

    {persons.map((person) => (
      <div className="item" key={person.id}>
        <p>
          {person.name} - {person.age} anos
        </p>

        <button onClick={() => deletePerson(person.id)}>
          Excluir Pessoa
        </button>
      </div>
    ))}
  </div>


  <div className="section">
    <h2>Cadastrar Transação</h2>

    <select 
      value={personId} 
      onChange={(e) => setPersonId(parseInt(e.target.value))}
    >
      {persons.map((person) => (
        <option key={person.id} value={person.id}>
          {person.name}
        </option>
      ))}
    </select>

    <input
      placeholder="Descrição"
      value={description}
      onChange={(e) => setDescription(e.target.value)}
    />
    <label>Valor:</label>
    <input
      type="number"
      placeholder="Valor"
      value={value}
      onChange={(e) => setValue(parseFloat(e.target.value))}
    />

    <select 
      value={type} 
      onChange={(e) => setType(parseInt(e.target.value))}
    >
      <option value={1}>
        Receita
      </option>

      <option value={0}>
        Despesa
      </option>
    </select>

    <button onClick={createTransaction}>
      Cadastrar transação
    </button>
  </div>


  <div className="section">
    <h2>Transações</h2>

    {transactions.map((transaction) => {
      const person = persons.find(
        p => p.id === transaction.personId
      );

      return (
        <div className="item" key={transaction.id}>
          {transaction.description} - R$ {transaction.value} - 
          {transaction.type === 0 ? " Despesa" : " Receita"} -
          {" "}{person?.name}
        </div>
      )
    })}
  </div>


  {summary && (
    <div className="section">

      <h2>Resumo por pessoa</h2>

      {summary.persons.map((person) =>
        <div className="item" key={person.name}>
          <h3>{person.name}</h3>

          <p>
            Receitas: R$ {person.totalIncome}
          </p>

          <p>
            Despesas: R$ {person.totalExpense}
          </p>

          <p>
            Saldo: R$ {person.balance}
          </p>
        </div>
      )}


      <h2>Resumo Geral</h2>

      <div className="cards">

        <div className="card">
          <h3>Receitas</h3>
          <p>R$ {summary.totalIncome}</p>
        </div>

        <div className="card">
          <h3>Despesas</h3>
          <p>R$ {summary.totalExpense}</p>
        </div>

        <div className="card">
          <h3>Saldo</h3>
          <p>R$ {summary.balance}</p>
        </div>

      </div>

    </div>
  )}

</div>
  );
}

export default App;
