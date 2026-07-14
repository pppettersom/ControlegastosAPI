import type { PersonSummary } from "./PersonSummary";

export interface Summary{
    persons : PersonSummary[];
    totalIncome : number;
    totalExpense : number;
    balance : number; 
}