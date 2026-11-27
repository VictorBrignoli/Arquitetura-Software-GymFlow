# GymFlow: Gestão de Academia em Microsserviços

Back-end de um sistema de gestão de academia, construído com arquitetura de microsserviços (.NET, SQLite, REST) para a avaliação de Sistemas Distribuídos, UniSATC.

## Aplicação publicada (Render)

| Serviço | URL base | Documentação interativa |
|---|---|---|
| Alunos | https://PREENCHER-alunos.onrender.com | `/scalar` |
| Exercícios | https://gymflow-exercicios.onrender.com	 | `/scalar` |
| Treinos | https://gymflow-treinos.onrender.com	 | `/scalar` |
| Avaliações Físicas | https://gymflow-avaliacoes.onrender.com	 | `/scalar` |

> Os serviços estão no plano gratuito do Render e "dormem" após alguns minutos sem acesso. O primeiro acesso a cada um pode levar cerca de 1 minuto.

## Documento de Requisitos

### a. Propósito do sistema

O GymFlow permite que uma academia cadastre seus alunos, mantenha um catálogo de exercícios, monte treinos personalizados e acompanhe a evolução física de cada aluno por meio de avaliações periódicas. O sistema também gera planos de treino com Inteligência Artificial, a partir do objetivo, do nível e da última avaliação física do aluno.

### b. Quais são os usuários

| Usuário | Descrição |
|---|---|
| **Instrutor** | Cadastra alunos e exercícios, monta e edita treinos, registra avaliações físicas e solicita treinos gerados por IA. |
| **Aluno** | Consulta seus treinos e o histórico de avaliações físicas. |
| **Administrador** | Gerencia o catálogo de exercícios e ativa ou inativa cadastros de alunos. |

### c. Requisitos funcionais

**Alunos**
- **RF01.** O sistema deve permitir cadastrar alunos com nome, e-mail, data de nascimento, objetivo, nível, peso atual, altura e meta de peso.
- **RF02.** O sistema deve permitir listar os alunos e consultar um aluno pelo seu identificador.
- **RF03.** O sistema deve permitir editar os dados de um aluno, incluindo ativar ou inativar o cadastro.
- **RF04.** O sistema deve atualizar o peso atual do aluno sempre que uma nova avaliação física for registrada.

**Exercícios**
- **RF05.** O sistema deve permitir cadastrar e editar exercícios com nome, grupo muscular, equipamento, nível de dificuldade e descrição, incluindo ativar ou inativar o exercício.
- **RF06.** O sistema deve permitir listar os exercícios, com filtro por grupo muscular e por situação (ativo ou inativo).
- **RF07.** O sistema deve permitir consultar um exercício pelo seu identificador.
- **RF08.** O sistema deve disponibilizar um catálogo inicial de exercícios ao ser iniciado.

**Treinos**
- **RF09.** O sistema deve permitir criar um treino para um aluno, recusando a criação se o aluno não existir ou estiver inativo. *(Integração 1)*
- **RF10.** O sistema deve permitir adicionar exercícios a um treino, informando séries, repetições, descanso e ordem, recusando exercícios inexistentes ou inativos. *(Integração 2)*
- **RF11.** O sistema deve permitir remover um exercício de um treino.
- **RF12.** O sistema deve permitir consultar um treino, exibindo o nome e o objetivo do aluno e o nome e o grupo muscular de cada exercício.
- **RF13.** O sistema deve permitir listar todos os treinos de um aluno, com as mesmas informações do RF12.
- **RF14.** O sistema deve gerar automaticamente um treino com Inteligência Artificial, com base no objetivo, no nível e na última avaliação física do aluno, usando apenas exercícios ativos do catálogo. Se a IA não responder, o sistema deve gerar um treino-modelo adequado ao nível do aluno.

**Avaliações Físicas**
- **RF15.** O sistema deve permitir registrar uma avaliação física com peso, percentual de gordura, medidas de cintura, quadril e braço e observações, recusando o registro se o aluno não existir ou estiver inativo.
- **RF16.** Ao registrar uma avaliação, o sistema deve enviar o novo peso para o cadastro do aluno. Se o envio falhar, a avaliação deve ficar marcada como pendente de sincronização. *(Integração 3)*
- **RF17.** O sistema deve permitir reenviar ao cadastro dos alunos o peso das avaliações pendentes de sincronização.
- **RF18.** O sistema deve permitir consultar uma avaliação pelo seu identificador e o histórico de avaliações de um aluno, da mais recente para a mais antiga.

**Geral**
- **RF19.** Cada microsserviço deve disponibilizar um endpoint de verificação de funcionamento (`/health`).

### Integrações entre os microsserviços

| # | Tipo | Origem → Destino | Gatilho |
|---|---|---|---|
| 1 | Busca simples | Treinos → Alunos | Criação de treino |
| 2 | Busca simples | Treinos → Exercícios | Inclusão de exercício no treino |
| 3 | Alteração de dados | Avaliações → Alunos | Registro de avaliação física |

A geração de treino com IA (RF14) também consulta Alunos, Exercícios e Avaliações.

---

## Descritivo Técnico

### Visão geral da arquitetura

O GymFlow é composto por **4 microsserviços independentes**, todos criados a partir do template de microsserviço da disciplina (.NET, SQLite, REST). Cada serviço:

- tem sua própria API REST e seu próprio banco SQLite (nenhum serviço acessa o banco de outro);
- é publicado como um serviço separado no Render;
- guarda apenas o **ID** dos registros que pertencem a outros serviços (por exemplo, `AlunoId` em Treinos) e consulta o serviço dono do dado por HTTP quando precisa dele;
- recebe as URLs dos demais serviços por variáveis de ambiente.

```
                  ┌──────────────┐
          ┌──────▶│    Alunos    │◀──────┐
          │ busca │              │ altera│
          │   1   └──────────────┘       │
   ┌──────┴───────┐                ┌─────┴──────────┐
   │   Treinos    │ ─ ─ (IA) ─ ─ ▶ │   Avaliações   │
   └──────┬───────┘                │    Físicas     │
          │ busca 2                └────────────────┘
          ▼
   ┌──────────────┐
   │  Exercícios  │
   └──────────────┘
```

### Tecnologias

| Item | Tecnologia |
|---|---|
| Linguagem / framework | C# com .NET 10 (template da disciplina) |
| Banco de dados | SQLite com Entity Framework Core, um arquivo por serviço |
| Comunicação | REST/JSON via `HttpClient` (`IHttpClientFactory`) |
| Documentação da API | OpenAPI + Scalar (`/scalar`) |
| Hospedagem | Render (um Web Service com Docker por microsserviço) |
| Código-fonte | GitHub (repositório único, uma pasta por serviço) |
| IA | Google Gemini (API REST), chamada pelo serviço de Treinos |

### Microsserviços

#### 1. Alunos
**Função:** é dono do cadastro de alunos e do estado atual deles (objetivo, nível, peso atual, meta).

| Item | Detalhe |
|---|---|
| Entidade | `Aluno`: Id, Nome, Email, DataNascimento, Objetivo, Nivel, PesoAtual, AlturaCm, MetaPeso, Ativo |
| Endpoints | `GET /api/alunos`, `GET /api/alunos/{id}`, `POST /api/alunos`, `PUT /api/alunos/{id}`, `PATCH /api/alunos/{id}/medidas`, `GET /health` |
| Chama outros serviços | Nenhum |
| É chamado por | Treinos (busca 1 e IA), Avaliações (validação e alteração de dados) |

#### 2. Exercícios
**Função:** mantém o catálogo de exercícios disponíveis para montar treinos. Inicia com 40 exercícios cadastrados (*seed*).

| Item | Detalhe |
|---|---|
| Entidade | `Exercicio`: Id, Nome, GrupoMuscular, Equipamento, NivelDificuldade, Descricao, Ativo |
| Endpoints | `GET /api/exercicios` (filtros `?grupo=` e `?ativo=`), `GET /api/exercicios/{id}`, `POST /api/exercicios`, `PUT /api/exercicios/{id}`, `GET /health` |
| Chama outros serviços | Nenhum |
| É chamado por | Treinos (busca 2 e IA) |

#### 3. Treinos
**Função:** monta e armazena os treinos dos alunos e gera treinos com IA. É o serviço que mais se integra com os outros.

| Item | Detalhe |
|---|---|
| Entidades | `Treino`: Id, AlunoId, Nome, DataCriacao, Ativo<br>`TreinoItem`: Id, TreinoId, ExercicioId, Series, Repeticoes, DescansoSegundos, Ordem |
| Endpoints | `POST /api/treinos`, `GET /api/treinos/{id}`, `GET /api/treinos/aluno/{alunoId}`, `POST /api/treinos/{id}/itens`, `DELETE /api/treinos/{id}/itens/{itemId}`, `POST /api/treinos/gerar-ia`, `GET /health` |
| Chama outros serviços | Alunos, Exercícios, Avaliações (só na geração com IA) |
| É chamado por | Nenhum |

#### 4. Avaliações Físicas
**Função:** guarda o histórico de medições de cada aluno e propaga o peso mais recente para o cadastro do aluno.

| Item | Detalhe |
|---|---|
| Entidade | `Avaliacao`: Id, AlunoId, Data, Peso, PercentualGordura, Cintura, Quadril, Braco, Observacoes, Sincronizada |
| Endpoints | `POST /api/avaliacoes`, `GET /api/avaliacoes/{id}`, `GET /api/avaliacoes/aluno/{alunoId}`, `POST /api/avaliacoes/sincronizar`, `GET /health` |
| Chama outros serviços | Alunos |
| É chamado por | Treinos (IA, para obter a última avaliação) |

### Fluxo das integrações

**Integração 1, busca simples (Treinos → Alunos)**
1. `POST /api/treinos` recebe `alunoId` e nome do treino.
2. Treinos chama `GET /api/alunos/{id}`.
3. Se o aluno não existe (404) ou está inativo (400), retorna erro e não cria o treino.
4. Ao consultar um treino, os dados do aluno (nome e objetivo) são buscados em Alunos na hora, sem cópia local.

**Integração 2, busca simples (Treinos → Exercícios)**
1. `POST /api/treinos/{id}/itens` recebe `exercicioId`, séries, repetições e descanso.
2. Treinos chama `GET /api/exercicios/{id}`.
3. Se o exercício não existe (404) ou está inativo (400), o item é rejeitado.
4. Ao listar o treino, cada item é enriquecido com nome e grupo muscular vindos de Exercícios.

**Integração 3, alteração de dados (Avaliações → Alunos)**
1. `POST /api/avaliacoes` recebe `alunoId` e as medidas.
2. Avaliações valida o aluno com `GET /api/alunos/{id}`.
3. A avaliação é salva no banco de Avaliações.
4. Avaliações chama `PATCH /api/alunos/{id}/medidas` com o novo peso.
5. Alunos atualiza o `PesoAtual` do cadastro.
6. Se o `PATCH` falhar, a avaliação fica com `Sincronizada = false`; o `POST /api/avaliacoes/sincronizar` reenvia as pendentes.

**Geração de treino com IA (Treinos)**
1. `POST /api/treinos/gerar-ia` recebe o `alunoId`.
2. Treinos busca o aluno (Alunos), a última avaliação (Avaliações) e os exercícios ativos (Exercícios).
3. Monta o prompt com esses dados, sem enviar nome nem e-mail do aluno, e pede à IA um treino em JSON usando somente IDs de exercícios do catálogo.
4. Valida a resposta e descarta itens com exercício inexistente ou valores fora dos limites.
5. Salva o treino como qualquer outro (pode ser editado depois) e indica na resposta se ele foi gerado pela IA (`geradoPorIa`).
6. Se a IA falhar ou a chave não estiver configurada, gera um treino-modelo pelo nível do aluno.

### Configuração (variáveis de ambiente)

| Serviço | Variável | Uso |
|---|---|---|
| Treinos | `ALUNOS_URL` | URL base do serviço de Alunos |
| Treinos | `EXERCICIOS_URL` | URL base do serviço de Exercícios |
| Treinos | `AVALIACOES_URL` | URL base do serviço de Avaliações |
| Treinos | `IA_API_KEY` | Chave da API do Gemini (nunca versionada) |
| Treinos | `IA_MODELO` | Modelo do Gemini (opcional; padrão `gemini-3.5-flash-lite`) |
| Avaliações | `ALUNOS_URL` | URL base do serviço de Alunos |
| Todos | `ConnectionStrings__DefaultConnection` | Caminho do arquivo SQLite (opcional) |

As URLs são informadas sem barra no final. Portas locais: Alunos `5001`, Exercícios `5002`, Treinos `5003`, Avaliações `5004`.

### Estrutura do repositório

```
Arquitetura-Software-GymFlow/
├── README.md                     # requisitos + descritivo técnico
├── AppTemplate/                  # template da disciplina (referência)
├── alunos-service/
│   ├── Dockerfile
│   └── AlunosService/
├── exercicios-service/
│   ├── Dockerfile
│   └── ExerciciosService/
├── treinos-service/
│   ├── Dockerfile
│   └── TreinosService/
└── avaliacoes-service/
    └── avaliacoes-service/
        ├── Dockerfile
        └── AvaliacoesService/
```

### Como executar localmente

Em um terminal para cada serviço:

```bash
cd alunos-service/AlunosService && dotnet run --launch-profile http
cd exercicios-service/ExerciciosService && dotnet run --launch-profile http
cd avaliacoes-service/avaliacoes-service/AvaliacoesService && dotnet run --launch-profile http
cd treinos-service/TreinosService && dotnet run --launch-profile http
```

Cada serviço tem um arquivo `.http` com os testes das suas rotas e a documentação em `http://localhost:<porta>/scalar`.

### Tratamento de falhas

- Chamadas entre serviços usam timeout e tratamento de erro: se o serviço chamado não responder, o serviço de origem devolve **503** e não grava dados inconsistentes.
- **Escrita falha, leitura degrada:** ao criar treinos ou itens, se Alunos ou Exercícios estiver fora do ar, a operação é recusada (503). Ao consultar um treino, ele é devolvido mesmo assim, com `"(indisponível)"` no lugar dos nomes.
- Treinos busca os exercícios de um treino **um de cada vez**: o template compartilha a mesma instância do `DataContext` entre requisições, e chamadas simultâneas ao serviço de Exercícios falhavam.
- Na integração 3, a avaliação nunca se perde: se o `PATCH` em Alunos falhar, ela fica pendente (`Sincronizada = false`) até ser reenviada.
- No plano gratuito do Render os serviços ficam inativos após um período sem acesso e o primeiro acesso é lento, por isso o timeout das chamadas entre serviços é de 60 segundos.
- Os bancos SQLite são recriados a cada deploy no plano gratuito; por isso cada serviço aplica as migrations e carrega dados de demonstração (*seed*) na inicialização.

### Extras (item 7 do enunciado)

**Implementados**
- **Integração com IA:** geração de treinos personalizados com o Google Gemini (`POST /api/treinos/gerar-ia`), com validação da resposta e treino-modelo como alternativa.
- **Observabilidade (básica):** endpoint `/health` em todos os serviços e documentação interativa da API (`/scalar`).

**Possíveis evoluções**
- **Comunicação assíncrona:** migrar a integração 3 para uma fila de mensagens (evento `AvaliacaoRegistrada`).
- **Observabilidade completa:** logs estruturados e rastreamento distribuído entre os serviços.
