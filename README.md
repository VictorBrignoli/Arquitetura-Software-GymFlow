# GymFlow: Gestão de Academia em Microsserviços

Back-end de um sistema de gestão de academia, construído com arquitetura de microsserviços (.NET, SQLite, REST) para a avaliação de Sistemas Distribuídos, UniSATC.

## Documento de Requisitos

### a. Propósito do sistema

O GymFlow permite que uma academia cadastre seus alunos, mantenha um catálogo de exercícios, monte treinos personalizados e acompanhe a evolução física de cada aluno por meio de avaliações periódicas.
### b. Quais são os usuários

| Usuário | Descrição |
|---|---|
| **Instrutor** | Cadastra alunos e exercícios, monta e edita treinos e registra avaliações físicas. |
| **Aluno** | Consulta seus treinos e o histórico de avaliações físicas. |
| **Administrador** | Gerencia o catálogo de exercícios e ativa ou inativa cadastros de alunos. |

### c. Requisitos funcionais

_A definir._

### Integrações entre os microsserviços

| # | Tipo | Origem → Destino | Gatilho |
|---|---|---|---|
| 1 | Busca simples | Treinos → Alunos | Criação de treino |
| 2 | Busca simples | Treinos → Exercícios | Inclusão de exercício no treino |
| 3 | Alteração de dados | Avaliações → Alunos | Registro de avaliação física |

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
   │   Treinos    │                │   Avaliações   │
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
| Linguagem / framework | C# com .NET (template da disciplina) |
| Banco de dados | SQLite, um arquivo por serviço |
| Comunicação | REST/JSON via `HttpClient` (`IHttpClientFactory`) |
| Hospedagem | Render (um Web Service por microsserviço) |
| Código-fonte | GitHub (repositório único, uma pasta por serviço) |

### Microsserviços

#### 1. Alunos
**Função:** é dono do cadastro de alunos e do estado atual deles (objetivo, nível, peso atual, meta).

| Item | Detalhe |
|---|---|
| Entidade | `Aluno`: Id, Nome, Email, DataNascimento, Objetivo, Nivel, PesoAtual, AlturaCm, MetaPeso, Ativo |
| Endpoints | `GET /api/alunos`, `GET /api/alunos/{id}`, `POST /api/alunos`, `PUT /api/alunos/{id}`, `PATCH /api/alunos/{id}/medidas` |
| Chama outros serviços | Nenhum |
| É chamado por | Treinos (busca 1), Avaliações (validação e alteração de dados) |

#### 2. Exercícios
**Função:** mantém o catálogo de exercícios disponíveis para montar treinos. Roda com dados iniciais (*seed*) de 30 a 50 exercícios.

| Item | Detalhe |
|---|---|
| Entidade | `Exercicio`: Id, Nome, GrupoMuscular, Equipamento, NivelDificuldade, Descricao, Ativo |
| Endpoints | `GET /api/exercicios`, `GET /api/exercicios?grupo={grupo}`, `GET /api/exercicios/{id}`, `POST /api/exercicios`, `PUT /api/exercicios/{id}` |
| Chama outros serviços | Nenhum |
| É chamado por | Treinos (busca 2) |

#### 3. Treinos
**Função:** monta e armazena os treinos dos alunos. É o serviço que mais se integra com os outros.

| Item | Detalhe |
|---|---|
| Entidades | `Treino`: Id, AlunoId, Nome, DataCriacao, Ativo<br>`TreinoItem`: Id, TreinoId, ExercicioId, Series, Repeticoes, DescansoSegundos, Ordem |
| Endpoints | `POST /api/treinos`, `GET /api/treinos/{id}`, `GET /api/treinos/aluno/{alunoId}`, `POST /api/treinos/{id}/itens`, `DELETE /api/treinos/{id}/itens/{itemId}` |
| Chama outros serviços | Alunos, Exercícios |
| É chamado por | Nenhum |

#### 4. Avaliações Físicas
**Função:** guarda o histórico de medições de cada aluno e propaga o peso mais recente para o cadastro do aluno.

| Item | Detalhe |
|---|---|
| Entidade | `Avaliacao`: Id, AlunoId, Data, Peso, PercentualGordura, Cintura, Quadril, Braco, Observacoes, Sincronizada |
| Endpoints | `POST /api/avaliacoes`, `GET /api/avaliacoes/{id}`, `GET /api/avaliacoes/aluno/{alunoId}` |
| Chama outros serviços | Alunos |
| É chamado por | Nenhum |

### Fluxo das integrações

**Integração 1, busca simples (Treinos → Alunos)**
1. `POST /api/treinos` recebe `alunoId` e nome do treino.
2. Treinos chama `GET /api/alunos/{id}`.
3. Se o aluno não existe ou está inativo, retorna erro e não cria o treino.
4. Ao consultar um treino, os dados do aluno (nome e objetivo) são buscados em Alunos na hora, sem cópia local.

**Integração 2, busca simples (Treinos → Exercícios)**
1. `POST /api/treinos/{id}/itens` recebe `exercicioId`, séries e repetições.
2. Treinos chama `GET /api/exercicios/{id}`.
3. Se o exercício não existe ou está inativo, o item é rejeitado.
4. Ao listar o treino, cada item é enriquecido com nome e grupo muscular vindos de Exercícios.

**Integração 3, alteração de dados (Avaliações → Alunos)**
1. `POST /api/avaliacoes` recebe `alunoId` e as medidas.
2. Avaliações valida o aluno com `GET /api/alunos/{id}`.
3. A avaliação é salva no banco de Avaliações.
4. Avaliações chama `PATCH /api/alunos/{id}/medidas` com o novo peso.
5. Alunos atualiza o `PesoAtual` do cadastro.
6. Se o `PATCH` falhar, a avaliação fica com `Sincronizada = false` e a sincronização é tentada novamente depois.

### Configuração (variáveis de ambiente)

| Serviço | Variável | Uso |
|---|---|---|
| Treinos | `ALUNOS_URL` | URL base do serviço de Alunos |
| Treinos | `EXERCICIOS_URL` | URL base do serviço de Exercícios |
| Avaliações | `ALUNOS_URL` | URL base do serviço de Alunos |
| Avaliações, Treinos, Alunos, Exercícios | `ConnectionStrings__DefaultConnection` | Caminho do arquivo SQLite |

Portas locais sugeridas: Alunos `5001`, Exercícios `5002`, Treinos `5003`, Avaliações `5004`.

### Estrutura do repositório

```
gymflow/
├── README.md              # requisitos + descritivo técnico
├── alunos-service/
├── exercicios-service/
├── treinos-service/
└── avaliacoes-service/
```

### Tratamento de falhas

- Chamadas entre serviços usam timeout e tratamento de erro: se o serviço chamado não responder, o serviço de origem devolve um erro claro (por exemplo, 503) e não grava dados inconsistentes.
- No plano gratuito do Render os serviços ficam inativos após um período sem acesso e o primeiro acesso é lento, por isso o timeout das chamadas entre serviços é maior (30 a 60 segundos).
- Os bancos SQLite são recriados a cada deploy no plano gratuito; por isso cada serviço aplica as migrations e carrega dados de demonstração (*seed*) na inicialização.

### Extras previstos (item 7 do enunciado)

- **Comunicação assíncrona:** migrar a integração 3 para uma fila de mensagens (evento `AvaliacaoRegistrada`), se o prazo permitir.
- **Observabilidade:** logs estruturados e endpoint `/health` em cada serviço.
