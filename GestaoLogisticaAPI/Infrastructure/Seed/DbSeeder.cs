using GestaoLogisticaAPI.Infrastructure.Context;

namespace GestaoLogisticaAPI.Infrastructure.Seed;

public static class DbSeeder {
    public static void Seed(AppDbContext context) {
        // TODO: Adicione dados iniciais aqui
        context.SaveChanges();
    }
}