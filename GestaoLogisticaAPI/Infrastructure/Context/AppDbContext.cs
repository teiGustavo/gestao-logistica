using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static System.Enum;

namespace GestaoLogisticaAPI.Infrastructure.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<Armazem> Armazem { get; set; }
    public virtual DbSet<Cliente> Cliente { get; set; }
    public virtual DbSet<Endereco> Endereco { get; set; }
    public virtual DbSet<Entrega> Entrega { get; set; }
    public virtual DbSet<EntregaProduto> EntregaProduto { get; set; }
    public virtual DbSet<Estoque> Estoque { get; set; }
    public virtual DbSet<Motorista> Motorista { get; set; }
    public virtual DbSet<Produto> Produto { get; set; }
    public virtual DbSet<Rastreamento> Rastreamento { get; set; }
    public virtual DbSet<Rota> Rota { get; set; }
    public virtual DbSet<Transportadora> Transportadora { get; set; }
    public virtual DbSet<Usuario> Usuario { get; set; }
    public virtual DbSet<Veiculo> Veiculo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_unicode_ci").HasCharSet("utf8mb4");

        // Armazem
        modelBuilder.Entity<Armazem>(entity =>
        {
            entity.HasKey(e => e.CodArmazem).HasName("PRIMARY");
            entity.ToTable("armazem");
            entity.HasIndex(e => e.CodEndereco, "fk_armazem_endereco");
            entity.Property(e => e.CodArmazem)
                .HasColumnName("cod_armazem")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodEndereco)
                .HasColumnName("cod_endereco")
                .HasColumnType("int(11)");
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(120);
            entity.HasOne(d => d.CodEnderecoNavigation)
                .WithMany(p => p.Armazem)
                .HasForeignKey(d => d.CodEndereco)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_armazem_endereco");
        });

        // Cliente
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.CodCliente).HasName("PRIMARY");
            entity.ToTable("cliente");
            entity.HasIndex(e => e.CodEndereco, "fk_cliente_endereco");
            entity.Property(e => e.CodCliente)
                .HasColumnName("cod_cliente")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodEndereco)
                .HasColumnName("cod_endereco")
                .HasColumnType("int(11)");
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Documento)
                .HasColumnName("documento")
                .HasMaxLength(20);
            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(120);
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150);
            entity.Property(e => e.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(30);
            entity.HasOne(d => d.CodEnderecoNavigation)
                .WithMany(p => p.Cliente)
                .HasForeignKey(d => d.CodEndereco)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_cliente_endereco");
        });

        // Endereco
        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.CodEndereco).HasName("PRIMARY");
            entity.ToTable("endereco");
            entity.Property(e => e.CodEndereco)
                .HasColumnName("cod_endereco")
                .HasColumnType("int(11)");
            entity.Property(e => e.Bairro)
                .HasColumnName("bairro")
                .HasMaxLength(80);
            entity.Property(e => e.Cep)
                .HasColumnName("cep")
                .HasMaxLength(8).IsFixedLength();
            entity.Property(e => e.Cidade)
                .HasColumnName("cidade")
                .HasMaxLength(80);
            entity.Property(e => e.Complemento)
                .HasColumnName("complemento")
                .HasMaxLength(100);
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasMaxLength(2).IsFixedLength();
            entity.Property(e => e.Logradouro)
                .HasColumnName("logradouro")
                .HasMaxLength(150);
            entity.Property(e => e.Numero)
                .HasColumnName("numero")
                .HasMaxLength(20);
        });

        // Entrega
        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.HasKey(e => e.CodEntrega).HasName("PRIMARY");
            entity.ToTable("entrega");
            entity.HasIndex(e => e.CodClienteRemetente, "fk_entrega_cliente_rem");
            entity.HasIndex(e => e.CodMotorista, "fk_entrega_motorista");
            entity.HasIndex(e => e.CodRota, "fk_entrega_rota");
            entity.HasIndex(e => e.CodTransportadora, "fk_entrega_transportadora");
            entity.HasIndex(e => e.CodVeiculo, "fk_entrega_veiculo");
            entity.HasIndex(e => e.CodClienteDestinatario, "idx_entrega_cliente_dest");
            entity.HasIndex(e => e.StatusEntrega, "idx_entrega_status");
            entity.Property(e => e.CodEntrega)
                .HasColumnName("cod_entrega")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodClienteDestinatario)
                .HasColumnName("cod_cliente_destinatario")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodClienteRemetente)
                .HasColumnName("cod_cliente_remetente")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodMotorista)
                .HasColumnName("cod_motorista")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodRota)
                .HasColumnName("cod_rota")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodTransportadora)
                .HasColumnName("cod_transportadora")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodVeiculo)
                .HasColumnName("cod_veiculo")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodigoExterno)
                .HasColumnName("codigo_externo")
                .HasMaxLength(80);
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.DataPedido)
                .HasColumnName("data_pedido")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime");
            entity.Property(e => e.DataPrevisaoEntrega)
                .HasColumnName("data_previsao_entrega")
                .HasDefaultValueSql("NULL")
                .HasColumnType("date");
            entity.Property(e => e.PesoTotalKg)
                .HasColumnName("peso_total_kg")
                .HasPrecision(12, 3);
            entity.Property(e => e.StatusEntrega)
                .HasColumnName("status_entrega")
                .HasDefaultValueSql("'Pendente'")
                .HasColumnType("enum('Pendente','Em Rota','Em Entrega','Entregue','Cancelada')");
            entity.Property(e => e.ValorFrete)
                .HasColumnName("valor_frete")
                .HasPrecision(12, 2);
            entity.Property(e => e.VolumeTotalM3)
                .HasColumnName("volume_total_m3")
                .HasPrecision(12, 4);
            entity.HasOne(d => d.CodClienteDestinatarioNavigation)
                .WithMany(p => p.EntregacodClienteDestinatarioNavigation)
                .HasForeignKey(d => d.CodClienteDestinatario)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_entrega_cliente_dest");
            entity.HasOne(d => d.CodClienteRemetenteNavigation)
                .WithMany(p => p.EntregacodClienteRemetenteNavigation)
                .HasForeignKey(d => d.CodClienteRemetente)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_entrega_cliente_rem");
            entity.HasOne(d => d.CodMotoristaNavigation)
                .WithMany(p => p.Entrega)
                .HasForeignKey(d => d.CodMotorista)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_entrega_motorista");
            entity.HasOne(d => d.CodRotaNavigation)
                .WithMany(p => p.Entrega)
                .HasForeignKey(d => d.CodRota)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_entrega_rota");
            entity.HasOne(d => d.CodTransportadoraNavigation)
                .WithMany(p => p.Entrega)
                .HasForeignKey(d => d.CodTransportadora)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_entrega_transportadora");
            entity.HasOne(d => d.CodVeiculoNavigation)
                .WithMany(p => p.Entrega)
                .HasForeignKey(d => d.CodVeiculo)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_entrega_veiculo");
        });

        // EntregaProduto
        modelBuilder.Entity<EntregaProduto>(entity =>
        {
            entity.HasKey(e => e.CodEntregaProduto).HasName("PRIMARY");
            entity.ToTable("entrega_produto");
            entity.HasIndex(e => e.CodEntrega, "fk_ep_entrega");
            entity.HasIndex(e => e.CodProduto, "fk_ep_produto");
            entity.Property(e => e.CodEntregaProduto)
                .HasColumnName("cod_entrega_produto")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodEntrega)
                .HasColumnName("cod_entrega")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodProduto)
                .HasColumnName("cod_produto")
                .HasColumnType("int(11)");
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.PesoUnitKg)
                .HasColumnName("peso_unit_kg")
                .HasPrecision(10, 3);
            entity.Property(e => e.Quantidade)
                .HasColumnName("quantidade")
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)");
            entity.Property(e => e.VolumeUnitM3)
                .HasColumnName("volume_unit_m3")
                .HasPrecision(10, 4);
            entity.HasOne(d => d.CodEntregaNavigation)
                .WithMany(p => p.Entregaproduto)
                .HasForeignKey(d => d.CodEntrega)
                .HasConstraintName("fk_ep_entrega");
            entity.HasOne(d => d.CodProdutoNavigation)
                .WithMany(p => p.Entregaproduto)
                .HasForeignKey(d => d.CodProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ep_produto");
        });

        // Estoque
        modelBuilder.Entity<Estoque>(entity =>
        {
            entity.HasKey(e => e.CodEstoque).HasName("PRIMARY");
            entity.ToTable("estoque");
            entity.HasIndex(e => e.CodProduto, "fk_estoque_produto");
            entity.HasIndex(e => e.CodArmazem, "fk_estoque_armazem");
            entity.Property(e => e.CodEstoque)
                .HasColumnName("cod_estoque")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodProduto)
                .HasColumnName("cod_produto")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodArmazem)
                .HasColumnName("cod_armazem")
                .HasColumnType("int(11)");
            entity.Property(e => e.Quantidade)
                .HasColumnName("quantidade")
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)");
            // O campo 'atualizado_em' deve ser mapeado se existir no Model
            // entity.Property(e => e.atualizado_em).HasColumnType("timestamp");
            entity.HasOne(d => d.CodProdutoNavigation)
                .WithMany(p => p.Estoque)
                .HasForeignKey(d => d.CodProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_estoque_produto");
            entity.HasOne(d => d.CodArmazemNavigation)
                .WithMany(p => p.Estoque)
                .HasForeignKey(d => d.CodArmazem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_estoque_armazem");
        });

        // Motorista
        modelBuilder.Entity<Motorista>(entity =>
        {
            entity.HasKey(e => e.CodMotorista).HasName("PRIMARY");
            entity.ToTable("motorista");
            entity.Property(e => e.CodMotorista)
                .HasColumnName("cod_motorista")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodTransportadora)
                .HasColumnName("cod_transportadora")
                .HasColumnType("int(11)");
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150);
            entity.Property(e => e.Cnh)
                .HasColumnName("cnh")
                .HasMaxLength(30);
            entity.Property(e => e.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(30);
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.HasMany(d => d.Entrega)
                .WithOne(p => p.CodMotoristaNavigation)
                .HasForeignKey(d => d.CodMotorista);
            entity.HasOne(d => d.CodTransportadoraNavigation)
                .WithMany(p => p.Motorista)
                .HasForeignKey(d => d.CodTransportadora)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_motorista_transportadora");
        });

        // Produto
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.CodProduto).HasName("PRIMARY");
            entity.ToTable("produto");
            entity.Property(e => e.CodProduto)
                .HasColumnName("cod_produto")
                .HasColumnType("int(11)");
            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150);
            entity.Property(e => e.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(500);
            entity.Property(e => e.PesoUnitKg)
                .HasColumnName("peso_unit_kg")
                .HasPrecision(10, 3);
            entity.Property(e => e.VolumeUnitM3)
                .HasColumnName("volume_unit_m3")
                .HasPrecision(10, 4);
            entity.Property(e => e.Ativo)
                .HasColumnName("ativo")
                .HasColumnType("tinyint(1)");
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.HasMany(d => d.Entregaproduto)
                .WithOne(p => p.CodProdutoNavigation)
                .HasForeignKey(d => d.CodProduto);
        });

        // Rastreamento
        modelBuilder.Entity<Rastreamento>(entity =>
        {
            entity.HasKey(e => e.CodRastreamento).HasName("PRIMARY");
            entity.ToTable("rastreamento");
            entity.HasIndex(e => e.CodEntrega, "fk_rastreamento_entrega");
            entity.Property(e => e.CodRastreamento)
                .HasColumnName("cod_rastreamento")
                .HasColumnType("int(11)");
            entity.Property(e => e.CodEntrega)
                .HasColumnName("cod_entrega")
                .HasColumnType("int(11)");
            // Propriedade 'status' não mapeada no scaffold original
            // entity.Property(e => e.status).HasMaxLength(50);
            entity.Property(e => e.DataHora) // Corrigido para 'data_hora'
                .HasColumnName("data_hora")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(30);
            entity.HasOne(d => d.CodEntregaNavigation)
                .WithMany(p => p.Rastreamento)
                .HasForeignKey(d => d.CodEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rastreamento_entrega");
        });

        // Rota
        modelBuilder.Entity<Rota>(entity =>
        {
            entity.HasKey(e => e.CodRota).HasName("PRIMARY");
            entity.ToTable("rota");
            entity.Property(e => e.CodRota)
                .HasColumnName("cod_rota")
                .HasColumnType("int(11)");
            // Propriedade 'nome' não mapeada no scaffold original
            // entity.Property(e => e.nome).HasMaxLength(120);
            entity.Property(e => e.Origem)
                .HasColumnName("origem")
                .HasMaxLength(120);
            entity.Property(e => e.Destino)
                .HasColumnName("destino")
                .HasMaxLength(120);
            entity.Property(e => e.DistanciaKm)
                .HasColumnName("distancia_km")
                .HasPrecision(12, 3);
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.HasMany(d => d.Entrega)
                .WithOne(p => p.CodRotaNavigation)
                .HasForeignKey(d => d.CodRota);
        });

        // Transportadora
        modelBuilder.Entity<Transportadora>(entity =>
        {
            entity.HasKey(e => e.CodTransportadora).HasName("PRIMARY");
            entity.ToTable("transportadora");
            entity.Property(e => e.CodTransportadora)
                .HasColumnName("cod_transportadora")
                .HasColumnType("int(11)");
            entity.Property(e => e.NomeFantasia)
                .HasColumnName("nome_fantasia")
                .HasMaxLength(150);
            entity.Property(e => e.Cnpj)
                .HasColumnName("cnpj")
                .HasMaxLength(18);
            entity.Property(e => e.Contato)
                .HasColumnName("contato")
                .HasMaxLength(50); // Corrigido para 'contato'
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.HasMany(d => d.Entrega)
                .WithOne(p => p.CodTransportadoraNavigation)
                .HasForeignKey(d => d.CodTransportadora);
        });

        // Usuario

        // Cria um conversor que converte Enum para String e vice-versa
        var converter = new ValueConverter<RoleUsuarioEnum, string>(
            v => v.ToString().ToLower(), // C# Enum para String no BD
            v => string.IsNullOrWhiteSpace(v)
                ? default
                : Parse<RoleUsuarioEnum>(v, true)
        ); // String no BD para C# Enum

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuario");
            entity.HasKey(u => u.CodUsuario);
            entity.Property(u => u.CodUsuario)
                .HasColumnName("cod_usuario")
                .HasColumnType("int(11)");
            entity.Property(u => u.Apelido)
                .HasColumnName("apelido")
                .HasMaxLength(50)
                .IsRequired();
            entity.Property(u => u.Senha)
                .HasColumnName("senha")
                .HasMaxLength(255)
                .IsRequired();
            entity.Property(e => e.NomeCompleto)
                .HasColumnName("nome_completo");
            entity.Property(u => u.Role)
                .HasConversion(converter)
                .HasColumnName("role")
                .HasMaxLength(50)
                .IsRequired();
            entity.Property(u => u.Ativo)
                .HasColumnName("ativo")
                .HasColumnType("tinyint(1)")
                .IsRequired();
            entity.Property(u => u.CriadoEm)
                .HasColumnName("criado_em")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()");
        });


        // Veiculo
        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.HasKey(e => e.CodVeiculo).HasName("PRIMARY");
            entity.ToTable("veiculo");
            entity.Property(e => e.CodVeiculo)
                .HasColumnName("cod_veiculo")
                .HasColumnType("int(11)");
            entity.Property(e => e.Placa)
                .HasColumnName("placa")
                .HasMaxLength(10);
            entity.Property(e => e.Modelo)
                .HasColumnName("modelo")
                .HasMaxLength(80);
            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.HasMany(d => d.Entrega)
                .WithOne(p => p.CodVeiculoNavigation)
                .HasForeignKey(d => d.CodVeiculo);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
