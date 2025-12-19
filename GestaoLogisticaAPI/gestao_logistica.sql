CREATE DATABASE IF NOT EXISTS gestao_logistica
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;
USE gestao_logistica;

-- Endereços reutilizáveis
CREATE TABLE endereco (
  cod_endereco INT AUTO_INCREMENT PRIMARY KEY,
  logradouro VARCHAR(150),
  numero VARCHAR(20),
  complemento VARCHAR(100),
  bairro VARCHAR(80),
  cep CHAR(8),        -- somente números (ex: 01001000)
  cidade VARCHAR(80),
  estado CHAR(2),
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Usuários para o sistema (armazenar hash de senha)
CREATE TABLE usuario (
  cod_usuario INT AUTO_INCREMENT PRIMARY KEY,
  apelido VARCHAR(50) NOT NULL UNIQUE,
  senha VARCHAR(255) NOT NULL, -- hash (bcrypt/argon2)
  nome_completo VARCHAR(150),
  role ENUM('admin','operador','motorista') NOT NULL DEFAULT 'operador',
  ativo TINYINT(1) NOT NULL DEFAULT 1,
  criado_em TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Clientes (remetente/destinatário)
CREATE TABLE cliente (
  cod_cliente INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(150) NOT NULL,
  documento VARCHAR(20), -- cpf ou cnpj
  email VARCHAR(120),
  telefone VARCHAR(30),
  cod_endereco INT DEFAULT NULL,
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_cliente_endereco FOREIGN KEY (cod_endereco)
    REFERENCES endereco(cod_endereco) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Transportadoras
CREATE TABLE transportadora (
  cod_transportadora INT AUTO_INCREMENT PRIMARY KEY,
  cnpj CHAR(14) UNIQUE,
  nome_fantasia VARCHAR(150) NOT NULL,
  contato VARCHAR(50),
  cod_endereco INT DEFAULT NULL,
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_transportadora_endereco FOREIGN KEY (cod_endereco)
    REFERENCES endereco(cod_endereco) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Motoristas
CREATE TABLE motorista (
  cod_motorista INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(150) NOT NULL,
  cnh VARCHAR(30),
  telefone VARCHAR(30),
  cod_transportadora INT DEFAULT NULL,
  ativo TINYINT(1) DEFAULT 1,
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_motorista_transportadora FOREIGN KEY (cod_transportadora)
    REFERENCES transportadora(cod_transportadora) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Veículos (vinculados à transportadora)
CREATE TABLE veiculo (
  cod_veiculo INT AUTO_INCREMENT PRIMARY KEY,
  placa VARCHAR(10) NOT NULL UNIQUE,
  modelo VARCHAR(100),
  capacidade_kg DECIMAL(10,2),
  cod_transportadora INT DEFAULT NULL,
  status ENUM('Disponível','Em uso','Manutenção') DEFAULT 'Disponível',
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_veiculo_transportadora FOREIGN KEY (cod_transportadora)
    REFERENCES transportadora(cod_transportadora) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Armazéns simples
CREATE TABLE armazem (
  cod_armazem INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(120) NOT NULL,
  cod_endereco INT DEFAULT NULL,
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_armazem_endereco FOREIGN KEY (cod_endereco)
    REFERENCES endereco(cod_endereco) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Produtos básicos
CREATE TABLE produto (
  cod_produto INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(200) NOT NULL,
  descricao TEXT,
  sku VARCHAR(60) UNIQUE,
  peso_unit_kg DECIMAL(10,3) DEFAULT NULL,
  volume_unit_m3 DECIMAL(10,4) DEFAULT NULL,
  ativo TINYINT(1) NOT NULL DEFAULT 1,
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Estoque (por armazém)
CREATE TABLE estoque (
  cod_estoque INT AUTO_INCREMENT PRIMARY KEY,
  cod_armazem INT NOT NULL,
  cod_produto INT NOT NULL,
  quantidade INT NOT NULL DEFAULT 0,
  lote VARCHAR(80),
  data_entrada DATE DEFAULT NULL,
  atualizado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_estoque_armazem FOREIGN KEY (cod_armazem)
    REFERENCES armazem(cod_armazem) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT fk_estoque_produto FOREIGN KEY (cod_produto)
    REFERENCES produto(cod_produto) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Rotas simples (origem/destino = endereco)
CREATE TABLE rota (
  cod_rota INT AUTO_INCREMENT PRIMARY KEY,
  origem INT NOT NULL,
  destino INT NOT NULL,
  distancia_km DECIMAL(8,2) DEFAULT NULL,
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_rota_origem FOREIGN KEY (origem)
    REFERENCES endereco(cod_endereco) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT fk_rota_destino FOREIGN KEY (destino)
    REFERENCES endereco(cod_endereco) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Entregas (cabeçalho). Aqui uso ENUM simples para status, por simplicidade.
CREATE TABLE entrega (
  cod_entrega INT AUTO_INCREMENT PRIMARY KEY,
  codigo_externo VARCHAR(80),          -- código do cliente/marketplace
  cod_cliente_remetente INT DEFAULT NULL,
  cod_cliente_destinatario INT DEFAULT NULL,
  cod_transportadora INT DEFAULT NULL,
  cod_veiculo INT DEFAULT NULL,
  cod_motorista INT DEFAULT NULL,
  cod_rota INT DEFAULT NULL,
  data_pedido DATETIME DEFAULT CURRENT_TIMESTAMP,
  data_previsao_entrega DATE DEFAULT NULL,
  peso_total_kg DECIMAL(12,3) DEFAULT NULL,
  volume_total_m3 DECIMAL(12,4) DEFAULT NULL,
  valor_frete DECIMAL(12,2) DEFAULT NULL,
  status_entrega ENUM('Pendente','Em Rota','Em Entrega','Entregue','Cancelada') DEFAULT 'Pendente',
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_entrega_cliente_rem FOREIGN KEY (cod_cliente_remetente)
    REFERENCES cliente(cod_cliente) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT fk_entrega_cliente_dest FOREIGN KEY (cod_cliente_destinatario)
    REFERENCES cliente(cod_cliente) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT fk_entrega_transportadora FOREIGN KEY (cod_transportadora)
    REFERENCES transportadora(cod_transportadora) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT fk_entrega_veiculo FOREIGN KEY (cod_veiculo)
    REFERENCES veiculo(cod_veiculo) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT fk_entrega_motorista FOREIGN KEY (cod_motorista)
    REFERENCES motorista(cod_motorista) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT fk_entrega_rota FOREIGN KEY (cod_rota)
    REFERENCES rota(cod_rota) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Itens da entrega (permite múltiplos produtos por entrega)
CREATE TABLE entrega_produto (
  cod_entrega_produto INT AUTO_INCREMENT PRIMARY KEY,
  cod_entrega INT NOT NULL,
  cod_produto INT NOT NULL,
  quantidade INT NOT NULL DEFAULT 1,
  peso_unit_kg DECIMAL(10,3) DEFAULT NULL,
  volume_unit_m3 DECIMAL(10,4) DEFAULT NULL,
  criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_ep_entrega FOREIGN KEY (cod_entrega)
    REFERENCES entrega(cod_entrega) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT fk_ep_produto FOREIGN KEY (cod_produto)
    REFERENCES produto(cod_produto) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Rastreamento simples (eventos)
CREATE TABLE rastreamento (
  cod_rastreamento INT AUTO_INCREMENT PRIMARY KEY,
  cod_entrega INT NOT NULL,
  data_hora DATETIME DEFAULT CURRENT_TIMESTAMP,
  latitude DECIMAL(10,7) DEFAULT NULL,
  longitude DECIMAL(10,7) DEFAULT NULL,
  localizacao_texto VARCHAR(255) DEFAULT NULL,
  observacao VARCHAR(255) DEFAULT NULL,
  status ENUM('Pendente','Em Rota','Em Entrega','Entregue','Cancelada') DEFAULT NULL,
  CONSTRAINT fk_rastreamento_entrega FOREIGN KEY (cod_entrega)
    REFERENCES entrega(cod_entrega) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Índices úteis (simples)
CREATE INDEX idx_entrega_status ON entrega(status_entrega);
CREATE INDEX idx_entrega_cliente_dest ON entrega(cod_cliente_destinatario);
CREATE INDEX idx_rastreamento_entrega ON rastreamento(cod_entrega);

INSERT INTO usuario (apelido, senha, nome_completo, role, ativo)
VALUES (
  'admin',
  'admin',
  'Administrador do Sistema',
  'admin',
  1
);