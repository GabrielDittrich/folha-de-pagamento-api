using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Rotas Funcionario
app.MapPost("/api/funcionario/cadastrar", ([FromBody] Funcionario funcionario, [FromServices] AppDataContext ctx) =>
{
    ctx.Funcionarios.Add(funcionario);
    ctx.SaveChanges();
    return Results.Created("", funcionario);
});

app.MapGet("/api/funcionario/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Funcionarios.Any())
    {
        return Results.Ok(ctx.Funcionarios.ToList());
    }
    return Results.NotFound();
});

app.MapPut("/api/funcionario/atualizar/{id}", async (int id, Funcionario funcionarioAtualizado, AppDataContext ctx) =>
{
    var funcionario = await ctx.Funcionarios.FindAsync(id);
    if (funcionario == null)
    {
        return Results.NotFound("Funcionario não encontrado.");
    }

    funcionario.Nome = funcionarioAtualizado.Nome;
    funcionario.Cpf = funcionarioAtualizado.Cpf;
    await ctx.SaveChangesAsync();
    return Results.Ok("Funcionario atualizado com sucesso.");
});


app.MapGet("/api/funcionario/buscar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{

    Funcionario? funcionario = ctx.Funcionarios.Find(id);

    if (funcionario is null)
    {
        return Results.NotFound("Nenhum funcionario encontrado");
    }
    return Results.Ok(funcionario);
});

app.MapDelete("/api/funcionario/deletar/{id}", async (int id, AppDataContext context) =>
{
    var funcionario = await context.Funcionarios.FindAsync(id);
    if (funcionario == null)
    {
        return Results.NotFound("Funcionario não encontrado.");
    }

    context.Funcionarios.Remove(funcionario);
    await context.SaveChangesAsync();
    return Results.Ok("Funcionario removido com sucesso.");
});


// Rotas Folha de Pagamento
app.MapPost("/api/folha/cadastrar", ([FromBody] Folha folha, [FromServices] AppDataContext ctx) =>
{
    // Validar se o funcionario existe

    Funcionario? funcionario =
    ctx.Funcionarios.Find(folha.FuncionarioId);

    if (funcionario is null)
        return Results.NotFound("Funcionario não encontrado");

    folha.Funcionario = funcionario;

    // Calcular o IRRF
    if (folha.SalarioBruto <= 1903.98)
        folha.ImpostoIRRF = 0;
    else if (folha.SalarioBruto <= 2826.65)
        folha.ImpostoIRRF = (folha.SalarioBruto * .075) - 142.80;
    else if (folha.SalarioBruto <= 3751.05)
        folha.ImpostoIRRF = (folha.SalarioBruto * .15) - 354.80;
    else if (folha.SalarioBruto <= 4664.68)
        folha.ImpostoIRRF = (folha.SalarioBruto * .225) - 636.13;
    else
        folha.ImpostoIRRF = (folha.SalarioBruto * .275) - 869.36;

    // Calcular o INSS
    if (folha.SalarioBruto <= 1603.72)
        folha.INSS = folha.SalarioBruto * .08;
    else if (folha.SalarioBruto <= 2822.90)
        folha.INSS = folha.SalarioBruto * .09;
    else if (folha.SalarioBruto <= 5645.80)
        folha.INSS = folha.SalarioBruto * .11;
    else
        folha.INSS = 621.04;

    // Calcular o FGTS
    folha.ImpostoFGTS = folha.SalarioBruto * .08;

    // Calcular o salário líquido
    folha.SalarioLiquido = folha.SalarioBruto - folha.ImpostoIRRF - folha.INSS;

    ctx.Folhas.Add(folha);
    ctx.SaveChanges();
    return Results.Created("", folha);
});

app.MapGet("/api/folha/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Folhas.Any())
    {
        return Results.Ok(ctx.Folhas.Include(x => x.Funcionario).ToList());
    }
    return Results.NotFound("Nenhuma folha encontrada");
});

app.MapGet("/api/folha/buscar/{cpf}/{mes}/{ano}", ([FromServices] AppDataContext ctx, [FromRoute] int mes, [FromRoute] int ano, [FromRoute] string cpf) =>
{

    Folha? folha = ctx.Folhas.Include(x => x.Funcionario).FirstOrDefault(f => f.Funcionario.Cpf == cpf && f.Mes == mes && f.Ano == ano);

    if (folha is null)
    {
        return Results.NotFound("Nenhuma folha encontrada");
    }
    return Results.Ok(folha);
});

app.MapDelete("/api/folha/deletar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{

    Folha? folha = ctx.Folhas.Find(id);

    if (folha is null)
    {
        return Results.NotFound("Nenhuma Folha com esse Id foi encontrado");
    }
    ctx.Folhas.Remove(folha);
    ctx.SaveChanges();
    return Results.Ok("Folha deletada");
});


app.Run();