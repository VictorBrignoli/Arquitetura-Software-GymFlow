# Avaliações Físicas Service (GymFlow)

Microsserviço de avaliações físicas, no padrão do AppTemplate da disciplina (.NET 10, SQLite, REST).

## Rodar local
```bash
cd AvaliacoesService
# opcional (padrão: http://localhost:5001)
export ALUNOS_URL=http://localhost:5001      # PowerShell: $env:ALUNOS_URL="http://localhost:5001"
dotnet run --launch-profile http
```
- Porta: `http://localhost:5004`
- Documentação (Scalar): `http://localhost:5004/scalar`
- Testes rápidos: `AvaliacoesService/AvaliacoesService.http`
- O serviço de **Alunos** precisa estar rodando para registrar avaliações.

## Endpoints
| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/avaliacoes` | Registra avaliação (integração 3) |
| GET | `/api/avaliacoes/{id}` | Busca uma avaliação |
| GET | `/api/avaliacoes/aluno/{alunoId}` | Histórico do aluno (mais recente primeiro) |
| POST | `/api/avaliacoes/sincronizar` | Reenvia o peso das avaliações com `sincronizada = false` |
| GET | `/health` | Health check |

Body do POST:
```json
{ "alunoId": 1, "peso": 71.8, "percentualGordura": 28.4, "cintura": 80, "quadril": 101, "braco": 30, "observacoes": "Avaliação mensal" }
```

## Integração 3 (Avaliações → Alunos)
1. Valida os dados e consulta `GET /api/alunos/{id}`.
2. Aluno inexistente → **404**; inativo → **400**; Alunos fora do ar → **503** (nada é gravado).
3. Salva a avaliação com `sincronizada = false`.
4. Chama `PATCH /api/alunos/{id}/medidas` com `{ "pesoAtual": ... }`.
5. Se o PATCH deu certo → `sincronizada = true`; se falhou, continua `false` e o `POST /api/avaliacoes/sincronizar` tenta de novo.

## Configuração
| Variável | Uso |
|---|---|
| `ALUNOS_URL` | URL base do serviço de Alunos (no Render: URL pública do Alunos) |
| `ConnectionStrings__DefaultConnection` | Caminho do SQLite (ex.: `Data Source=/data/avaliacoes.db`) |

## Deploy (Render)
Web Service com Docker, raiz = pasta `avaliacoes-service` (usa o `Dockerfile` do template). Definir `ALUNOS_URL`.

## Migrations
Já vem a migration `Inicial` (tabela `Avaliacoes` + 3 avaliações de exemplo), aplicada automaticamente na inicialização.
Se mudar a entidade: `dotnet ef migrations add NomeDaMudanca` dentro de `AvaliacoesService/`.
