-- MySQL Script generated by MySQL Workbench
-- Sun Oct 27 18:05:54 2019
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mpmg
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mpmg
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mpmg` DEFAULT CHARACTER SET utf8 ;
USE `mpmg` ;

-- -----------------------------------------------------
-- Table `mpmg`.`Municipio`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`Municipio` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`Municipio` (
  `id_municipio` INT(11) NOT NULL AUTO_INCREMENT,
  `nome_municipio` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_municipio`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mpmg`.`TabelaUsuario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`TabelaUsuario` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`TabelaUsuario` (
  `sgdp` INT NOT NULL COMMENT 'Código identificador da tabela.\n',
  `id_municipio_referente` INT NOT NULL,
  `id_municipio` INT NOT NULL,
  `dt_geracao` DATE NOT NULL,
  `ano_referente` VARCHAR(4) NOT NULL,
  `titulo_aba_1` VARCHAR(120) NOT NULL,
  `titulo_aba_2` VARCHAR(120) NOT NULL,
  `titulo_aba_3` VARCHAR(120) NOT NULL,
  `analista_resp` VARCHAR(90) NULL,
  PRIMARY KEY (`sgdp`),
  CONSTRAINT `fk_TabelaUsuario_Municipio1`
    FOREIGN KEY (`id_municipio`)
    REFERENCES `mpmg`.`Municipio` (`id_municipio`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_TabelaUsuario_Municipio2`
    FOREIGN KEY (`id_municipio_referente`)
    REFERENCES `mpmg`.`Municipio` (`id_municipio`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_TabelaUsuario_Municipio1_idx` ON `mpmg`.`TabelaUsuario` (`id_municipio` ASC);

CREATE INDEX `fk_TabelaUsuario_Municipio2_idx` ON `mpmg`.`TabelaUsuario` (`id_municipio_referente` ASC);


-- -----------------------------------------------------
-- Table `mpmg`.`TabelaANP`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`TabelaANP` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`TabelaANP` (
  `id_municipio` INT NOT NULL,
  `mes` INT NOT NULL,
  `ano` INT NOT NULL,
  `produto` varchar(45) NOT NULL, 
  `preco_medio_revenda` DECIMAL NOT NULL,
  `preco_minimo_revenda` DECIMAL NOT NULL,
  `preco_maximo_revenda` DECIMAL NOT NULL,
  `dt_vigencia_municipio_anp` DATE NULL,
  PRIMARY KEY (`id_municipio`, `mes`, `ano`),
  CONSTRAINT `fk_TabelaANP_Municipio1`
    FOREIGN KEY (`id_municipio`)
    REFERENCES `mpmg`.`Municipio` (`id_municipio`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_TabelaANP_Municipio1_idx` ON `mpmg`.`TabelaANP` (`id_municipio` ASC);


-- -----------------------------------------------------
-- Table `mpmg`.`Departamento`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`Departamento` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`Departamento` (
  `id_dpto` INT NOT NULL,
  `nome_dpto` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_dpto`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mpmg`.`UploadTabelaFAM`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`UploadTabelaFAM` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`UploadTabelaFAM` (
  `id_upload` INT NOT NULL,
  `dt_upload` DATE NULL,
  PRIMARY KEY (`id_upload`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mpmg`.`TabelaFAM`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`TabelaFAM` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`TabelaFAM` (
  `mes` CHAR(2) NOT NULL,
  `ano` CHAR(4) NOT NULL,
  `id_upload` INT NOT NULL,
  `valor_fam` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`mes`, `ano`),
  CONSTRAINT `fk_TabelaFAM_UploadTabelaFAM1`
    FOREIGN KEY (`id_upload`)
    REFERENCES `mpmg`.`UploadTabelaFAM` (`id_upload`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_TabelaFAM_UploadTabelaFAM1_idx` ON `mpmg`.`TabelaFAM` (`id_upload` ASC);


-- -----------------------------------------------------
-- Table `mpmg`.`NotaFiscal`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`NotaFiscal` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`NotaFiscal` (
  `id_nota_fiscal` INT NOT NULL,
  `sgdp` INT NOT NULL,
  `id_dpto` INT NOT NULL,
  `mes_fam` CHAR(2) NOT NULL,
  `ano_fam` CHAR(4) NOT NULL,
  `vrtotal` DECIMAL(7,2) NOT NULL,
  `nro_folha` INT NULL,
  `dt_consulta_anp` DATE NULL,
  `veiculo` VARCHAR(50) NULL,
  `placa_veiculo` VARCHAR(7) NULL,
  `dt_emissao` DATE NULL,
  PRIMARY KEY (`id_nota_fiscal`),
  CONSTRAINT `fk_NotaFiscal_TabelaUsuario1`
    FOREIGN KEY (`sgdp`)
    REFERENCES `mpmg`.`TabelaUsuario` (`sgdp`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_NotaFiscal_Departamento1`
    FOREIGN KEY (`id_dpto`)
    REFERENCES `mpmg`.`Departamento` (`id_dpto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_NotaFiscal_TabelaFAM1`
    FOREIGN KEY (`mes_fam` , `ano_fam`)
    REFERENCES `mpmg`.`TabelaFAM` (`mes` , `ano`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_NotaFiscal_TabelaUsuario1_idx` ON `mpmg`.`NotaFiscal` (`sgdp` ASC);

CREATE INDEX `fk_NotaFiscal_Departamento1_idx` ON `mpmg`.`NotaFiscal` (`id_dpto` ASC);

CREATE INDEX `fk_NotaFiscal_TabelaFAM1_idx` ON `mpmg`.`NotaFiscal` (`mes_fam` ASC, `ano_fam` ASC);


-- -----------------------------------------------------
-- Table `mpmg`.`CupomFiscal`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`CupomFiscal` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`CupomFiscal` (
  `coo` VARCHAR(45) NOT NULL,
  `sgdp` INT NOT NULL,
  `id_nota_fiscal` INT NULL,
  `ecf` VARCHAR(45) NULL,
  `posto_referente` VARCHAR(90) NULL,
  `hodometro` INT NULL,
  `cliente` VARCHAR(90) NULL,
  `dt_emissao` DATETIME NULL,
  `qtd` INT NULL,
  `vr_unitario` DECIMAL(7,2) NULL,
  PRIMARY KEY (`coo`),
  CONSTRAINT `fk_CupomFiscal_NotaFiscal1`
    FOREIGN KEY (`id_nota_fiscal`)
    REFERENCES `mpmg`.`NotaFiscal` (`id_nota_fiscal`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_CupomFiscal_TabelaUsuario1`
    FOREIGN KEY (`sgdp`)
    REFERENCES `mpmg`.`TabelaUsuario` (`sgdp`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_CupomFiscal_NotaFiscal1_idx` ON `mpmg`.`CupomFiscal` (`id_nota_fiscal` ASC);

CREATE INDEX `fk_CupomFiscal_TabelaUsuario1_idx` ON `mpmg`.`CupomFiscal` (`sgdp` ASC);


-- -----------------------------------------------------
-- Table `mpmg`.`ItemNotaFiscal`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mpmg`.`ItemNotaFiscal` ;

CREATE TABLE IF NOT EXISTS `mpmg`.`ItemNotaFiscal` (
  `id_nota_fiscal` INT NOT NULL,
  `id_item` INT NOT NULL,
  `vrunitario` DECIMAL(7,2) NOT NULL,
  `id_tipo_combustivel` VARCHAR(45) NOT NULL,
  `qtde` INT NOT NULL,
  PRIMARY KEY (`id_nota_fiscal`, `id_item`),
  CONSTRAINT `fk_ItemNF_NF1`
    FOREIGN KEY (`id_nota_fiscal`)
    REFERENCES `mpmg`.`NotaFiscal` (`id_nota_fiscal`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_ItemNF_NF1_idx` ON `mpmg`.`ItemNotaFiscal` (`id_nota_fiscal` ASC);

INSERT INTO `Municipio` VALUES ('BELO HORIZONTE');
INSERT INTO `TabelaANP` VALUES (1, 2,  2013, 'ETANOL HIDRATADO',2.08,1.86,2.40, NULL);

-- INSERT INTO `dadosanp` VALUES 
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','ALFENAS',2.15,1.83,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','ARAGUARI',2.11,1.97,2.25),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','ARAXA',2.15,2.05,2.25),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','BARBACENA',2.24,1.99,2.39),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','BETIM',2.16,1.98,2.50),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','BOM DESPACHO',2.02,1.95,2.10),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','CAMPO BELO',2.15,1.90,2.29),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','CARATINGA',2.14,2.00,2.50),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','CATAGUASES',2.31,2.11,2.40),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','CONGONHAS',2.06,1.98,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','CONSELHEIRO LAFAIETE',2.13,1.90,2.33),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','CONTAGEM',2.03,1.87,2.20),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','CORONEL FABRICIANO',2.13,1.85,2.29),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','CURVELO',2.15,2.00,2.29),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','DIAMANTINA',2.27,2.26,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','DIVINOPOLIS',2.07,1.89,2.20),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','FORMIGA',2.08,1.99,2.29),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','FRUTAL',2.08,1.89,2.22),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','GOVERNADOR VALADARES',2.10,1.89,2.29),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','GUAXUPE',2.04,1.98,2.19),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','IBIRITE',2.07,2.00,2.20),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','IPATINGA',2.07,1.98,2.19),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','ITABIRA',2.09,1.98,2.20),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','ITAJUBA',2.05,1.99,2.19),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','ITAUNA',2.10,1.99,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','ITUIUTABA',2.06,1.89,2.20),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','JANAUBA',2.16,2.00,2.29),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','JANUARIA',2.28,2.24,2.37),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','JOAO MONLEVADE',2.05,1.92,2.13),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','JOAO PINHEIRO',2.13,2.06,2.20),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','JUIZ DE FORA',2.20,1.97,2.34),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','LAVRAS',2.23,1.96,2.35),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','LEOPOLDINA',2.23,1.99,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','MANHUACU',2.10,1.97,2.39),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','MARIANA',2.34,2.20,2.46),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','MONTE CARMELO',2.06,1.89,2.28),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','MONTES CLAROS',2.05,1.86,2.29),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','MURIAE',2.35,2.19,2.40),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','NOVA LIMA',2.25,2.08,2.50),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','OLIVEIRA',2.07,1.99,2.22),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','OURO PRETO',2.25,2.15,2.29),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','PARA DE MINAS',2.15,1.99,2.42),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','PARACATU',2.17,2.13,2.19),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','PASSOS',2.04,1.77,2.17),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','PATOS DE MINAS',2.13,1.99,2.20),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','PATROCINIO',2.19,1.99,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','POCOS DE CALDAS',2.16,2.09,2.27),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','POUSO ALEGRE',2.13,1.97,2.28),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','RIBEIRAO DAS NEVES',2.08,2.00,2.20),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','SABARA',2.13,1.99,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','SANTA LUZIA',2.09,2.00,2.32),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','SANTOS DUMONT',2.26,2.17,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','SAO JOAO DEL REI',2.12,1.96,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','SAO LOURENCO',2.07,2.04,2.20),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','SAO SEBASTIAO DO PARAISO',2.14,2.00,2.29),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','SETE LAGOAS',2.09,1.85,2.47),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','TEOFILO OTONI',2.15,1.98,2.26),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','TIMOTEO',2.12,1.99,2.40),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','TRES CORACOES',2.17,2.10,2.26),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','UBA',2.22,1.99,2.34),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','UBERABA',2.05,1.89,2.14),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','UBERLANDIA',2.01,1.80,2.10),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','UNAI',2.13,1.97,2.26),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','VARGINHA',2.11,1.95,2.30),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','VESPASIANO',2.20,2.08,2.32),
-- (1,2013,'ETANOL HIDRATADO','MINAS GERAIS','VICOSA',2.14,2.00,2.28)
-- ); 


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
