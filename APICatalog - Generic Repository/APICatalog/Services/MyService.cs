namespace APICatalog.Services
{
    public class MyService : IMyService
    {
        public string Saudacao(string nome)
        {
            return $"Bem-Vindo, {nome} \n\n {DateTime.UtcNow}";
        }
    }
}
