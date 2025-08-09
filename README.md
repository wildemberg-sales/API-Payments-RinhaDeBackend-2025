# API-Payments-RinhaDeBackend-2025
Este repositório é destinado ao projeto da Rinha de Backend edição de 2025

## Setup do Projeto

Criar Migration:
````sh
dotnet ef migrations add Initial_migration -p .\ApiPaymentServices\ -s .\ApiPayments\
````

Atualizar o Banco:
````sh
dotnet ef database update -p .\ApiPaymentServices\ -s .\ApiPayments\
````