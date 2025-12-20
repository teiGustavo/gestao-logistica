# Como rodar o projeto

Este repositório contém uma aplicação fullstack:
- Backend: `GestaoLogisticaAPI` (ASP.NET Core, .NET 9)
- Frontend: `GestaoLogisticaVue` (Vue 3 + Vite)
- Banco de dados: MySQL — o script de criação está em `GestaoLogisticaAPI/gestao_logistica.sql`

Resumo rápido
- Backend entra pelo diretório `GestaoLogisticaAPI` e usa .NET 9 (TargetFramework: net9.0).
- Frontend entra pelo diretório `GestaoLogisticaVue` e usa Node.js (consulte `package.json` para a versão recomendada).
- O arquivo SQL `GestaoLogisticaAPI/gestao_logistica.sql` cria o schema `gestao_logistica` e todas as tabelas; use-o para popular/criar o banco localmente.

Checklist (o que este README cobre)
- Pré-requisitos e como checar versões
- Diferença entre backend e frontend
- Instruções passo-a-passo para rodar o backend
- Como criar o banco usando o arquivo `.sql` (MySQL)
- Alternativa: usar migrações EF Core
- Instruções para rodar o frontend
- Verificações/health checks

1) Pré-requisitos
- .NET SDK 9 (compatível com TargetFramework net9.0). Verifique no `GestaoLogisticaAPI/GestaoLogisticaAPI.csproj`.
- Node.js (versão recomendada conforme `GestaoLogisticaVue/package.json` — engines: "node": "^20.19.0 || >=22.12.0"). Em resumo: Node 20.x (a partir de 20.19.0) ou Node 22+.
- npm (vem com Node) ou yarn
- MySQL (servidor local ou remoto) — o projeto espera um MySQL compatível (o script e a connection string usam sintaxe MySQL)
- (Opcional) dotnet-ef para aplicar migrações: `dotnet tool install --global dotnet-ef`

2) Como checar versões (comandos que você pode rodar no Windows — cmd.exe ou PowerShell)

- .NET SDK:
```
dotnet --version
dotnet --list-sdks
```
- EF Core tools (se estiver instalado):
```
dotnet ef --version
```
- Node / npm:
```
node -v
npm -v
```

Observação: tentei checar as versões neste ambiente automatizado, mas não foi possível detectar versões do .NET/Node daqui; execute os comandos acima no seu terminal local e confirme. Use .NET 9 e Node 20+ conforme indicado.

3) Diferença entre Backend e Frontend
- Backend (`GestaoLogisticaAPI`)
  - Implementado em C# com ASP.NET Core (TargetFramework net9.0).
  - Usa EntityFramework Core com o provedor Pomelo para MySQL (`Pomelo.EntityFrameworkCore.MySql`).
  - Expõe APIs REST sob o prefixo global `api/v1` (ver `Program.cs` onde a convenção é adicionada).
  - Swagger disponível em ambiente de desenvolvimento (endereço: `http://localhost:<porta>/swagger` — veja abaixo).
  - Porta padrão: quando executado com `dotnet run` a porta é determinada pelo `launchSettings.json` ou pelo ASPNETCORE_URLS/launch binding. Se não configurado, o Kestrel escolhe portas dinâmicas; geralmente a API ficará disponível em `http://localhost:5000` ou `https://localhost:5001`.

- Frontend (`GestaoLogisticaVue`)
  - SPA em Vue 3 + Vite.
  - Comandos de desenvolvimento via `npm run dev` que inicia o servidor de desenvolvimento do Vite (normalmente `http://localhost:5173`, mas pode mudar).
  - A aplicação consome a API no backend (configure a URL da API nas variáveis/serviços do frontend se necessário).

4) Configuração da connection string (Backend)
- O arquivo `GestaoLogisticaAPI/appsettings.json` já contém uma connection string exemplo:

```
"ConnectionStrings": {
  "DefaultConnection": "server=127.0.0.1;port=3306;database=gestao_logistica;user=root;password=root;SslMode=None"
}
```

- Atualize `appsettings.json` com as credenciais/host corretos ou prefira configurar via variável de ambiente:

Em cmd.exe (temporário, apenas para sessão atual):
```
set ConnectionStrings__DefaultConnection="server=127.0.0.1;port=3306;database=gestao_logistica;user=root;password=root;SslMode=None"
```

Em PowerShell (temporário, apenas para sessão atual):
```
$env:ConnectionStrings__DefaultConnection = "server=127.0.0.1;port=3306;database=gestao_logistica;user=root;password=root;SslMode=None"
```

5) Criar o banco MySQL usando o arquivo `.sql`
- O arquivo que cria o banco está em `GestaoLogisticaAPI/gestao_logistica.sql` e já cria o schema `gestao_logistica` e todas as tabelas.
- Usando o cliente MySQL (instale `mysql` CLI):

Em cmd.exe / PowerShell (a partir do diretório raiz do projeto):
```
mysql -u <usuario> -p < GestaoLogisticaAPI\gestao_logistica.sql
```
- Exemplo (usuário root):
```
mysql -u root -p < GestaoLogisticaAPI\gestao_logistica.sql
```
- Você também pode abrir o arquivo no MySQL Workbench ou outro cliente e executar o script.

Observação: o script define `CREATE DATABASE IF NOT EXISTS gestao_logistica` — portanto ele criará o banco se não existir.

6) Alternativa: criar o banco via migrações EF Core
- Se preferir usar EF Core migrations, certifique-se de ter o `dotnet-ef` instalado:
```
dotnet tool install --global dotnet-ef
```
- Depois, no diretório do backend:
```
cd GestaoLogisticaAPI
dotnet restore
dotnet ef database update
```
- Isso aplicará as migrações presentes no projeto (se houver). Se não houver migrações, você pode criar uma com `dotnet ef migrations add InitialCreate` (opcional) e depois aplicar.

7) Passos para rodar o Backend (modo desenvolvimento)
- No terminal (cmd.exe / PowerShell):
```
cd GestaoLogisticaAPI
dotnet restore
dotnet build
# Certifique-se que o banco 'gestao_logistica' existe (via .sql ou migrações)
dotnet run
```
- A API estará disponível normalmente em `http://localhost:5000` ou `https://localhost:5001` — verifique a saída do `dotnet run` e `launchSettings.json` em `GestaoLogisticaAPI/Properties/launchSettings.json`.
- Em ambiente de desenvolvimento, o Swagger UI estará disponível em `/swagger`.

8) Passos para rodar o Frontend (modo desenvolvimento)
- No terminal:
```
cd GestaoLogisticaVue
npm install
npm run dev
```
- O Vite iniciará o servidor de desenvolvimento e exibirá a URL (ex.: `http://localhost:5173`).

9) Verificações rápidas (smoke tests)
- Backend: acesse `http://localhost:5000/swagger` (ou a porta indicada) para ver a documentação e testar endpoints.
- Frontend: abra a URL do Vite (ex.: `http://localhost:5173`) e verifique se a aplicação carrega.
- Banco: conecte ao MySQL e rode `SHOW DATABASES;` e `USE gestao_logistica; SHOW TABLES;` para confirmar as tabelas criadas.

10) Problemas comuns e dicas
- Se receber erro de conexão MySQL, cheque host/porta/usuário/senha e se o servidor MySQL aceita conexões TCP (e não apenas socket).
- No Windows, se usar Docker para MySQL, exponha a porta 3306 e use a connection string apontando para `127.0.0.1` ou `host.docker.internal` conforme o caso.
- Se as migrações não existirem no projeto, importe o SQL diretamente; se preferir manter migrações, gere-as a partir do modelo DbContext.

11) Comandos úteis resumidos
- Verificar versões:
```
# .NET
dotnet --version
# EF tools
dotnet ef --version
# Node / npm
node -v
npm -v
```
- Rodar backend:
```
cd GestaoLogisticaAPI
dotnet restore
dotnet build
dotnet run
```
- Importar banco via SQL:
```
mysql -u root -p < GestaoLogisticaAPI\gestao_logistica.sql
```
- Usar migrações EF Core:
```
dotnet tool install --global dotnet-ef
cd GestaoLogisticaAPI
dotnet ef database update
```
- Rodar frontend:
```
cd GestaoLogisticaVue
npm install
npm run dev
```